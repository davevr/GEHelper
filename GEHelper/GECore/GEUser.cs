using System;

namespace GEHelper.Core
{
    public class GEUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string federated_id { get; set; }
        public string planet_id { get; set; }
        public string g { get; set; }
        public string s { get; set; }
        public string p { get; set; }
        public string current_planet { get; set; }
        public string rank { get; set; }
        public string points { get; set; }
        public string fights { get; set; }
        public string fights_win { get; set; }
        public string fights_loot { get; set; }
        public string fights_lost { get; set; }
        public string avatar { get; set; }
        public string ally_id { get; set; }
        public string ally_name { get; set; }
        public string ally_tag { get; set; }
        public string ally_rank { get; set; }
        public string ally_stat { get; set; }
        public string ally_joined { get; set; }
        public string ally_apply_id { get; set; }
        public string ally_apply_name { get; set; }
        public string ally_apply_tag { get; set; }
        public string ally_apply_text { get; set; }
        public string description { get; set; }
        public string has_mail { get; set; }
        public string send_spys { get; set; }
        public string chat_color { get; set; }
        public string vacation { get; set; }
        public string b_tech_planet { get; set; }
        public string espionage_tech { get; set; }
        public string computer_tech { get; set; }
        public string weapons_tech { get; set; }
        public string shielding_tech { get; set; }
        public string armour_tech { get; set; }
        public string energy_tech { get; set; }
        public string hyperspace_tech { get; set; }
        public string combustion_drive_tech { get; set; }
        public string impulse_drive_tech { get; set; }
        public string hyperspace_drive_tech { get; set; }
        public string laser_tech { get; set; }
        public string ion_tech { get; set; }
        public string plasma_tech { get; set; }
        public string intergalactic_research_tech { get; set; }
        public string astrophysics_tech { get; set; }
        public string graviton_tech { get; set; }
        public string phalanx_planet_id { get; set; }
        public string off_geologist { get; set; }
        public string off_scientist { get; set; }
        public string off_engineer { get; set; }
        public string off_general { get; set; }
        public string off_admiral { get; set; }
        public string off_energy { get; set; }
        public string off_storer { get; set; }
        public string off_spy { get; set; }
        public string off_exspy { get; set; }
        public string starter { get; set; }
        public string ban_account { get; set; }
        public string ban_account_reason { get; set; }
        public string ban_forum { get; set; }
        public string ban_forum_reason { get; set; }
        public string ban_chat { get; set; }
        public string ban_chat_reason { get; set; }
        public string show_in_stat { get; set; }
        public string wheel_time { get; set; }
        public string email_registration { get; set; }
        public string email_current { get; set; }
        public string level { get; set; }
        public int MAX_BUILDING_QUEUE_SIZE { get; set; }
        public int FLEET_TO_DEBRIES { get; set; }
        public int resource_multiplier { get; set; }
        public int ENERGY_MULTIPLIER { get; set; }
        public int game_speed { get; set; }
        public int fleet_speed { get; set; }
        public int attack_enabled { get; set; }
    }
}

