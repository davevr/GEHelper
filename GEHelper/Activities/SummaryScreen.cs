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


namespace GEHelper.Activities
{
    [Activity(Label = "Galactic Empires Helper") ]
    public class SummaryScreen : Activity
    {
        private bool refreshInProgress = false;
        private ActionBar.Tab currentTab = null;
        private ActionBar.Tab planetTab;
        private ActionBar.Tab searchTab;
        private ActionBar.Tab fleetTab;
        private PlanetsFragment planetFragment = null;
        private SearchAndScanActivity searchFragment = null;
        FleetFragment fleetFragment = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SummaryScreen);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            planetTab = this.ActionBar.NewTab();

            planetTab.SetText("Planets");
            planetTab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                currentTab = planetTab;
                if (planetFragment == null)
                {
                    planetFragment = new Activities.PlanetsFragment();
                    e.FragmentTransaction.Add(Resource.Id.fragmentContainer, planetFragment);
                }
                else
                {
                    e.FragmentTransaction.Show(planetFragment);
                    StartRefresh();
                }
                     
            };

            planetTab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Hide(planetFragment);
            };

            searchTab = this.ActionBar.NewTab();
            searchTab.SetText("Scan");
            searchTab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                currentTab = searchTab;
                if (searchFragment == null)
                {
                    searchFragment = new Activities.SearchAndScanActivity();
                    e.FragmentTransaction.Add(Resource.Id.fragmentContainer, searchFragment);
                }
                else
                {
                    e.FragmentTransaction.Show(searchFragment);
                    StartRefresh();
                }

            };

            searchTab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Hide(searchFragment);
            };

            fleetTab = this.ActionBar.NewTab();
            fleetTab.SetText("Fleet");
            fleetTab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                currentTab = fleetTab;
                if (fleetFragment == null)
                {
                    fleetFragment = new Activities.FleetFragment();
                    e.FragmentTransaction.Add(Resource.Id.fragmentContainer, fleetFragment);
                    
                }
                else
                {
                    e.FragmentTransaction.Show(fleetFragment);
                    StartRefresh();
                }

            };

            fleetTab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Hide(fleetFragment);
            };

            this.ActionBar.AddTab(planetTab);
            this.ActionBar.AddTab(searchTab);
            this.ActionBar.AddTab(fleetTab);
        }

        private void StartRefresh()
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
                            });

                            
                        }
                        refreshInProgress = false;
                       
                    });
            }
        }
    }
}