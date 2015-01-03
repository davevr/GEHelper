using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace GEHelper.Activities
{
        public class SignInEventArgs : EventArgs
        {

        }

        public delegate void DialogEventHandler(object sender, SignInEventArgs args);

       

    [Activity(Label = "SignInActivity")]
    public class SignInActivity : Android.App.DialogFragment
    {
        private Button signInButton;
        private EditText userNameField;
        private EditText passwordField;
        private ProgressDialog signinProgress;


        public event DialogEventHandler Dismissed;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Android 3.x+ still wants to show title: disable
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Create our view
            var view = inflater.Inflate(Resource.Layout.SignIn, container, true);

            // Handle dismiss button click
            signInButton = view.FindViewById<Button>(Resource.Id.signInButton);
            userNameField = view.FindViewById<EditText>(Resource.Id.signInEmailAddress);
            passwordField = view.FindViewById<EditText>(Resource.Id.signInPassword);

            if (!String.IsNullOrEmpty(Core.AppSettings.Instance.Username))
                userNameField.Text = Core.AppSettings.Instance.Username;

            if (!String.IsNullOrEmpty(Core.AppSettings.Instance.Password))
                passwordField.Text = Core.AppSettings.Instance.Password;


            signInButton.Click += signInButton_Click;

            return view;
        }

        void signInButton_Click(object sender, EventArgs e)
        {
            signInButton.Enabled = false;
            string userName = userNameField.Text;
            string password = passwordField.Text;
            AttemptSignIn(userName, password);
        }

        protected void AttemptSignIn(string username, string password)
        {
            signinProgress = new ProgressDialog(this.Activity);
            signinProgress.SetProgressStyle(ProgressDialogStyle.Spinner);
            signinProgress.Show();
            GEHelper.Core.GEServer.Instance.Login(username, password, (theResult) =>
                {
                    if (theResult == "")
                    {
                        System.Console.WriteLine("Logged in!");
                        Core.AppSettings.Instance.Username = username;
                        Core.AppSettings.Instance.Password = password;

                        // now chose a universe...
                        Core.GEServer.Instance.GetServerList((theSet) =>
                        {
                            System.Console.WriteLine("Got Server List!");
                            ((MainActivity)Activity).UniverseList = new Core.ServerInfo[3];
                            ((MainActivity)Activity).UniverseList[0] = theSet.srv1;
                            ((MainActivity)Activity).UniverseList[1] = theSet.srv2;
                            ((MainActivity)Activity).UniverseList[2] = theSet.srv3;

                            this.Activity.RunOnUiThread(() =>
                            {
                                signinProgress.Hide();
                                signInButton.Enabled = true;
                                var dialog = new Activities.ChooseUniverseActivity();
                                FragmentTransaction showUniverseTrans = FragmentManager.BeginTransaction();
                                dialog.Dismissed += UniverseDialog_Dismissed;
                                dialog.Show(showUniverseTrans, "choose universe");
                            });
                            
                   
                        });
                       
                    }
                    else
                    {
                        this.Activity.RunOnUiThread(() =>
                        {
                            signinProgress.Hide();
                            signInButton.Enabled = true;
                            Toast.MakeText(this.Activity, Resource.String.err_login, ToastLength.Short).Show();
                        });
                    }
                });

        }



        void UniverseDialog_Dismissed(object sender, Activities.ChooseUniverseEventArgs args)
        {
            
            Core.AppSettings.Instance.Universe = args.Universe;
            Dismiss();
            if (Dismissed != null)
                Dismissed(this, new SignInEventArgs());
        }

        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Make sure there is no background behind our view
           // Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(DialogFragmentStyle.NoFrame, Android.Resource.Style.Theme);

            base.OnResume();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unwire event
            if (disposing)
                signInButton.Click -= signInButton_Click;
        }


    }
}