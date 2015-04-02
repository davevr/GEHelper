using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.IO.IsolatedStorage;

namespace GEHelper.Core
{
    class AppSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Universe { get; set; }

        private static AppSettings _instance = null;



        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }


        public void LoadSettings()
        {
            System.Console.WriteLine("Loading Settings");
            string settingsString = SafeLoadSetting("Settings", "");
            if (!String.IsNullOrEmpty(settingsString))
                _instance = settingsString.FromJson<AppSettings>();
            else
            {
                _instance = new AppSettings();
                _instance.Username = "davevr@gmail.com";
                _instance.Password = "Love4Runess";
                _instance.Universe = "srv3";
            }

        }

        public void SaveSettings()
        {
            System.Console.WriteLine("Saving Settings");
            SafeSaveSetting("Settings", _instance.ToJson());
        }

        public string SafeLoadSetting(string setting, string defVal)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(setting))
                return (string)settings[setting];
            else
            {
                settings.Add(setting, defVal);
                return defVal;
            }
        }


        public void SafeSaveSetting(string setting, string val)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(setting))
                settings[setting] = val;
            else
            {
                settings.Add(setting, val);

            }
            settings.Save();
        }

        public void SafeClearSetting(string setting)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings settings = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(setting))
            {
                settings.Remove(setting);
                settings.Save();
            }
        }

        public void Clear()
        {
            Username = "";
            Password = "";
            Universe = "";
        }
    }


}
