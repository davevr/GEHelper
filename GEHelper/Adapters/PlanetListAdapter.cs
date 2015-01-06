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
            PlanetItemView view = (PlanetItemView)convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = PlanetItemView.inflate(parent);
            view.SetItem(this[position]);
           
            return view;
        }
    }

}