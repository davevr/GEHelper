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
    public class PlanetOverviewFragment : Android.Support.V4.App.Fragment
    {
        public GEHelper.Activities.PlanetsFragment BaseView;
        private PullToRefresharp.Android.Widget.ListView planetList;
        TextView summaryView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PlanetOverviewFragment, container, false);

            planetList = view.FindViewById<PullToRefresharp.Android.Widget.ListView>(Resource.Id.PlanetList);
            planetList.Adapter = new PlanetListAdapter(this.Activity, null);

            summaryView = view.FindViewById<TextView>(Resource.Id.summaryFooterText);

            planetList.RefreshActivated += (o, e) =>
            {
                BaseView.SummaryPage.StartRefresh(() => { planetList.OnRefreshCompleted(); });

            };

            Refresh();

            return view;
        }

        public void Refresh()
        {
            try
            {
                planetList.InvalidateViews();
                planetList.SmoothScrollToPosition(0);
                summaryView.Text = Core.GEServer.Instance.ServerState.SummaryText;

            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Refresh failed:  " + exp.Message);
                // do nothing
            }
        }
    }
}