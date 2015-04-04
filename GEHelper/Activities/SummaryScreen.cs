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
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Animation;
using Android.Views.Animations;
using Android.Util;
using Android.Support.V7.Widget;
using Android.Support.V7.View;
using Android.Support.V7.AppCompat;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using GEHelper.Core;

namespace GEHelper.Activities
{
	[Activity(Label = "Galactic Empires Helper", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light")]
    public class SummaryScreen : Android.Support.V7.App.ActionBarActivity
    {
        private bool refreshInProgress = false;

        private PlanetsFragment planetFragment = null;
        private SearchAndScanActivity searchFragment = null;
        private FleetFragment fleetFragment = null;

        public event Action PulledToRefresh;

        private String[] mPlanetTitles = new string[] { "Planets", "Universe", "Fleet", "Strategy" };
        private DrawerLayout mDrawerLayout;
        private ListView mDrawerList;
        private string mDrawerTitle;
        private MyDrawerToggle mDrawerToggle;

        class MyDrawerToggle : Android.Support.V7.App.ActionBarDrawerToggle
        {
            private  string openString, closeString;
            private SummaryScreen baseActivity;

            public MyDrawerToggle(Activity activity, DrawerLayout drawerLayout, int openDrawerContentDescRes, int closeDrawerContentDescRes) :
                base ( activity,  drawerLayout,  openDrawerContentDescRes,  closeDrawerContentDescRes)
            {
                baseActivity = (SummaryScreen)activity;
                openString = baseActivity.Resources.GetString(openDrawerContentDescRes);
                closeString = baseActivity.Resources.GetString(closeDrawerContentDescRes);
            }
            public override void OnDrawerOpened(View drawerView)
            {
 	             base.OnDrawerOpened(drawerView);
                //baseActivity.Title = openString;

                
            }

            public override void OnDrawerClosed(View drawerView)
            {
 	             base.OnDrawerClosed(drawerView);
                //baseActivity.Title = closeString;
            }
        }
     
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SummaryScreen);

            
			InitGame ();

         


        }

		protected void InitDrawers()
		{
			// set up drawer
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			mDrawerList = FindViewById<ListView>(Resource.Id.left_drawer);
			// Set the adapter for the list view
			mDrawerList.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, mPlanetTitles);
			// Set the list's click listener
			mDrawerList.ItemClick += mDrawerList_ItemClick;

			mDrawerToggle = new MyDrawerToggle(this, mDrawerLayout, Resource.String.drawer_open, Resource.String.drawer_close);


			mDrawerLayout.SetDrawerListener(mDrawerToggle);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetHomeButtonEnabled(true);
			selectItem(0);
			mDrawerToggle.SyncState();
		}

		protected void InitGame() 
		{
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
											RunOnUiThread(() => 
												{
													InitDrawers();
												});
											
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

		protected override void OnStop ()
		{
			base.OnStop ();
			GEServer.Instance.SaveState ();
		}

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
 	         base.OnPostCreate(savedInstanceState);
            
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
 	         base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
 	         if (mDrawerToggle.OnOptionsItemSelected(item)) 
             {
                  return true;
             }

            // Handle your other action bar items...

            return base.OnOptionsItemSelected(item);
        }

        void mDrawerList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            selectItem(e.Position);
        }

		private Android.Support.V4.App.Fragment oldPage = null;

        private void selectItem(int position)
        {
			Android.Support.V4.App.Fragment newPage = null;
			var fragmentManager = this.SupportFragmentManager;
            var ft = fragmentManager.BeginTransaction();
            bool firstTime = false;

            switch (position)
            {
                case 0:
                    if (planetFragment == null)
                    {
                        planetFragment = new PlanetsFragment();
                        planetFragment.SummaryPage = this;
                        firstTime = true;
                    }
                    newPage = planetFragment;
                    break;
                case 1:
                    if (searchFragment == null)
                    {
                        searchFragment = new SearchAndScanActivity();
                        searchFragment.SummaryPage = this;
                        firstTime = true;
                    }
                    newPage = searchFragment;
                    break;
                case 2:
                    if (fleetFragment == null)
                    {
                        fleetFragment = new FleetFragment();
                        fleetFragment.SummaryPage = this;
                        firstTime = true;
                    }
                    newPage = fleetFragment;
                    break;
                case 3:
                    break;
            }

            if (oldPage != newPage)
            {
                if (oldPage != null)
                {
                    // to do - deactivate it
                    ft.Hide(oldPage);

                }

                oldPage = newPage;

                if (newPage != null)
                {
                    if (firstTime)
                        ft.Add(Resource.Id.fragmentContainer, newPage);
                    else
                        ft.Show(newPage);
                }
                
                ft.Commit();

                // update selected item title, then close the drawer
                Title = mPlanetTitles[position];
                mDrawerList.SetItemChecked(position, true);
                mDrawerLayout.CloseDrawer(mDrawerList);
            }
        }

        protected override void OnTitleChanged(Java.Lang.ICharSequence title, Android.Graphics.Color color)
        {
            //base.OnTitleChanged (title, color);
            this.SupportActionBar.Title = title.ToString();
        }


        public void StartRefresh(Action callback = null)
        {
            if (!refreshInProgress)
            {
                refreshInProgress = true;

                Core.GEServer.Instance.Refresh((theResult) =>
                    {
                        if (theResult == "ok")
                        {
                            RunOnUiThread(() =>
                            {
                                if (planetFragment != null)
                                    planetFragment.Refresh();
                                if (searchFragment != null)
                                    searchFragment.Refresh();
                                if (fleetFragment != null)
                                    fleetFragment.Refresh();

                                if (callback != null)
                                    callback();
                            });    
                        }
                        else
                        {

                        }
                        refreshInProgress = false;
                       
                    });
            }
        }


    }
}