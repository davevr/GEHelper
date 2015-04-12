using System;
using System.Collections.Generic;
using ServiceStack.Text;
using System.Globalization;

namespace GEHelper.Core
{
   
    
    public class GEStatusObject
    {
        public GalaxyShowList galaxyshow  {get; set;}
        public MailList maillist { get; set; }
        public List<object> authchooseserver { get; set; }
        public int status { get; set; }
        public int timestamp { get; set; }
        public string server_time { get; set; }
        public string token { get; set; }
        public GEState state { get; set; }
        public int online_users { get; set; }
        public string total_users { get; set; }

        public void Normalize()
        {
            int GMT = 0;
            string time = server_time;
            if (time.IndexOf("EST") > -1)
            {
                time = time.Substring(0, time.Length - 4);
                GMT = 5;
            }
            else if (time.IndexOf("EDT") > -1)
            {
                time = time.Substring(0, time.Length - 4);
                GMT = 4;
            }

            state.UpdateTime = DateTime.Parse(time).AddHours(GMT);

            state.planetList = new GEPlanetList();
            List<GEPlanet> moonList = new List<GEPlanet>();
            
            foreach (KeyValuePair<string, string> curObj in state.planets)
            {
                GEPlanet newPlanet = curObj.Value.FromJson<GEPlanet>();

                if (newPlanet.planet_type == "3")
                    moonList.Add(newPlanet);
                else
                    state.planetList.Add(newPlanet);
            }

            if (moonList.Count > 0)
            {
                foreach (GEPlanet curMoon in moonList)
                {
                    foreach (GEPlanet curPlanet in state.planetList)
                    {
                        if (curPlanet.moon_id == curMoon.id)
                        {
                            curPlanet.moon = curMoon;
                            break;
                        }
                    }
                }
            }

            state.planetSummaryList = new List<GEPlanetSummary>();
            List<GEPlanetSummary> moonSummaryList = new List<GEPlanetSummary>();
            
            foreach (KeyValuePair<string, string> curObj in state.planets_sorted)
            {
                GEPlanetSummary newPlanet = curObj.Value.FromJson<GEPlanetSummary>();

                if (newPlanet.planet_type == "3")
                    moonSummaryList.Add(newPlanet);
                else
                    state.planetSummaryList.Add(newPlanet);
            }
            

            if (moonSummaryList.Count > 0)
            {
                foreach (GEPlanetSummary curMoon in moonSummaryList)
                {
                    foreach (GEPlanet curPlanet in state.planetList)
                    {
                        if (curPlanet.moon_id == curMoon.id)
                        {
                            state.planetSummaryList.Find(planet => planet.id == curPlanet.id).moon = curMoon;
                            break;
                        }
                    }
                }
            }

            state.fleetList = new List<GEFleet>();
            if (!(state.fleet == null))
            {
                if (state.fleet is JsonObject)
                {
                    foreach (KeyValuePair<string, string> curObj in state.fleet)
                    {
                        GEFleet newFleet = curObj.Value.FromJson<GEFleet>();
                        if (newFleet != null)
                            state.fleetList.Add(newFleet);
                    }
                }
                
            }

            if (galaxyshow != null)
            {
                galaxyshow.Normalize();
            }


        }
    }
}

