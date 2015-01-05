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
     public class PlanetListAdapter : BaseAdapter<Core.GEPlanet>
    {
        private Activity context;

        public PlanetListAdapter(Activity context, Core.GEPlanet[] items)
            : base()
        {
            this.context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Core.GEPlanet this[int position]
        {
            get { return Core.GEServer.Instance.ServerState.planetList[position]; }
        }
        public override int Count
        {
            get { return Core.GEServer.Instance.ServerState.planetList.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            Core.GEPlanet curPlanet = Core.GEServer.Instance.ServerState.planetList[position];

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = curPlanet.name;
            string descString = String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", curPlanet.metal, curPlanet.crystal, curPlanet.deuterium);
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = descString;
            return view;
        }
    }

    public class PlanetsFragment : Fragment
    {
        private PullToRefresharp.Android.Widget.ListView planetList;
        TextView summaryView;
        public SummaryScreen SummaryPage { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PlanetsFragment, container, false);

            planetList = view.FindViewById<PullToRefresharp.Android.Widget.ListView>(Resource.Id.PlanetList);
            planetList.Adapter = new PlanetListAdapter(this.Activity, null);

            summaryView = view.FindViewById<TextView>(Resource.Id.summaryFooterText);

            planetList.RefreshActivated += (o, e) =>
                {
                    SummaryPage.StartRefresh(() => { planetList.OnRefreshCompleted(); });

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