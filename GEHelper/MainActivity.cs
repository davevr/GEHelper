using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace GEHelper
{
    [Activity(Label = "Galactic Empire Helper", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private Bundle gameState;

        public Core.ServerInfo[] UniverseList { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            gameState = new Bundle();

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate 
            {
                var dialog = new Activities.SignInActivity();
                FragmentTransaction loginTrans = FragmentManager.BeginTransaction();
                dialog.Dismissed += LoginDialog_Dismissed;
                dialog.Show(loginTrans, "sign in");
            };

            Core.AppSettings.Instance.LoadSettings();

            if (!String.IsNullOrEmpty(Core.AppSettings.Instance.Username))
            {
                string username = Core.AppSettings.Instance.Username;
                if (!String.IsNullOrEmpty(Core.AppSettings.Instance.Password))
                {
                    string password = Core.AppSettings.Instance.Password;
                    if (!String.IsNullOrEmpty(Core.AppSettings.Instance.Universe))
                    {
                        string universe = Core.AppSettings.Instance.Universe;

                        GEHelper.Core.GEServer.Instance.Login(username, password, (result) =>
                            {
                                if (result == "")
                                {
                                    Core.GEServer.Instance.SetServer(universe, (theResult) =>
                                        {
                                            StartActivity(typeof(Activities.SummaryScreen));
                                        });
                                }
                                else
                                {
                                    // auto password is wrong

                                }
                            });
                    }
                }
            }
        }

        void LoginDialog_Dismissed(object sender, Activities.SignInEventArgs args)
        {
            Core.AppSettings.Instance.SaveSettings();
            string universe = Core.AppSettings.Instance.Universe;

            Core.GEServer.Instance.SetServer(universe, (theResult) =>
                {
                    StartActivity(typeof(Activities.SummaryScreen));
                });
        }

       

        public void DoSignIn(string username, string password)
        {

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void FinishActivityFromChild(Activity child, int requestCode)
        {
            base.FinishActivityFromChild(child, requestCode);
        }

    }
}

