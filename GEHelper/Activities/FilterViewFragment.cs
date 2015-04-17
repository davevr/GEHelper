
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;


namespace GEHelper
{
	public class FilterViewFragment : Android.Support.V4.App.Fragment
	{
		public GEHelper.Activities.SearchAndScanActivity BaseView;

		private CheckBox usernameCheckbox;
		private EditText userNameField;
		private CheckBox nearRankCheckbox;
		private CheckBox nearPlanetCheckbox;
		private CheckBox lanxableCheckbox;
		private CheckBox ownPlanetsCheckbox;
		private EditText rankRangeField;
		private EditText planetRangeField;
		private CheckBox allianceCheckbox;
		private EditText allianceField;
		private CheckBox debrisCheckbox;
		private EditText debrisField;
        private CheckBox inactiveCheckbox;
        private CheckBox limitScanRangeCheckbox;
        private EditText startGalaxyField;
        private EditText endGalaxyField;
        private EditText startSystemField;
        private EditText endSystemField;
        private LinearLayout ScanLimitArea;

		private Button FilterNowBtn;
		private Button ClearFilterBtn;


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

		 	base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.FilterViewFragment, container, false);

			usernameCheckbox = view.FindViewById<CheckBox>(Resource.Id.usernameCheckbox);
			userNameField = view.FindViewById<EditText>(Resource.Id.usernameField);

			nearRankCheckbox = view.FindViewById<CheckBox>(Resource.Id.nearRankCheckbox);
			rankRangeField = view.FindViewById<EditText>(Resource.Id.rankRangeField);

			nearPlanetCheckbox = view.FindViewById<CheckBox>(Resource.Id.nearPlanetCheckbox);
			planetRangeField = view.FindViewById<EditText>(Resource.Id.nearPlanetField);

			allianceCheckbox = view.FindViewById<CheckBox>(Resource.Id.allianceCheckbox);
			allianceField = view.FindViewById<EditText>(Resource.Id.allianceField);

			debrisCheckbox = view.FindViewById<CheckBox>(Resource.Id.debrisCheckbox);
			debrisField = view.FindViewById<EditText>(Resource.Id.debrisField);

			lanxableCheckbox = view.FindViewById<CheckBox>(Resource.Id.lanxCheckbox);

			ownPlanetsCheckbox = view.FindViewById<CheckBox>(Resource.Id.ownPlanetsCheckbox);

            inactiveCheckbox = view.FindViewById<CheckBox>(Resource.Id.inactiveCheckbox);

            limitScanRangeCheckbox = view.FindViewById<CheckBox>(Resource.Id.limitRangeCheckbox);
            ScanLimitArea = view.FindViewById<LinearLayout>(Resource.Id.scanLimitArea);
            startGalaxyField = view.FindViewById<EditText>(Resource.Id.startGalaxyField);
            endGalaxyField = view.FindViewById<EditText>(Resource.Id.endGalaxyField);
            startSystemField = view.FindViewById<EditText>(Resource.Id.startSystemField);
            endSystemField = view.FindViewById<EditText>(Resource.Id.endSystemField);

			FilterNowBtn = view.FindViewById<Button>(Resource.Id.filterNowBtn);
			ClearFilterBtn = view.FindViewById<Button>(Resource.Id.clearFilterBtn);


            ScanLimitArea.Visibility = ViewStates.Gone;

			FilterNowBtn.Click += FilterNowBtn_Click;
			ClearFilterBtn.Click += ClearFilterBtn_Click;
            limitScanRangeCheckbox.CheckedChange += limitScanRangeCheckbox_CheckedChange;

			return view;
		}

        void limitScanRangeCheckbox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
                ScanLimitArea.Visibility = ViewStates.Visible;
            else
                ScanLimitArea.Visibility = ViewStates.Gone;
        }



		void ClearFilterBtn_Click(object sender, EventArgs e)
		{
			BaseView.UserClearFilters ();
		}

		public void FilterNowBtn_Click(object sender, EventArgs e)
		{
			BaseView.useUsername = usernameCheckbox.Checked;
			if (BaseView.useUsername)
				BaseView.scanName = userNameField.Text;

			BaseView.useAlliance = allianceCheckbox.Checked;
			if (BaseView.useAlliance)
				BaseView.allianceName = allianceField.Text;

			BaseView.useRank = nearRankCheckbox.Checked;
			if (BaseView.useRank)
				int.TryParse(rankRangeField.Text, out BaseView.rankRange);
			
			BaseView.usePlanets = nearPlanetCheckbox.Checked;
			if (BaseView.usePlanets)
				int.TryParse(planetRangeField.Text, out BaseView.planetRange);

			BaseView.useDebris = debrisCheckbox.Checked;
			if (BaseView.useDebris)
				long.TryParse(debrisField.Text, out BaseView.debrisSize);
			
			BaseView.useLanx = lanxableCheckbox.Checked;

			BaseView.includeOwnPlanets = ownPlanetsCheckbox.Checked;

            BaseView.inactiveOnly = inactiveCheckbox.Checked;

            BaseView.useLimits = limitScanRangeCheckbox.Checked;
            if (BaseView.useLimits)
            {
                int.TryParse(startGalaxyField.Text, out BaseView.startGalaxy);
                int.TryParse(endGalaxyField.Text, out BaseView.endGalaxy);
                int.TryParse(startSystemField.Text, out BaseView.startSystem);
                int.TryParse(endSystemField.Text, out BaseView.endSystem);
            }


			BaseView.UserApplyFilters ();
		}
	}
}

