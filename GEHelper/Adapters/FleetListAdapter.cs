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
    public class FleetListAdapter : BaseAdapter<Core.GEFleet>
    {
        private Activity context;

        public FleetListAdapter(Activity context, Core.GEFleet[] items)
            : base()
        {
            this.context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Core.GEFleet this[int position]
        {
            get { return Core.GEServer.Instance.ServerState.fleetList[position]; }
        }
        public override int Count
        {
            get { return Core.GEServer.Instance.ServerState.fleetList.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            FleetItemView view = (FleetItemView)convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = FleetItemView.inflate(parent);
            view.SetItem(this[position]);

            return view;
        }
    }

}