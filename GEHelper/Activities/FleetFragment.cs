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
   
	public class FleetFragment : Android.Support.V4.App.Fragment
    {
        public SummaryScreen SummaryPage { get; set; }

        public static FleetSummaryFragment SummaryView;
        public static FleetActionFragment ActionView;

        public class FleetPageAdapter : FragmentPagerAdapter
        {
            private string[] Titles = { "Summary", "Action" };


            public FleetPageAdapter(Android.Support.V4.App.FragmentManager fm)
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
                        theItem = FleetFragment.SummaryView;
                        break;

                    case 1:
                        theItem = FleetFragment.ActionView;
                        break;

                }
                return theItem;
            }
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.FleetFragment, container, false);
            var pager = view.FindViewById<ViewPager>(Resource.Id.fleet_pager);
            pager.Adapter = new FleetPageAdapter(this.FragmentManager);

            var tabs = view.FindViewById<PagerSlidingTabStrip>(Resource.Id.fleet_tabs);
            tabs.SetViewPager(pager);

            SummaryView = new FleetSummaryFragment();
            SummaryView.BaseView = this;

            ActionView = new FleetActionFragment();
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