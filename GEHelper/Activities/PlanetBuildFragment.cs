using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using GEHelper.Core;


namespace GEHelper.Activities
{
    public class PlanetBuildFragment : Android.Support.V4.App.Fragment
    {
        public GEHelper.Activities.PlanetsFragment BaseView;
        private ListView BuildSpecListView;
        private Button NewBuildSpecBtn;
        private Button DeleteBuildSpecBtn;
        private Button BuildBtn;
        private int selectedIndex;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.PlanetBuildFragment, container, false);

            BuildSpecListView = view.FindViewById<ListView>(Resource.Id.buildSpecList);
            NewBuildSpecBtn = view.FindViewById<Button>(Resource.Id.newDefSpecBtn);
            DeleteBuildSpecBtn = view.FindViewById<Button>(Resource.Id.deleteDefSpecBtn);
            BuildBtn = view.FindViewById<Button>(Resource.Id.buildDefSpecBtn);

            NewBuildSpecBtn.Enabled = true;
            DeleteBuildSpecBtn.Enabled = false;
            BuildBtn.Enabled = false;

            BuildBtn.Click += BuildBtn_Click;
            BuildSpecListView.Adapter = new BuildSpecListAdapter(this.Activity, null);

            BuildSpecListView.ItemClick += BuildSpecListView_ItemClick;
            selectedIndex = -1;

            Refresh();


            return view;
        }

        void BuildSpecListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ((BuildSpecListAdapter)BuildSpecListView.Adapter).selectedIndex = e.Position;
            BuildSpecListView.SetSelection(selectedIndex);

            DeleteBuildSpecBtn.Enabled = true;
            BuildBtn.Enabled = true;
        }

        void BuildBtn_Click(object sender, EventArgs e)
        {
            if (selectedIndex >= 0)
            {
                BuildSpec curSpec = GEServer.Instance.BuildSpecs[selectedIndex];
                BuildBtn.Enabled = false;
                selectedIndex = -1;
                //BuildSpecListView.SetSelection(-1);

                GEServer.Instance.BuildSpecOnAllPlanets(curSpec, (theResult) =>
                {
                    this.Activity.RunOnUiThread(() =>
                        {
                            Toast.MakeText(this.Activity, "Build Queued OK", ToastLength.Long).Show();
                        });
                });
            }
        }

       

        public void Refresh()
        {
            try
            {
                BuildSpecListView.InvalidateViews();
                BuildSpecListView.SmoothScrollToPosition(0);
                
            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Refresh failed:  " + exp.Message);
                // do nothing
            }
        }
    }
}