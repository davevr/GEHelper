using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

namespace GEHelper.Core
{
    public class GEPlanet : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void UpdateForTime()
        {
            DateTime curTime = DateTime.Now;
            double hours = (curTime - _lastUpdateTime).TotalHours;
            metal += metal_perhour * hours;
            crystal += crystal_perhour * hours;
            deuterium += deuterium_perhour * hours;
            _lastUpdateTime = curTime;
        }

        private double _metal;
        private double _crystal;
        private double _deuterium;

        private DateTime _lastUpdateTime = DateTime.Now;

        public string id { get; set; }
        public string name { get; set; }
        public string user_id { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string clan_id { get; set; }
        public string created { get; set; }
        public int last_update { get; set; }
        public string last_activity_update { get; set; }
        public string planet_type { get; set; }
        public string moon_id { get; set; }
        public string b_building { get; set; }
        public string b_building_id { get; set; }
        public string b_tech_started { get; set; }
        public string b_tech { get; set; }
        public string b_tech_id { get; set; }
        public int b_shipyard { get; set; }
        public string b_shipyard_id { get; set; }
        public string image { get; set; }
        public string diameter { get; set; }
        public string field_current { get; set; }
        public string field_max { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
        public double metal 
        { 
            get { return _metal; } 
            set 
            { 
                SetField<double>(ref _metal, value, "metal");
            }
        }

        public double crystal 
        { 
            get { return _crystal; } 
            set 
            { 
                SetField<double>(ref _crystal, value, "crystal");
            }
        }

        public double deuterium 
        { 
            get { return _deuterium; } 
            set 
            { 
                SetField<double>(ref _deuterium, value, "deuterium");
            }
        }

		public bool CanLanx(int galaxy, int solarsystem) {
			if (this.moon != null) {
				int lanxLevel = 0;
				int.TryParse(this.moon.sensor_phalanx, out lanxLevel);
				int lanxDistance = (lanxLevel * lanxLevel) - 1;
				if (galaxy == int.Parse(g)) {
					int spread = Math.Abs (solarsystem - int.Parse (s));
					return spread <= lanxDistance;
				} else 
					return false; // can't lanx across galaxy
			} else {
				return false; // no moon
			}
		}

		public int GetDistanceInGalaxy(int galaxy, int solarsystem) {
			if (galaxy == int.Parse (g)) {
				int spread = Math.Abs (solarsystem - int.Parse (s));
				return spread;
			} else
				return int.MaxValue;	
		}


		public int GetTravelDistance(int galaxy, int solarsystem, int planet) {
			int flightDistance = int.MaxValue;

			if (galaxy == int.Parse (g)) {
				// within galaxy
				if (solarsystem == int.Parse (s)) {
					// within system
					flightDistance = 1000 + 5 * Math.Abs (planet - int.Parse (p));
				} else {
					// across the system
					flightDistance = 2700 + 95 * Math.Abs (solarsystem - int.Parse (s));
				}
			} else {
				
				flightDistance = 20000 * Math.Abs (galaxy - int.Parse (g));
			}

			return flightDistance;
		}

		public int GetTravelTime(int galaxy, int solarsystem, int planet, int minVel, int speed) {
			int flightDistance = GetTravelDistance(galaxy, solarsystem, planet);
			int time = (int)(10 + 3500 * Math.Sqrt ((10 * flightDistance) / minVel));
			if (speed != 100)
				time = (int)((double)time / (speed / 100d));

			return time;
		}



        public int metal_perhour { get; set; }
        public int metal_max { get; set; }
        //public double crystal { get; set; }
        public int crystal_perhour { get; set; }
        public int crystal_max { get; set; }
        //public double deuterium { get; set; }
        public int deuterium_perhour { get; set; }
        public int deuterium_max { get; set; }
        public int energy_used { get; set; }
        public int energy_max { get; set; }
        public string debries_crystal { get; set; }
        public string debries_metal { get; set; }
        public string metal_mine { get; set; }
        public string crystal_mine { get; set; }
        public string deuterium_synthesizer { get; set; }
        public string solar_plant { get; set; }
        public string fusion_reactor { get; set; }
        public string robotics_factory { get; set; }
        public string nanite_factory { get; set; }
        public string shipyard { get; set; }
        public string metal_storage { get; set; }
        public string crystal_storage { get; set; }
        public string deuterium_storage { get; set; }
        public string research_lab { get; set; }
        public string terraformer { get; set; }
        public string moon_base { get; set; }
        public string sensor_phalanx { get; set; }
        public string jump_gate { get; set; }
        public string missile_silo { get; set; }
        public string small_cargo { get; set; }
        public string large_cargo { get; set; }
        public string light_fighter { get; set; }
        public string heavy_fighter { get; set; }
        public string cruiser { get; set; }
        public string battleship { get; set; }
        public string colony_ship { get; set; }
        public string recycler { get; set; }
        public string espionage_probe { get; set; }
        public string bomber { get; set; }
        public string solar_satellite { get; set; }
        public string destroyer { get; set; }
        public string deathstar { get; set; }
        public string battlecruiser { get; set; }
        public string rocket_launcher { get; set; }
        public string light_laser { get; set; }
        public string heavy_laser { get; set; }
        public string gauss_cannon { get; set; }
        public string ion_cannon { get; set; }
        public string plasma_cannon { get; set; }
        public string small_shield_dome { get; set; }
        public string large_shield_dome { get; set; }
        public string anti_ballistic_missiles { get; set; }
        public string interplanetary_missiles { get; set; }
        public string metal_mine_percent { get; set; }
        public string crystal_mine_percent { get; set; }
        public string deuterium_synthesizer_percent { get; set; }
        public string solar_plant_percent { get; set; }
        public string fusion_reactor_percent { get; set; }
        public string solar_satellite_percent { get; set; }
        public string last_jump_time { get; set; }
        public int moved { get; set; }
        public string show_in_scoreboard { get; set; }
        public int BASE_STORAGE_SIZE { get; set; }
        public int METAL_BASIC_INCOME { get; set; }
        public int CRYSTAL_BASIC_INCOME { get; set; }
        public int DEUTERIUM_BASIC_INCOME { get; set; }
        public int ENERGY_BASIC_INCOME { get; set; }
        public GEPlanet moon { get; set; }

        // computed

        public double CurrentMetal
        {
            get
            {
                
                DateTime startTime = GEServer.Instance.ServerState.LocalUpdateTime;
                TimeSpan elapsedTime = DateTime.Now - startTime;
                double hours = elapsedTime.TotalHours;
                double newMetal = metal_perhour * hours;

                double totalMetal = metal + newMetal;
                if (totalMetal > metal_max)
                {
                    if (metal > metal_max)
                        return metal;
                    else
                        return metal_max;
                }
                return totalMetal;
            }
        }

        public double CurrentCrystal
        {
            get
            {
                DateTime startTime = GEServer.Instance.ServerState.LocalUpdateTime;
                TimeSpan elapsedTime = DateTime.Now - startTime;
                double hours = elapsedTime.TotalHours;
                double newCrystal = crystal_perhour * hours;

                double totalCrystal = crystal + newCrystal;
                if (totalCrystal > crystal_max)
                {
                    if (crystal > crystal_max)
                        return crystal;
                    else
                        return crystal_max;
                }
                return totalCrystal;
            }
        }

        public double CurrentDeuterium
        {
            get
            {
                DateTime startTime = GEServer.Instance.ServerState.LocalUpdateTime;
                TimeSpan elapsedTime = DateTime.Now - startTime;
                double hours = elapsedTime.TotalHours;
                double newDeuterium = deuterium_perhour * hours;

                double totalDeuterium = deuterium + newDeuterium;
                if (totalDeuterium > deuterium_max)
                {
                    if (deuterium > deuterium_max)
                        return deuterium;
                    else
                        return deuterium_max;
                }
                return totalDeuterium;
            }
        }
    }

    public class GEPlanetList : ObservableCollection<GEPlanet> 
    {
        public void UpdateForTime()
        {
            foreach (GEPlanet curPlanet in this)
            {
                curPlanet.UpdateForTime();
            }
        }
    }

    public class PlanetSummaryData
    {
        public Resources resources { get; set; }
        public Fleet ships { get; set; }
        public Defense defense { get; set; }
        public Buildings buildings { get; set; }
        public Tech tech { get; set; }
        public DateTime lastUpdated { get; set; }
       
    }

    public class GEPlanetSummary
    {
        public string id { get; set; }
        public string name { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string planet_type { get; set; }
        public GEPlanetSummary moon {get; set;}

        public PlanetSummaryData data { get; set; }
    }


}

