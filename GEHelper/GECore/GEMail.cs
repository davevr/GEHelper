using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceStack.Text;

namespace GEHelper.Core
{
    public class Resources
    {
        public long metal { get; set; }
        public long crystal { get; set; }
        public long deuterium { get; set; }
        public long energy { get; set; }

        public Resources(long m, long c, long d, long e)
        {
            metal = m;
            crystal = c;
            deuterium = d;
            energy = e;
        }
    }

    public class Fleet
    {
        public long small_cargo { get; set; }
        public long large_cargo { get; set; }
        public long light_fighter { get; set; }
        public long heavy_fighter { get; set; }
        public long cruiser { get; set; }
        public long battleship { get; set; }
        public long colony_ship { get; set; }
        public long recycler { get; set; }
        public long espionage_probe { get; set; }
        public long bomber { get; set; }
        public long solar_satellite { get; set; }
        public long destroyer { get; set; }
        public long deathstar { get; set; }
        public long battlecruiser { get; set; }

        public Fleet Multiply(long count)
        {
            Fleet newFleet = new Fleet();
            newFleet.small_cargo = small_cargo * count;
            newFleet.large_cargo = large_cargo * count;
            newFleet.light_fighter = light_fighter * count;
            newFleet.heavy_fighter = heavy_fighter * count;
            newFleet.cruiser = cruiser * count;
            newFleet.battleship = battleship * count;
            newFleet.colony_ship = colony_ship * count;
            newFleet.recycler = recycler * count;
            newFleet.espionage_probe = espionage_probe * count;
            newFleet.bomber = bomber * count;
            newFleet.solar_satellite = solar_satellite * count;
            newFleet.destroyer = destroyer * count;
            newFleet.deathstar = deathstar * count;
            newFleet.battlecruiser = battlecruiser * count;

            return newFleet;
        }

        public static long small_cargo_metal_cost = 2000;
        public static long small_cargo_crystal_cost = 2000;
        public static long small_cargo_deuterium_cost = 0;

        public static long large_cargo_metal_cost = 6000;
        public static long large_cargo_crystal_cost = 6000;
        public static long large_cargo_deuterium_cost = 0;

        public static long light_fighter_metal_cost = 3000;
        public static long light_fighter_crystal_cost = 1000;
        public static long light_fighter_deuterium_cost = 0;

        public static long heavy_fighter_metal_cost = 6000;
        public static long heavy_fighter_crystal_cost = 4000;
        public static long heavy_fighter_deuterium_cost = 0;

        public static long cruiser_metal_cost = 20000;
        public static long cruiser_crystal_cost = 7000;
        public static long cruiser_deuterium_cost = 2000;

        public static long battleship_metal_cost = 45000;
        public static long battleship_crystal_cost = 15000;
        public static long battleship_deuterium_cost = 0;

        public static long colony_ship_metal_cost = 10000;
        public static long colony_ship_crystal_cost = 20000;
        public static long colony_ship_deuterium_cost = 10000;

        public static long recycler_metal_cost = 10000;
        public static long recycler_crystal_cost = 6000;
        public static long recycler_deuterium_cost = 2000;

        public static long espionage_probe_metal_cost = 0;
        public static long espionage_probe_crystal_cost = 1000;
        public static long espionage_probe_deuterium_cost = 0;
        
        public static long bomber_metal_cost = 50000;
        public static long bomber_crystal_cost = 25000;
        public static long bomber_deuterium_cost = 15000;
        
        public static long solar_satellite_metal_cost = 0;
        public static long solar_satellite_crystal_cost = 2000;
        public static long solar_satellite_deuterium_cost = 500;
        
        public static long destroyer_metal_cost = 60000;
        public static long destroyer_crystal_cost = 50000;
        public static long destroyer_deuterium_cost = 15000;
        
        public static long deathstar_metal_cost = 5000000;
        public static long deathstar_crystal_cost = 4000000;
        public static long deathstar_deuterium_cost = 1000000;
        
        public static long battlecruiser_metal_cost = 30000;
        public static long battlecruiser_crystal_cost = 40000;
        public static long battlecruiser_deuterium_cost = 15000;


        public string SummaryString()
        {
            string result = "";
            if (small_cargo > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} small cargos", small_cargo);
            }

            if (large_cargo > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} large cargos", large_cargo);
            }

