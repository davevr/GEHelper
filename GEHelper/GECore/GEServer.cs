using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using ServiceStack.Text;
using System.Globalization;


namespace GEHelper.Core
{
    public delegate void string_callback(String theResult);
    public delegate void bool_callback(bool theResult);
    public delegate void GalaxyList_callback(GalaxyList theResult);
    public delegate void MailList_callback(MailList theResult);
  

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
        private string _version = "1.7.3";
        private  bool _isSignedIn = false;
        private  string url = "http://ge.seazonegames.com/";
        private bool _serverSelected = false;
        private GEState _serverState = null;
        private RestClient client = null;
        public GalaxyList ScanResults = null;
        public GalaxyList FilteredScanResults = null;
        public BuildSpecList BuildSpecs = null;


        private static GEServer _singleton = null;

        public bool IsServerSelected
        {
            get { return _serverSelected; }
        }

        public bool IsFleetSlotAvailable
        {
            get 
            {
                int numSlots = ServerState.fleetList.FindAll(e => e.fleet_owner == ServerState.user.id).Count;
                int maxSlots = int.Parse(ServerState.user.computer_tech) + 1;

                return (numSlots < maxSlots);
            }
        }

        public DateTime GetFirstFleetReturnTime()
        {
            CultureInfo enUS = new CultureInfo("en-US"); 
            DateTime curDateTime;
            DateTime minDateTime = DateTime.Now.AddYears(100);
            string dateFormat = "M/dd/yyyy h:mm:ss tt EST";

            foreach (GEFleet curFleet in ServerState.fleetList)
            {
                if (curFleet.fleet_owner == ServerState.user.id)
                {
                    int numSeconds = int.Parse(curFleet.fleet_end_time);
                    curDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    curDateTime.AddSeconds(numSeconds);

                    if (curDateTime < minDateTime)
                        minDateTime = curDateTime;
                }
            }

            return minDateTime;

        }

        public GEState ServerState
        {
            get { return _serverState; }
        }

		public GEPlanet	CurrentPlanet {
			get {
				string curPlanetId = ServerState.user.current_planet;
				foreach (GEPlanet curPlanet in ServerState.planetList) {
					if (curPlanet.id == curPlanetId)
						return curPlanet;
				}
				return null;
			}
		}

		public GEPlanet	GetNearestPlanet(int galaxy, int solarsystem, int planet) {
			GEPlanet closest = null;
			int minDistance = int.MaxValue;
			foreach (GEPlanet curPlanet in ServerState.planetList) {
				int distanceTo = curPlanet.GetTravelDistance (galaxy, solarsystem, planet);
				if (distanceTo < minDistance) {
					minDistance = distanceTo;
					closest = curPlanet;
				}
			}

			return closest;
		}

		public GEPlanet	GetNearestPlanetInGalaxy(int galaxy, int solarsystem, int maxDistance) {
			GEPlanet closest = null;
			int minDistance = int.MaxValue;
			foreach (GEPlanet curPlanet in ServerState.planetList) {
				int distanceTo = curPlanet.GetDistanceInGalaxy(galaxy, solarsystem);
				if (distanceTo < minDistance) {
					minDistance = distanceTo;
					closest = curPlanet;
				}
			}
			if (minDistance <= maxDistance)
				return closest;
			else
				return null;
		}

		public GEPlanet	GetLanxable(int galaxy, int solarsystem) {
			foreach (GEPlanet curPlanet in ServerState.planetList) {
				if (curPlanet.CanLanx(galaxy, solarsystem))
					return curPlanet;
				}
			return null;
		}

        public void BuildSpecOnAllPlanets(BuildSpec theSpec, string_callback callback)
        {
            BuildSpecOnNthPlanet(0, theSpec, callback);
        }

