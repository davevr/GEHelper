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
using Android.Support.V4.View;
using com.refractored;

namespace GEHelper.Activities
{
     
	public class PlanetsFragment : Android.Support.V4.App.Fragment
    {
        public static PlanetOverviewFragment SummaryView;
        public static PlanetBuildFragment BuildView;
        public static PlanetActionFragment ActionView;

        public SummaryScreen SummaryPage { get; set; }


        public class PlanetPageAdapter : FragmentPagerAdapter
        {
            private string[] Titles = { "Summary", "Build", "Action" };


            public PlanetPageAdapter(Android.Support.V4.App.FragmentManager fm)
                : base(fm)
            {
            }

            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(Titles[position]);
            }

            public override int Count
            {
                get
                {
                    return Titles.Length;
                }
            }

            public override Android.Support.V4.App.Fragment GetItem(int position)
            {
                Android.Support.V4.App.Fragment theItem = null;
                switch (position)
                {
                    case 0:
                        theItem = PlanetsFragment.SummaryView;
                        break;

                    case 1:
                        theItem = PlanetsFragment.BuildView;
                        break;

                    case 2:
                        theItem = PlanetsFragment.ActionView;
                        break;

                }
                return theItem;
            }
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PlanetsFragment, container, false);

            var pager = view.FindViewById<ViewPager>(Resource.Id.planet_pager);
            pager.Adapter = new PlanetPageAdapter(this.FragmentManager);

            var tabs = view.FindViewById<PagerSlidingTabStrip>(Resource.Id.planet_tabs);
            tabs.SetViewPager(pager);

            SummaryView = new PlanetOverviewFragment();
            SummaryView.BaseView = this;

            BuildView = new PlanetBuildFragment();
            BuildView.BaseView = this;

            ActionView = new PlanetActionFragment();
            ActionView.BaseView = this;

            Refresh();
            return view;
        }

        public void Refresh()
        {
            try
            {
                SummaryView.Refresh();

            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Refresh failed:  " + exp.Message);
                // do nothing
            }
        }

       


    }
}