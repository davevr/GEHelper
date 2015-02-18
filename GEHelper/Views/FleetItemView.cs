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
    class FleetItemView : RelativeLayout
    {
        private ImageView mImageView;
        private TextView mTitleTextView;
        private TextView mDescriptionTextView;

        public static FleetItemView inflate(ViewGroup parent)
        {
            FleetItemView itemView = (FleetItemView)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.FleetItemWrapper, parent, false);

            return itemView;
        }

        public FleetItemView(Context context, Android.Util.IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.FleetItem, this, true);
            setupChildren();
        }

        public FleetItemView(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.FleetItem, this, true);
            setupChildren();
        }

        private void setupChildren()
        {
            mImageView = FindViewById<ImageView>(Resource.Id.item_imageView);
            mTitleTextView = FindViewById<TextView>(Resource.Id.item_titleTextView);
            mDescriptionTextView = FindViewById<TextView>(Resource.Id.item_descriptionTextView);
        }


        public void SetItem(Core.GEFleet curFleet)
        {
            mTitleTextView.Text = curFleet.fleet_start_planet_name + " to " + curFleet.fleet_end_planet_name;
            mDescriptionTextView.Text = String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", curFleet.fleet_resource_metal, curFleet.fleet_resource_crystal, curFleet.fleet_resource_deuterium);
          
           
        }
    }
}