        private void BuildSpecOnNthPlanet(int curPlanetIndex, BuildSpec curSpec, string_callback callback)
        {
            GEPlanet curPlanet = ServerState.planetList[curPlanetIndex];
            long defMetal, defCrystal, defDeut;

            curSpec.GetBuildCost(out defMetal, out defCrystal, out defDeut);


            SetPlanet(curPlanet.id, (theResult) =>
            {
                // now, build the spec
                long maxCount = long.MaxValue;
                long curCount;

                curCount = (long)curPlanet.metal / defMetal;
                if (curCount < maxCount)
                    maxCount = curCount;

                curCount = (long)curPlanet.crystal / defCrystal;
                if (curCount < maxCount)
                    maxCount = curCount;

                curCount = (long)curPlanet.deuterium / defDeut;
                if (curCount < maxCount)
                    maxCount = curCount;

                if (maxCount > 0)
                {
                    Fleet fleetList = curSpec.fleet.Multiply(maxCount);

                    BuildFleet(fleetList, (fleetResult) =>
                    {
                        Defense defList = curSpec.defense.Multiply(maxCount);

                        BuildDefense(defList, (defResult) =>
                        {
                            ContinueBuildOnNextPlanet(curPlanetIndex, curSpec, callback);
                        });

                    });
                }
                else
                {
                    // no room on this planet, just move on
                    ContinueBuildOnNextPlanet(curPlanetIndex, curSpec, callback);
                }
                    

            });
        }

        private void ContinueBuildOnNextPlanet(int curPlanetIndex, BuildSpec curSpec, string_callback callback)
        {
            curPlanetIndex++;
            if (curPlanetIndex >= ServerState.planetList.Count)
                callback("ok");
            else
                BuildSpecOnNthPlanet(curPlanetIndex, curSpec, callback);
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

            AppSettings.Instance.SafeSaveSetting("buildspecs", BuildSpecs.ToJson());
		}

		private void LoadState()
		{
			try {
				string scanStr = AppSettings.Instance.SafeLoadSetting ("scanresults", "");
				if (!String.IsNullOrEmpty (scanStr)) {
					ScanResults = scanStr.FromJson<GalaxyList> ();
           
				}
                
			} catch (Exception exp) {
                ScanResults = new GalaxyList();
			}

            try
            {
                string scanStr = AppSettings.Instance.SafeLoadSetting("buildspecs", "");
                if (!String.IsNullOrEmpty(scanStr))
                    BuildSpecs = scanStr.FromJson<BuildSpecList>();
            }
            catch (Exception exp)
            {
                

            }

           
		}
        
        private void InitClient()
        {
            client = new RestClient(url);
            client.CookieContainer = new CookieContainer();
            ScanResults = new GalaxyList();
            FilteredScanResults = new GalaxyList();
            CreateDefaulBuildSpecs();
			LoadState ();
        }

        private void CreateDefaulBuildSpecs()
        {
            BuildSpecs = new BuildSpecList();
            BuildSpec fleetSpec = new BuildSpec();
            fleetSpec.name = "Standard Attack Fleet";
            fleetSpec.fleet.small_cargo = 15;
            fleetSpec.fleet.light_fighter = 45;
            fleetSpec.fleet.heavy_fighter = 15;
            fleetSpec.fleet.cruiser = 5;
            fleetSpec.fleet.battleship = 1;
            fleetSpec.fleet.recycler = 10;
            fleetSpec.fleet.battlecruiser = 1;
            BuildSpecs.Add(fleetSpec);

            BuildSpec defSpec = new BuildSpec();
            defSpec.name = "Standard Defense";
            defSpec.defense.rocket_launcher = 32;
            defSpec.defense.light_laser = 16;
            defSpec.defense.heavy_laser = 8;
            defSpec.defense.gauss_cannon = 2;
            defSpec.defense.ion_cannon = 4;
            defSpec.defense.plasma_cannon = 1;
            BuildSpecs.Add(defSpec);


        }

       

