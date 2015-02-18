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

namespace GEHelper.Activities
{
   
    public class FleetFragment : Fragment
    {
        public SummaryScreen SummaryPage { get; set; }
        private PullToRefresharp.Android.Widget.ListView fleetList;
        TextView summaryView;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.FleetFragment, container, false);

            fleetList = view.FindViewById<PullToRefresharp.Android.Widget.ListView>(Resource.Id.FleetList);
            fleetList.Adapter = new FleetListAdapter(this.Activity, null);

            summaryView = view.FindViewById<TextView>(Resource.Id.summaryFooterText);

            fleetList.RefreshActivated += (o, e) =>
            {
                SummaryPage.StartRefresh(() => { fleetList.OnRefreshCompleted(); });

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