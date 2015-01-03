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
                string summary = "Your planets have a total of ";
                summary += TotalCurrentMetal.ToString("#,#") + " metal, ";
                summary += TotalCurrentCrystal.ToString("#,#") + " crystal, and ";
                summary += TotalCurrentDeuterium.ToString("#,#") + " deuterium.  \n";
                summary += "They are generating ";
                summary += HourlyMetal.ToString("#,#") + " metal, ";
                summary += HourlyCrystal.ToString("#,#") + " crystal, and ";
                summary += HourlyDeuterium.ToString("#,#") + " deuterium per hour.";
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