        private void MakeAPICall(string paramStr, string_callback callback)
        {
            MakeAPICallRetry(paramStr, 3, callback);
        }

        private void MakeAPICallRetry(string paramStr, int retriesLeft, string_callback callback)
        {
            string fullURL = "api.php?" + paramStr;
            fullURL += "&version=" + _version;
            fullURL += "&live=1";
            if (!String.IsNullOrEmpty(_token))
                fullURL += "&token=" + _token;

            RestRequest request = new RestRequest(fullURL, Method.POST);
            request.Timeout = 3000;

            client.ExecuteAsync(request, (response) =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (retriesLeft > 0)
                    {
                        retriesLeft--;
                        MakeAPICallRetry(paramStr, retriesLeft, callback);
                    }
                    else
                        callback("");
                }
                else
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

        public void SpyPlanet(int galaxy, int solarsystem, int planet, string type, string_callback callback)
        {
            string queryString = "";
            queryString += "object=shipyard";
            queryString += "&action=ajax";
            queryString += "&mission=6";
            queryString += "&g=" + galaxy;
            queryString += "&s=" + solarsystem;
            queryString += "&p=" + planet;
            queryString += "&t=" + type;        // 1=planet, 3=moon, 2=debris

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

        public void DeployFleet(string galaxy, string solarsystem, string planet, string type, string speed, 
             Resources rez, Fleet fleetList, string_callback callback)
        {

            DoFleetAction(galaxy, solarsystem, planet, type, "10", "4", "0", "0", rez, fleetList, callback);
        }

        public void TransportFleet(string galaxy, string solarsystem, string planet, string type, string speed,
            Resources rez, Fleet fleetList, string_callback callback)
        {

            DoFleetAction(galaxy, solarsystem, planet, type, "10", "4", "0", "0", rez, fleetList, callback);
        }

        public void Attack(string galaxy, string solarsystem, string planet, string type, string speed,
           Fleet fleetList, string_callback callback)
        {

            DoFleetAction(galaxy, solarsystem, planet, type, "10", "4", "0", "0", null, fleetList, callback);
        }


        public void DoFleetAction(string galaxy, string solarsystem, string planet, string type, string speed, 
            string mission, string fleetGroup, string stayTime, Resources rez, Fleet fleetList, 
            string_callback callback)
        {
            string queryString = "";
            queryString += "object=shipyard";
            queryString += "&action=ajax";
            queryString += "&mission=6";
            queryString += "&g=" + galaxy;
            queryString += "&s=" + solarsystem;
            queryString += "&p=" + planet;
            queryString += "&t=" + type;        // 1=planet, 3=moon, 2=debris
            queryString += "&speed=" + speed; // 1-10
            queryString += "&mission=" + mission;  //1=attack, 3=transport, 4=deploy, 6=spy
            queryString += "&fleet_group=" + fleetGroup;  // 0 = none
            queryString += "&stay=" + stayTime; // 0 = none

            if (rez != null)
            {
                queryString += "&metal=" + rez.metal;
                queryString += "&crystal=" + rez.crystal;
                queryString += "&deuterium=" + rez.deuterium;
            }
            else
            {
                queryString += "&metal=0&crystal=0&deuterium=0";
            }


            if (fleetList != null)
            {
                if (fleetList.small_cargo > 0 )
                    queryString += "&ships[small_cargo]=" + fleetList.small_cargo;
                if (fleetList.large_cargo > 0)
                    queryString += "&ships[large_cargo]=" + fleetList.large_cargo;
                if (fleetList.light_fighter > 0)
                    queryString += "&ships[light_fighter]=" + fleetList.light_fighter;
                if (fleetList.heavy_fighter > 0)
                    queryString += "&ships[heavy_fighter]=" + fleetList.heavy_fighter;
                if (fleetList.cruiser > 0)
                    queryString += "&ships[cruiser]=" + fleetList.cruiser;
                if (fleetList.battleship > 0)
                    queryString += "&ships[battleship]=" + fleetList.battleship;
                if (fleetList.colony_ship > 0)
                    queryString += "&ships[colony_ship]=" + fleetList.colony_ship;
                if (fleetList.recycler > 0)
                    queryString += "&ships[recycler]=" + fleetList.recycler;
                if (fleetList.espionage_probe > 0)
                    queryString += "&ships[espionage_probe]=" + fleetList.espionage_probe;
                if (fleetList.bomber > 0)
                    queryString += "&ships[bomber]=" + fleetList.bomber;
                if (fleetList.solar_satellite > 0)
                    queryString += "&ships[solar_satellite]=" + fleetList.solar_satellite;
                if (fleetList.destroyer > 0)
                    queryString += "&ships[destroyer]=" + fleetList.destroyer;
                if (fleetList.deathstar > 0)
                    queryString += "&ships[deathstar]=" + fleetList.deathstar;
                if (fleetList.battlecruiser > 0)
                    queryString += "&ships[battlecruiser]=" + fleetList.battlecruiser;
            }
            

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

      
        // mailtypes = 0 = spy
        public void CheckMail(string mailtype, MailList_callback callback)
        {
            string queryString = "";
            queryString += "object=mail";
            queryString += "&action=list";
            queryString += "&type=" + mailtype;


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
                        {
                            if (response.maillist != null)
                                callback(response.maillist);
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
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback(null);
            }
        }

        public void RecallFleet(string fleetId, string_callback callback)
        {
            string queryString = "";
            queryString += "object=planet";
            queryString += "&action=recall";
            queryString += "&sid=" + fleetId;


            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("fail");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("fail");
            }
        }

        public void BuildFleet(Fleet fleetList, string_callback callback)
        {
            if (fleetList.small_cargo > 0)
            {
                BuildShips("small_cargo", (int)fleetList.small_cargo, (resultStr) =>
                    {
                        fleetList.small_cargo = 0;
                        BuildFleet(fleetList, callback);
                    });
            }
            else if (fleetList.large_cargo > 0)
            {
                BuildShips("large_cargo", (int)fleetList.large_cargo, (resultStr) =>
                {
                    fleetList.large_cargo = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.light_fighter > 0)
            {
                BuildShips("light_fighter", (int)fleetList.light_fighter, (resultStr) =>
                {
                    fleetList.light_fighter = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.heavy_fighter > 0)
            {
                BuildShips("heavy_fighter", (int)fleetList.heavy_fighter, (resultStr) =>
                {
                    fleetList.heavy_fighter = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.cruiser > 0)
            {
                BuildShips("cruiser", (int)fleetList.cruiser, (resultStr) =>
                {
                    fleetList.cruiser = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.battleship > 0)
            {
                BuildShips("battleship", (int)fleetList.battleship, (resultStr) =>
                {
                    fleetList.battleship = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.colony_ship > 0)
            {
                BuildShips("colony_ship", (int)fleetList.colony_ship, (resultStr) =>
                {
                    fleetList.colony_ship = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.recycler > 0)
            {
                BuildShips("recycler", (int)fleetList.recycler, (resultStr) =>
                {
                    fleetList.recycler = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.espionage_probe > 0)
            {
                BuildShips("espionage_probe", (int)fleetList.espionage_probe, (resultStr) =>
                {
                    fleetList.espionage_probe = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.bomber > 0)
            {
                BuildShips("bomber", (int)fleetList.bomber, (resultStr) =>
                {
                    fleetList.bomber = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.solar_satellite > 0)
            {
                BuildShips("solar_satellite", (int)fleetList.solar_satellite, (resultStr) =>
                {
                    fleetList.solar_satellite = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.destroyer > 0)
            {
                BuildShips("destroyer", (int)fleetList.destroyer, (resultStr) =>
                {
                    fleetList.destroyer = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.deathstar > 0)
            {
                BuildShips("deathstar", (int)fleetList.deathstar, (resultStr) =>
                {
                    fleetList.deathstar = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else if (fleetList.battlecruiser > 0)
            {
                BuildShips("battlecruiser", (int)fleetList.battlecruiser, (resultStr) =>
                {
                    fleetList.battlecruiser = 0;
                    BuildFleet(fleetList, callback);
                });
            }
            else
                callback("ok");

        }

        public void BuildShips(string shipType, int count, string_callback callback)
        {
            string queryString = "";
            queryString += "object=ships";
            queryString += "&action=add";
            queryString += "&prod=" + shipType;
            queryString += "&count=" + count;


            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("fail");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("fail");
            }
        }


        // mailtype:  0 = spy, 5 = tranport
        public void DeleteAllMail(string mailtype, string_callback callback)
        {
            string queryString = "";
            queryString += "object=mail";
            queryString += "&action=deleteall";
            queryString += "&type=" + mailtype;


            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("fail");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("fail");
            }
        }

        public void BuildDefense(Defense defList, string_callback callback)
        {
            if (defList.rocket_launcher > 0)
            {
                BuildDefenseItem("rocket_launcher", (int)defList.rocket_launcher, (resultStr) =>
                {
                    defList.rocket_launcher = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.light_laser > 0)
            {
                BuildDefenseItem("light_laser", (int)defList.light_laser, (resultStr) =>
                {
                    defList.light_laser = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.heavy_laser > 0)
            {
                BuildDefenseItem("heavy_laser", (int)defList.heavy_laser, (resultStr) =>
                {
                    defList.heavy_laser = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.gauss_cannon > 0)
            {
                BuildDefenseItem("gauss_cannon", (int)defList.gauss_cannon, (resultStr) =>
                {
                    defList.gauss_cannon = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.ion_cannon > 0)
            {
                BuildDefenseItem("ion_cannon", (int)defList.ion_cannon, (resultStr) =>
                {
                    defList.ion_cannon = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.plasma_cannon > 0)
            {
                BuildDefenseItem("plasma_cannon", (int)defList.plasma_cannon, (resultStr) =>
                {
                    defList.plasma_cannon = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.small_shield_dome > 0)
            {
                BuildDefenseItem("small_shield_dome", (int)defList.small_shield_dome, (resultStr) =>
                {
                    defList.small_shield_dome = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.large_shield_dome > 0)
            {
                BuildDefenseItem("large_shield_dome", (int)defList.large_shield_dome, (resultStr) =>
                {
                    defList.large_shield_dome = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.anti_ballistic_missiles > 0)
            {
                BuildDefenseItem("anti_ballistic_missiles", (int)defList.anti_ballistic_missiles, (resultStr) =>
                {
                    defList.anti_ballistic_missiles = 0;
                    BuildDefense(defList, callback);
                });
            }
            else if (defList.interplanetary_missiles > 0)
            {
                BuildDefenseItem("interplanetary_missiles", (int)defList.interplanetary_missiles, (resultStr) =>
                {
                    defList.interplanetary_missiles = 0;
                    BuildDefense(defList, callback);
                });
            }
            else
                callback("ok");

        }

        public void BuildDefenseItem(string defType, int count, string_callback callback)
        {
            string queryString = "";
            queryString += "object=defense";
            queryString += "&action=add";
            queryString += "&prod=" + defType;
            queryString += "&count=" + count;


            try
            {
                MakeAPICall(queryString, (content) =>
                {
                    try
                    {
                        GEStatusObject response = content.FromJson<GEStatusObject>();
                        response.Normalize();
                        _serverState = response.state;
                        callback("ok");
                    }
                    catch (Exception)
                    {
                        if (callback != null)
                            callback("fail");
                    }
                });
            }
            catch (Exception exp)
            {
                // to do:  do something
                if (callback != null)
                    callback("fail");
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

