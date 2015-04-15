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
    class BuildSpecListAdapter : BaseAdapter<Core.BuildSpec>
    {
        private Activity context;
        public int selectedIndex = -1;

        public BuildSpecListAdapter(Activity context, Core.BuildSpec[] items)
            : base()
        {
            this.context = context;
           
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Core.BuildSpec this[int position]
        {
            get
            {
                return Core.GEServer.Instance.BuildSpecs[position];
            }
        }
        public override int Count
        {
            get
            {
                try
                {
                    return Core.GEServer.Instance.BuildSpecs.Count;
                }
                catch (Exception exp)
                {
                    return 0;
                }

            }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            BuildSpecItemView view = (BuildSpecItemView)convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = BuildSpecItemView.inflate(parent);
            view.SetItem(this[position], selectedIndex == position);
       
            return view;
        }
    }
}