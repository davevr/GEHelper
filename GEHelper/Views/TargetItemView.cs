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
        private TextView mMoonDescriptionTextView;
        private LinearLayout mMoonView;

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
            mMoonDescriptionTextView = FindViewById<TextView>(Resource.Id.item_moondescriptionTextView);
            mMoonView = FindViewById<LinearLayout>(Resource.Id.item_moonView);
        }


        public void SetItem(Core.GEGalaxyPlanet curPlanet)
        {
            mTitleTextView.Text = curPlanet.name + String.Format(" - {0:0}:{1:0}:{2:0}", curPlanet.g, curPlanet.s, curPlanet.p);
            mDescriptionTextView.Text = String.Format("{0} ({1})", curPlanet.username, curPlanet.ally_name);
            if (curPlanet.moon != null)
            {
                mMoonView.Visibility = ViewStates.Visible;
                mMoonTitleTextView.Text = curPlanet.moon.name;
                mMoonDescriptionTextView.Text = String.Format("size: {0}", curPlanet.moon.diameter);
            }
            else
            {
                mMoonView.Visibility = ViewStates.Gone;
            }

        }

    }
}