using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }

    public class FleetSpec : Fleet
    {
        public string name { get; set; }
    }

    public class FleetSpecList : List<FleetSpec>
    {

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
    }

    public class DefenseSpec : Defense
    {
        public string name { get; set; }
    }

    public class DefenseSpecList : List<DefenseSpec>
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
}
