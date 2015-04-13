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
    class BuildSpecItemView : RelativeLayout
    {
        private TextView mTitleTextView;
        private TextView mDescriptionTextView;

        public static BuildSpecItemView inflate(ViewGroup parent)
        {
            BuildSpecItemView itemView = (BuildSpecItemView)LayoutInflater.From(parent.Context).Inflate(Resource.Layout.BuildSpecItemWrapper, parent, false);

            return itemView;
        }

        public BuildSpecItemView(Context context, Android.Util.IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.BuildSpecItem, this, true);
            setupChildren();
        }

        public BuildSpecItemView(Context context, Android.Util.IAttributeSet attrs)
            : base(context, attrs)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.BuildSpecItem, this, true);
            setupChildren();
        }

        private void setupChildren()
        {
            mTitleTextView = FindViewById<TextView>(Resource.Id.item_titleTextView);
            mDescriptionTextView = FindViewById<TextView>(Resource.Id.item_descriptionTextView);

        }


        public void SetItem(Core.BuildSpec curSpec)
        {

            mTitleTextView.Text = curSpec.name;
            mDescriptionTextView.Text = curSpec.SummaryString();
        }
			
    }
}