            if (light_fighter > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} light fighters", light_fighter);
            }

            if (heavy_fighter > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} heavy fighters", heavy_fighter);
            }

            if (cruiser > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} cruisers", cruiser);
            }

            if (battleship > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} battleships", battleship);
            }

            if (recycler > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} recyclers", recycler);
            }

            if (bomber > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} bombers", bomber);
            }

            if (destroyer > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} destroyers", destroyer);
            }

            if (deathstar > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} deathstars", deathstar);
            }

            if (battlecruiser > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} battle cruisers", battlecruiser);
            }

            return result;
        }

        public void GetBuildCost(out long metal, out long crystal, out long deuterium)
        {
            metal = crystal = deuterium = 0;

            metal += small_cargo_metal_cost * small_cargo;
            crystal += small_cargo_crystal_cost * small_cargo;
            deuterium += small_cargo_deuterium_cost * small_cargo;

            metal += large_cargo_metal_cost * large_cargo;
            crystal += large_cargo_crystal_cost * large_cargo;
            deuterium += large_cargo_deuterium_cost * large_cargo;

            metal += light_fighter_metal_cost * light_fighter;
            crystal += light_fighter_crystal_cost * light_fighter;
            deuterium += light_fighter_deuterium_cost * light_fighter;

            metal += heavy_fighter_metal_cost * heavy_fighter;
            crystal += heavy_fighter_crystal_cost * heavy_fighter;
            deuterium += heavy_fighter_deuterium_cost * heavy_fighter;

            metal += cruiser_metal_cost * cruiser;
            crystal += cruiser_crystal_cost * cruiser;
            deuterium += cruiser_deuterium_cost * cruiser;

            metal += battleship_metal_cost * battleship;
            crystal += battleship_crystal_cost * battleship;
            deuterium += battleship_deuterium_cost * battleship;

            metal += colony_ship_metal_cost * colony_ship;
            crystal += colony_ship_crystal_cost * colony_ship;
            deuterium += colony_ship_deuterium_cost * colony_ship;

            metal += recycler_metal_cost * recycler;
            crystal += recycler_crystal_cost * recycler;
            deuterium += recycler_deuterium_cost * recycler;

            metal += espionage_probe_metal_cost * espionage_probe;
            crystal += espionage_probe_crystal_cost * espionage_probe;
            deuterium += espionage_probe_deuterium_cost * espionage_probe;

            metal += bomber_metal_cost * bomber;
            crystal += bomber_crystal_cost * bomber;
            deuterium += bomber_deuterium_cost * bomber;

            metal += solar_satellite_metal_cost * solar_satellite;
            crystal += solar_satellite_crystal_cost * solar_satellite;
            deuterium += solar_satellite_deuterium_cost * solar_satellite;

            metal += destroyer_metal_cost * destroyer;
            crystal += destroyer_crystal_cost * destroyer;
            deuterium += destroyer_deuterium_cost * destroyer;

            metal += deathstar_metal_cost * deathstar;
            crystal += deathstar_crystal_cost * deathstar;
            deuterium += deathstar_deuterium_cost * deathstar;

            metal += battlecruiser_metal_cost * battlecruiser;
            crystal += battlecruiser_crystal_cost * battlecruiser;
            deuterium += battlecruiser_deuterium_cost * battlecruiser;

        }
    }



    public class Defense
    {
        public long rocket_launcher { get; set; }
        public long light_laser { get; set; }
        public long heavy_laser { get; set; }
        public long gauss_cannon { get; set; }
        public long ion_cannon { get; set; }
        public long plasma_cannon { get; set; }
        public long small_shield_dome { get; set; }
        public long large_shield_dome { get; set; }
        public long anti_ballistic_missiles { get; set; }
        public long interplanetary_missiles { get; set; }

        public Defense Multiply(long count)
        {
            Defense newFleet = new Defense();
            newFleet.rocket_launcher = rocket_launcher * count;
            newFleet.light_laser = light_laser * count;
            newFleet.heavy_laser = heavy_laser * count;
            newFleet.gauss_cannon = gauss_cannon * count;
            newFleet.ion_cannon = ion_cannon * count;
            newFleet.plasma_cannon = plasma_cannon * count;
            newFleet.small_shield_dome = small_shield_dome * count;
            newFleet.large_shield_dome = large_shield_dome * count;
            newFleet.anti_ballistic_missiles = anti_ballistic_missiles * count;
            newFleet.interplanetary_missiles = interplanetary_missiles * count;

            return newFleet;
        }

        public static long rocket_launcher_metal_cost = 2000;
        public static long rocket_launcher_crystal_cost = 0;
        public static long rocket_launcher_deuterium_cost = 0;

        public static long light_laser_metal_cost = 1500;
        public static long light_laser_crystal_cost = 500;
        public static long light_laser_deuterium_cost = 0;

        public static long heavy_laser_metal_cost = 6000;
        public static long heavy_laser_crystal_cost = 2000;
        public static long heavy_laser_deuterium_cost = 0;

        public static long gauss_cannon_metal_cost = 20000;
        public static long gauss_cannon_crystal_cost = 15000;
        public static long gauss_cannon_deuterium_cost = 2000;

        public static long ion_cannon_metal_cost = 2000;
        public static long ion_cannon_crystal_cost = 6000;
        public static long ion_cannon_deuterium_cost = 0;

        public static long plasma_cannon_metal_cost = 50000;
        public static long plasma_cannon_crystal_cost = 50000;
        public static long plasma_cannon_deuterium_cost = 30000;

        public static long small_shield_dome_metal_cost = 10000;
        public static long small_shield_dome_crystal_cost = 10000;
        public static long small_shield_dome_deuterium_cost = 0;

        public static long large_shield_dome_metal_cost = 50000;
        public static long large_shield_dome_crystal_cost = 50000;
        public static long large_shield_dome_deuterium_cost = 0;

        public static long anti_ballistic_missiles_metal_cost = 8000;
        public static long anti_ballistic_missiles_crystal_cost = 0;
        public static long anti_ballistic_missiles_deuterium_cost = 2000;

        public static long interplanetary_missiles_metal_cost = 12500;
        public static long interplanetary_missiles_crystal_cost = 2500;
        public static long interplanetary_missiles_deuterium_cost = 10000;

       
        public string SummaryString()
        {
            string result = "";
            if (rocket_launcher > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} rocket launchers", rocket_launcher);
            }

            if (light_laser > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} light lasers", light_laser);
            }

            if (heavy_laser > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} heavy lasers", heavy_laser);
            }

            if (ion_cannon > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} ion cannons", ion_cannon);
            }

            if (gauss_cannon > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} gauss cannons", gauss_cannon);
            }

            if (plasma_cannon > 0)
            {
                if (!String.IsNullOrEmpty(result))
                    result += ", ";
                result += String.Format("{0} plasma turrents", plasma_cannon);
            }

            return result;
        }

        public double GetBuildTime()
        {
            long metal,  crystal,  deuterium;
            GetBuildCost(out metal, out crystal, out deuterium);

            double buildTime = 3600.0 * ((metal + crystal) / (2500.0 * (1 + double.Parse(GEServer.Instance.CurrentPlanet.shipyard)) * Math.Pow(2, double.Parse(GEServer.Instance.CurrentPlanet.nanite_factory))));

            return buildTime / 5; // to do:  find universe time...
        }

        public void GetBuildCost(out long metal, out long crystal, out long deuterium)
        {
            metal = crystal = deuterium = 0;

            metal += rocket_launcher_metal_cost * rocket_launcher;
            crystal += rocket_launcher_crystal_cost * rocket_launcher;
            deuterium += rocket_launcher_deuterium_cost * rocket_launcher;

            metal += light_laser_metal_cost * light_laser;
            crystal += light_laser_crystal_cost * light_laser;
            deuterium += light_laser_deuterium_cost * light_laser;

            metal += heavy_laser_metal_cost * heavy_laser;
            crystal += heavy_laser_crystal_cost * heavy_laser;
            deuterium += heavy_laser_deuterium_cost * heavy_laser;

            metal += gauss_cannon_metal_cost * gauss_cannon;
            crystal += gauss_cannon_crystal_cost * gauss_cannon;
            deuterium += gauss_cannon_deuterium_cost * gauss_cannon;

            metal += ion_cannon_metal_cost * ion_cannon;
            crystal += ion_cannon_crystal_cost * ion_cannon;
            deuterium += ion_cannon_deuterium_cost * ion_cannon;

            metal += plasma_cannon_metal_cost * plasma_cannon;
            crystal += plasma_cannon_crystal_cost * plasma_cannon;
            deuterium += plasma_cannon_deuterium_cost * plasma_cannon;

            metal += small_shield_dome_metal_cost * small_shield_dome;
            crystal += small_shield_dome_crystal_cost * small_shield_dome;
            deuterium += small_shield_dome_deuterium_cost * small_shield_dome;

            metal += large_shield_dome_metal_cost * large_shield_dome;
            crystal += large_shield_dome_crystal_cost * large_shield_dome;
            deuterium += large_shield_dome_deuterium_cost * large_shield_dome;

            metal += anti_ballistic_missiles_metal_cost * anti_ballistic_missiles;
            crystal += anti_ballistic_missiles_crystal_cost * anti_ballistic_missiles;
            deuterium += anti_ballistic_missiles_deuterium_cost * anti_ballistic_missiles;

            metal += interplanetary_missiles_metal_cost * interplanetary_missiles;
            crystal += interplanetary_missiles_crystal_cost * interplanetary_missiles;
            deuterium += interplanetary_missiles_deuterium_cost * interplanetary_missiles;

        }
    }

    public class BuildSpec
    {
        public string name { get; set; }
        public Defense defense { get; set; }
        public Fleet fleet { get; set; }
        public BuildSpec()
        {
            defense = new Defense();
            fleet = new Fleet();
        }

        public void GetBuildCost(out long metal, out long crystal, out long deut)
        {
            long defM, defC, defD, fleetM, fleetC, fleetD;

            defense.GetBuildCost(out defM, out defC, out defD);
            fleet.GetBuildCost(out fleetM, out fleetC, out fleetD);

            metal = defM + fleetM;
            crystal = defC + fleetC;
            deut = defD + fleetD;

        }
        public string SummaryString()
        {
            string result = "";
            result += defense.SummaryString();

            string fleetSum = fleet.SummaryString();

            if (!String.IsNullOrEmpty(fleetSum) && !String.IsNullOrEmpty(result))
                result = result + ", " + fleetSum;
            else
                result += fleetSum;
                
            return result;
        }
    }

    public class BuildSpecList : List<BuildSpec>
    {

    }
    public class Buildings
    {
        public int metal_mine { get; set; }
        public int crystal_mine { get; set; }
        public int deuterium_synthesizer { get; set; }
        public int solar_plant { get; set; }
        public int fusion_reactor { get; set; }
        public int robotics_factory { get; set; }
        public int nanite_factory { get; set; }
        public int shipyard { get; set; }
        public int metal_storage { get; set; }
        public int crystal_storage { get; set; }
        public int deuterium_storage { get; set; }
        public int research_lab { get; set; }
        public int terraformer { get; set; }
        public int moon_base { get; set; }
        public int sensor_phalanx { get; set; }
        public int jump_gate { get; set; }

        public int missile_silo { get; set; }
    }

    public class Tech
    {
        public int espionage_tech { get; set; }
        public int computer_tech { get; set; }
        public int weapons_tech { get; set; }
        public int shielding_tech { get; set; }
        public int armour_tech { get; set; }
        public int energy_tech { get; set; }
        public int hyperspace_tech { get; set; }
        public int combustion_drive_tech { get; set; }
        public int impulse_drive_tech { get; set; }
        public int hyperspace_drive_tech { get; set; }
        public int laser_tech { get; set; }
        public int ion_tech { get; set; }
        public int plasma_tech { get; set; }
        public int intergalactic_research_tech { get; set; }
        public int astrophysics_tech { get; set; }
        public int graviton_tech { get; set; }
    }

    public class MessageData
    {
        public Resources resources { get; set; }
        public Fleet ships { get; set; }
        public Defense defense { get; set; }
        public Buildings buildings { get; set; }
        public Tech tech { get; set; }
        public string target_name { get; set; }
        public int target_g { get; set; }
        public int target_s { get; set; }
        public int target_p { get; set; }
        public string target_planet_type { get; set; }
        public string destruction_message { get; set; }
    }

   

    public class MailItem
    {
        public string message_id { get; set; }
        public string message_owner { get; set; }
        public string message_sender { get; set; }
        public string message_time { get; set; }
        public string message_type { get; set; }
        public string message_from { get; set; }
        public string message_subject { get; set; }
        public string message_text { get; set; }
        public MessageData message_data { get; set; }
        public string @new { get; set; }
        public string deleted { get; set; }
    }

    public class MailList : List<MailItem> 
    { 
    }

    [DataContract]
    public class MailCount
    {
         [DataMember(Name = "new")]
        public int newCount {get; set;}
         [DataMember(Name = "total")]
        public int totalCount {get; set;}
    }

    [DataContract]
    public class MailCatalog
    {
        [DataMember(Name = "0")]
        public MailCount SpyReports { get; set; }
        [DataMember(Name = "1")]
        public MailCount PrivateMessages { get; set; }
        [DataMember(Name = "2")]
        public MailCount AllianceMessages { get; set; }
        [DataMember(Name = "3")]
        public MailCount CombatReports { get; set; }
        [DataMember(Name = "4")]
        public MailCount Type4Reports { get; set; }
        [DataMember(Name = "5")]
        public MailCount TransportationReports { get; set; }
        [DataMember(Name = "6")]
        public MailCount Type6Reports { get; set; }
        [DataMember(Name = "7")]
        public MailCount Type7Reports { get; set; }
        [DataMember(Name = "15")]
        public MailCount Type8Reports { get; set; }
        [DataMember(Name = "16")]
        public MailCount Type16Reports { get; set; }
        [DataMember(Name = "99")]
        public MailCount Type99Reports { get; set; }
        [DataMember(Name = "100")]
        public MailCount AllReports { get; set; } 

    }

    public class MailResult
    {
        public MailCatalog mailcat { get; set; }
    }
}
