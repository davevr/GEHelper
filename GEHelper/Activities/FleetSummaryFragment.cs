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

namespace GEHelper.Activities
{
    public class FleetSummaryFragment : Android.Support.V4.App.Fragment
    {
        public GEHelper.Activities.FleetFragment BaseView;
        private PullToRefresharp.Android.Widget.ListView fleetList;
        TextView summaryView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }




        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.FleetSummaryFragment, container, false);

            fleetList = view.FindViewById<PullToRefresharp.Android.Widget.ListView>(Resource.Id.FleetList);
            fleetList.Adapter = new FleetListAdapter(this.Activity, null);

            summaryView = view.FindViewById<TextView>(Resource.Id.summaryFooterText);

            fleetList.RefreshActivated += (o, e) =>
            {
                BaseView.SummaryPage.StartRefresh(() => { fleetList.OnRefreshCompleted(); });

            };

            Refresh();

            return view;
        }

        public void Refresh()
        {
            try
            {
                fleetList.InvalidateViews();
                fleetList.SmoothScrollToPosition(0);
                summaryView.Text = Core.GEServer.Instance.ServerState.FleetSummary;

            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Refresh failed:  " + exp.Message);
                // do nothing
            }
        }
    }
}