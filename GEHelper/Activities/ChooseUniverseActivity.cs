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
using Android.Graphics.Drawables;
using Android.Graphics;


namespace GEHelper.Activities
{

    public class ChooseUniverseEventArgs : EventArgs
    {
        public string Universe { get; set; }
    }

    public delegate void UniverseDialogEventHandler(object sender, ChooseUniverseEventArgs args);

    public class UniverseListAdapter : BaseAdapter<Core.ServerInfo> 
    {
        private Core.ServerInfo[] items;
        private Activity context;

        public UniverseListAdapter(Activity context, Core.ServerInfo[] items)
            : base() 
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Core.ServerInfo this[int position] {  
            get { return items[position]; }
        }
        public override int Count {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = items[position].description;
            return view;
        }
    }


    [Activity(Label = "Choose Universe Activity")]
    public class ChooseUniverseActivity : Android.App.DialogFragment
    {
        private ListView universeList;


        public event UniverseDialogEventHandler Dismissed;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Android 3.x+ still wants to show title: disable
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            // Create our view
            var view = inflater.Inflate(Resource.Layout.ChooseUniverse, container, true);

            // Handle dismiss button click
            universeList = view.FindViewById<ListView>(Resource.Id.UniverseList);


            universeList.ItemSelected += universeList_ItemSelected;
            universeList.ItemClick += universeList_ItemClick;

            universeList.Adapter = new UniverseListAdapter(this.Activity, ((MainActivity)Activity).UniverseList);
            

            return view;
        }

        void universeList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            string uni = "srv3";

            switch (e.Position)
            {
                case 0:
                    uni = "srv1";
                    break;
                case 1:
                    uni = "srv2";
                    break;
                case 2:
                    uni = "srv3";
                    break;
            }

            Core.AppSettings.Instance.Universe = uni;

            Dismiss();
            if (Dismissed != null)
                Dismissed(this, new ChooseUniverseEventArgs { Universe = uni });
        }

        void universeList_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            string uni = "srv3";

            switch (e.Position)
            {
                case 0:
                    uni = "srv1";
                    break;
                case 1:
                    uni = "srv2";
                    break;
                case 2:
                    uni = "srv3";
                    break;
            }

            Core.AppSettings.Instance.Universe = uni;

            Dismiss();
            if (Dismissed != null)
                Dismissed(this, new ChooseUniverseEventArgs {Universe = uni} );
        }


        public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Make sure there is no background behind our view
           // Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            SetStyle(DialogFragmentStyle.NoFrame, Android.Resource.Style.Theme);

            base.OnResume();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unwire event
            if (disposing)
                universeList.ItemSelected -= universeList_ItemSelected;
        }


    }
}