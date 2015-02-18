using System;
using ServiceStack.Text;
using System.Collections.Generic;

namespace GEHelper.Core
{
    public class GEState
    {
        public GEUser user { get; set; }
        public JsonObject planets { get; set; }
        public JsonObject planets_sorted { get; set; }
        public GEPlanetList planetList { get; set; }
        public List<GEPlanetSummary> planetSummaryList { get; set; }
        public List<GEFleet> fleetList { get; set; }
        public JsonObject fleet { get; set; }
        public string global_notification { get; set; }
        public string global_notification_type { get; set; }
        public DateTime UpdateTime { get; set; }


        public DateTime LocalUpdateTime
        { 
            get
            {
                return UpdateTime.ToLocalTime();
            }
        }

        public string SummaryText
        {
            get
            {
                string summary = "";
                summary += "per hour:  ";
                summary +=  String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", HourlyMetal, HourlyCrystal, HourlyDeuterium);
                summary += "\n";
                summary += "total:  ";
                summary +=  String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", TotalCurrentMetal, TotalCurrentCrystal, TotalCurrentDeuterium);
                summary += "\n";
 
                return summary;
            }
        }

        public string FleetSummary
        {
            get
            {
                string summary = "";

                if ((fleetList == null) || (fleetList.Count == 0))
                    summary = "no fleet flying";
                else
                {
                    int friendly = 0;
                    int neutral = 0;
                    int enemy = 0;
					int totalMetal = 0;
					int totalCrystal = 0;
					int totalDeuterium = 0;

                    foreach (GEFleet fleet in fleetList)
                    {
						if (fleet.fleet_owner == this.user.id) {
							totalMetal += int.Parse(fleet.fleet_resource_metal);
							totalCrystal += int.Parse(fleet.fleet_resource_crystal);
							totalDeuterium += int.Parse(fleet.fleet_resource_deuterium);
							friendly++;
						}
                        else
                            enemy++;
                    }
                    summary += String.Format("{0} yours, {1} enemy", friendly, enemy);
					summary += "\n";
					summary += String.Format ("M:{0:0,0}, C:{1:0,0}, D:{2:0,0}", totalMetal, totalCrystal, totalDeuterium);
                }


                return summary;
            }
        }

        public int TotalCurrentMetal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.CurrentMetal;
                    if (curPlanet.moon != null)
                        total += curPlanet.moon.CurrentMetal;
                }

                return (int)total;
            }
        }

        public int TotalCurrentCrystal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.CurrentCrystal;
                    if (curPlanet.moon != null)
                        total += curPlanet.moon.CurrentCrystal;
                }

                return (int)total;
            }
        }

        public int TotalCurrentDeuterium
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.CurrentDeuterium;
                    if (curPlanet.moon != null)
                        total += curPlanet.moon.CurrentDeuterium;
                }

                return (int)total;
            }
        }

        public int TotalMetal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.metal;
                }

                return (int)total;
            }
        }

        public int TotalCrystal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.crystal;
                }

                return (int)total;
            }
        }

        public int TotalDeuterium
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.deuterium;
                }

                return (int)total;
            }
        }

        public int HourlyMetal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.metal_perhour;
                }

                return (int)total;
            }
        }

        public int HourlyCrystal
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.crystal_perhour;
                }

                return (int)total;
            }
        }

        public int HourlyDeuterium
        {
            get
            {
                double total = 0;

                foreach(GEPlanet curPlanet in planetList)
                {
                    total += curPlanet.deuterium_perhour;
                }

                return (int)total;
            }
        }
    }
}

