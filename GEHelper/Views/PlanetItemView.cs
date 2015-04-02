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
    class PlanetItemView : RelativeLayout
    {
        private ImageView mImageView;
        private TextView mTitleTextView;
        private TextView mDescriptionTextView;
        private ImageView mMoonImageView;
        private TextView mMoonTitleTextView;
        private TextView mMoonDescriptionTextView;
        private LinearLayout mMoonView;
		private TextView mPlanetLocView;
		private TextView mMoonSizeView;

        public static PlanetItemView inflate(ViewGroup parent)
        {
            PlanetItemView itemView = (PlanetItemView)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PlanetItemWrapper, parent, false);

            return itemView;
        }

        public PlanetItemView(Context context, Android.Util.IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.PlanetItem, this, true);
            setupChildren();
        }

        public PlanetItemView(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.PlanetItem, this, true);
            setupChildren();
        }

        private void setupChildren()
        {
            mImageView = FindViewById<ImageView>(Resource.Id.item_imageView);
            mTitleTextView = FindViewById<TextView>(Resource.Id.item_titleTextView);
            mDescriptionTextView = FindViewById<TextView>(Resource.Id.item_descriptionTextView);
            mMoonImageView = FindViewById<ImageView>(Resource.Id.item_moonimageView);
            mMoonTitleTextView = FindViewById<TextView>(Resource.Id.item_moontitleTextView);
            mMoonDescriptionTextView = FindViewById<TextView>(Resource.Id.item_moondescriptionTextView);
            mMoonView = FindViewById<LinearLayout>(Resource.Id.item_moonView);
			mPlanetLocView = FindViewById<TextView> (Resource.Id.item_titleLocView);
			mMoonSizeView = FindViewById<TextView> (Resource.Id.item_moonSizeView);
        }


        public void SetItem(Core.GEPlanet curPlanet)
        {
            mTitleTextView.Text = curPlanet.name;
			mPlanetLocView.Text = String.Format ("{0}:{1}:{2}", curPlanet.g, curPlanet.s, curPlanet.p);
            mDescriptionTextView.Text =  String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", curPlanet.metal, curPlanet.crystal, curPlanet.deuterium);
			string planetStr = "planets_" + curPlanet.image;
			int resourceId = Resources.GetIdentifier(planetStr, "drawable", this.Context.PackageName);

			mImageView.SetImageResource (resourceId);
            if (curPlanet.moon != null)
            {
				mMoonImageView.SetImageResource (Resource.Drawable.planets_moon);
                mMoonView.Visibility = ViewStates.Visible;
                mMoonTitleTextView.Text = curPlanet.moon.name;
                mMoonDescriptionTextView.Text = String.Format("M:{0:0,0}  C:{1:0,0}  D:{2:0,0}", curPlanet.moon.metal, curPlanet.moon.crystal, curPlanet.moon.deuterium);
				mMoonSizeView.Text = String.Format ("{0}m", curPlanet.moon.diameter);
            }
            else
            {
                mMoonView.Visibility = ViewStates.Gone;
            }
           
        }

    }
}