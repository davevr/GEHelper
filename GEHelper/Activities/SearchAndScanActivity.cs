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
    [Activity(Label = "SearchAndScanActivity")]
    public class SearchAndScanActivity : Fragment
    {
        public SummaryScreen SummaryPage { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SearchAndScan, container, false);

            // TO DO - init the view with the fleet.
            return view;
        }

        public void Refresh()
        {
            
        }
    }
}