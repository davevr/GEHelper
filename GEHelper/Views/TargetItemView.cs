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
    class TargetItemView : RelativeLayout
    {
        private ImageView mImageView;
        private TextView mTitleTextView;
        private TextView mDescriptionTextView;
        private ImageView mMoonImageView;
        private TextView mMoonTitleTextView;
        private LinearLayout mMoonView;
		private TextView mPlanetLocView;
		private TextView mMoonSizeView;

        public static TargetItemView inflate(ViewGroup parent)
        {
            TargetItemView itemView = (TargetItemView)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TargetItemWrapper, parent, false);

            return itemView;
        }

        public TargetItemView(Context context, Android.Util.IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.TargetItem, this, true);
            setupChildren();
        }

        public TargetItemView(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.TargetItem, this, true);
            setupChildren();
        }

        private void setupChildren()
        {
            mImageView = FindViewById<ImageView>(Resource.Id.item_imageView);
            mTitleTextView = FindViewById<TextView>(Resource.Id.item_titleTextView);
            mDescriptionTextView = FindViewById<TextView>(Resource.Id.item_descriptionTextView);
            mMoonImageView = FindViewById<ImageView>(Resource.Id.item_moonimageView);
            mMoonTitleTextView = FindViewById<TextView>(Resource.Id.item_moontitleTextView);
            mMoonView = FindViewById<LinearLayout>(Resource.Id.item_moonView);
			mPlanetLocView = FindViewById<TextView> (Resource.Id.item_titleLocView);
			mMoonSizeView = FindViewById<TextView> (Resource.Id.item_moonSizeView);
        }


        public void SetItem(Core.GEGalaxyPlanet curPlanet)
        {
			mTitleTextView.Text = curPlanet.name;
			mPlanetLocView.Text = String.Format ("{0}:{1}:{2}", curPlanet.g, curPlanet.s, curPlanet.p);
			string planetStr = "planets_" + curPlanet.image;
			int resourceId = Resources.GetIdentifier(planetStr, "drawable", this.Context.PackageName);

			mImageView.SetImageResource (resourceId);
			mDescriptionTextView.Text = String.Format(String.IsNullOrEmpty(curPlanet.ally_name) ?  "{0} - #{2}" : "{0} ({1}) - #{2}", curPlanet.username, curPlanet.ally_name, curPlanet.rank);
            if (curPlanet.moon != null)
            {
				mMoonImageView.SetImageResource (Resource.Drawable.planets_moon);
				mMoonView.Visibility = ViewStates.Visible;
				mMoonTitleTextView.Text = curPlanet.moon.name;
				mMoonSizeView.Text = String.Format ("{0}m", curPlanet.moon.diameter);
            }
            else
            {
                mMoonView.Visibility = ViewStates.Gone;
            }

        }

    }
}