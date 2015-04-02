using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using ServiceStack.Text;


namespace GEHelper.Core
{
    public delegate void string_callback(String theResult);
    public delegate void bool_callback(bool theResult);
    public delegate void GalaxyList_callback(GalaxyList theResult);


    public class ServerResponse
    {
        public List<object> authlogin { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
    }

    public class ServerInfo
    {
        public string name { get; set; }
        public string description { get; set; }
        public string start { get; set; }
        public string restart { get; set; }
        public string username { get; set; }
        public int open { get; set; }
    }

    public class ServerInfoSet
    {
        public ServerInfo srv1 { get; set; }
        public ServerInfo srv2 { get; set; }
        public ServerInfo srv3 { get; set; }
    }

    public class ServersList
    {
        public string default_server { get; set; }
        public ServerInfoSet list { get; set; }
    }

    public class Authservers
    {
        public ServersList servers_list { get; set; }
    }

    public class ServerListRequest
    {
        public Authservers authservers { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
    }
		


    public class GEServer
    {
        private  string _token = null;
        private  bool _isSignedIn = false;
        private  string url = "http://ge.seazonegames.com/";
        private bool _serverSelected = false;
        private GEState _serverState = null;
        private RestClient client = null;
        public GalaxyList ScanResults = null;
        public GalaxyList FilteredScanResults = null;
		public DateTime LastScanDate;

        private static GEServer _singleton = null;

        public bool IsServerSelected
        {
            get { return _serverSelected; }
        }

        public GEState ServerState
        {
            get { return _serverState; }
        }


        public static GEServer Instance
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new GEServer();
                    _singleton.InitClient();
                }
                return _singleton;
            }
        }

		public void SaveState()
		{
			AppSettings.Instance.SafeSaveSetting ("scanresults", ScanResults.ToJson ());
			AppSettings.Instance.SafeSaveSetting ("lastscandate", LastScanDate.ToJson());
		}

		private void LoadState()
		{
			string scanStr = AppSettings.Instance.SafeLoadSetting ("scanresults", "");
			if (!String.IsNullOrEmpty (scanStr)) {
				ScanResults = scanStr.FromJson<GalaxyList> ();
				string dateStr = AppSettings.Instance.SafeLoadSetting ("lastscandate", "");
				LastScanDate = dateStr.FromJson<DateTime> ();
			}
		}

        private void InitClient()
        {
            client = new RestClient(url);
            client.CookieContainer = new CookieContainer();
            ScanResults = new GalaxyList();
            FilteredScanResults = new GalaxyList();
			LoadState ();
        }

        private void MakeAPICall(string paramStr, string_callback callback)
        {
            string fullURL = "api.php?" + paramStr;
            if (!String.IsNullOrEmpty(_token))
                fullURL += "&token=" + _token;

            RestRequest request = new RestRequest(fullURL, Method.POST);

            client.ExecuteAsync(request, (response) =>
                {
                    callback(response.Content);
                });

            
        }

        public void Login(string username, string password, string_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=login";
            queryString += "&email=" + System.Net.WebUtility.UrlEncode(username);
            queryString += "&password=" + password;

            MakeAPICall(queryString, (content) =>
                {
                    ServerResponse response = content.FromJson<ServerResponse>();
                    if (!String.IsNullOrEmpty(response.token))
                    {
                        _isSignedIn = true;
                        _token = response.token;
                        callback("");
                    }
                    else
                    {
                        callback("error");
                    }
                });
        }

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
        }

        public delegate void ServerInfoSet_callback(ServerInfoSet theResult);

        public void GetServerList(ServerInfoSet_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=servers";

            MakeAPICall(queryString, (content) =>
                {
                    ServerListRequest response = content.FromJson<ServerListRequest>();
                    if (callback != null)
                        callback(response.authservers.servers_list.list);
                });
        }

        //public delegate void ServerInfoSet_callback(ServerInfoSet theResult);

        public void SetServer(string serverName, string_callback callback)
        {
            string queryString = "";
            queryString += "object=auth";
            queryString += "&action=chooseserver";
            queryString += "&server=" + serverName;

            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>(); 
                        response.Normalize();
                        _serverState = response.state;
                        if (callback != null)
                            callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("failed");
            }
            
        }

        public void SetPlanet(string planetId, string_callback callback)
        {
            string queryString = "";
            queryString += "object=planet";
            queryString += "&action=set";
            queryString += "&pid=" + planetId;

            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        if (callback != null)
                            callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("failed");
            }

        }

        public void ScanPlanet(string galaxy, string solarsystem, string planet, string type, string_callback callback)
        {
            string queryString = "";
            queryString += "object=shipyard";
            queryString += "&action=ajax";
            queryString += "&mission=6";
            queryString += "&g=" + galaxy;
            queryString += "&s=" + solarsystem;
            queryString += "&p=" + planet;
            queryString += "&t=" + type;        // 1=planet, 2=moon, 3=debris

            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        if (callback != null)
                            callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("failed");
            }

        }

        public void Refresh(string_callback callback)
        {
            string queryString = "";
            queryString += "live=1";

            MakeAPICall(queryString, (content) =>
                {
                    try 
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>(); 
                        response.Normalize();
                        _serverState = response.state;
                        if (callback != null)
                            callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("failed");
                    }
                });
        }

        public void ScanGalaxy(int galaxy, int system, GalaxyList_callback callback)
        {
            string queryString = "";
            queryString += "object=galaxy";
            queryString += "&action=show";
            queryString += "&g=" + galaxy.ToString();
            queryString += "&s=" + system.ToString();
            queryString += "live=1";


            MakeAPICall(queryString, (content) =>
            {
                try
                {
                    GEStatusObject response = content.FromJson<GEStatusObject>(); 
                    response.Normalize();
                    _serverState = response.state;
                    if (callback != null)
                    {
                        if (response.galaxyshow != null)
                            callback(response.galaxyshow.PlanetList);
                        else
                            callback(null);
                    }
                }
                catch (Exception)
                {
                    if (callback != null)
                        callback(null);
                }
            });
        }


    }


}

