using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace GEHelper.Core
{

    public class GEGalaxyMoon
    {
        public string id { get; set; }
        public string name { get; set; }
        public string diameter { get; set; }
    }

    public class GEGalaxyPlanet
    {
        public string id { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string planet_type { get; set; }
        public string moon_id { get; set; }
        public string debries_crystal { get; set; }
        public string debries_metal { get; set; }
        public string image { get; set; }
        public string diameter { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
        public string username { get; set; }
        public string rank { get; set; }
        public string points { get; set; }
        public int last_time { get; set; }
        public string ally_id { get; set; }
        public string ally_name { get; set; }
        public string ally_tag { get; set; }
        public string description { get; set; }
        public string vacation { get; set; }
        public string ban_account { get; set; }
        public string avatar { get; set; }
        public GEGalaxyMoon moon { get; set; }
        public string status { get; set; }
    }

    public class GalaxyList :  ObservableCollection<GEGalaxyPlanet> 
    {

    }

    public class GalaxyShowList
    {
        
        public JsonArrayObjects list { get; set; }

        private GalaxyList _list = null;

        public void Normalize()
        {
            _list = new GalaxyList();

            foreach (JsonObject curSubList in list)
            {
                foreach (KeyValuePair<string, string> curObj in curSubList)
                {
                    GEGalaxyPlanet newPlanet = curObj.Value.FromJson<GEGalaxyPlanet>();
                    _list.Add(newPlanet);
                }
            }

        }

        public GalaxyList PlanetList
        {
            get { return _list; }
        }

    }

}
