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

namespace GEHelper
{
    public class TargetListAdapter : BaseAdapter<Core.GEGalaxyPlanet>
    {
        private Activity context;

        public TargetListAdapter(Activity context, Core.GEGalaxyPlanet[] items)
            : base()
        {
            this.context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Core.GEGalaxyPlanet this[int position]
        {
            get { return Core.GEServer.Instance.FilteredScanResults[position]; }
        }
        public override int Count
        {
            get { return Core.GEServer.Instance.FilteredScanResults.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TargetItemView view = (TargetItemView)convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = TargetItemView.inflate(parent);
            view.SetItem(this[position]);

            return view;
        }
    }
}