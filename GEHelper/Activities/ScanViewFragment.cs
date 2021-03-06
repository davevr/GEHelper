﻿
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
using GEHelper.Core;


namespace GEHelper
{
	public class ScanViewFragment : Android.Support.V4.App.Fragment
	{
		public GEHelper.Activities.SearchAndScanActivity BaseView;
		public Button ScanNowBtn;
		public Button ScanFilterBtn;
		public Button CancelScanBtn;
		public ListView TargetList;


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.ScanViewFragment, container, false);

			ScanNowBtn = view.FindViewById<Button>(Resource.Id.scanBtn);
			ScanFilterBtn = view.FindViewById<Button>(Resource.Id.scanFilteredBtn);
			CancelScanBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
			TargetList = view.FindViewById<ListView>(Resource.Id.enemyList);
			TargetList.Adapter = new TargetListAdapter(this.Activity, null, TargetList);
			TargetList.ChoiceMode = ChoiceMode.Multiple;
            TargetList.ItemClick += TargetList_ItemClick;

			ScanNowBtn.Click += ScanNowBtn_Click;
			CancelScanBtn.Click += CancelScanBtn_Click;
			CancelScanBtn.Enabled = false;

			ScanFilterBtn.Click += ScanFilterBtn_Click;

			return view;
		}

        void TargetList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            bool    isChecked =  !TargetList.IsItemChecked(e.Position);
            TargetList.SetItemChecked(e.Position, isChecked);
            GEGalaxyPlanet selectedPlanet = GEServer.Instance.FilteredScanResults[e.Position];
            if (selectedPlanet == BaseView.TargetPlanet)
                BaseView.TargetPlanet = null;
            else
                BaseView.TargetPlanet = selectedPlanet;  
        }



        private void UpdateSelection()
        {
            BaseView.UpdateCounters();
        }

		public void Refresh()
		{
			try
			{
				TargetList.InvalidateViews();
				TargetList.SmoothScrollToPosition(0);
				BaseView.UpdateCounters();

			}
			catch (Exception exp)
			{
				System.Console.WriteLine("Refresh failed:  " + exp.Message);
				// do nothing
			}
		}

		public void CancelScanBtn_Click(object sender, EventArgs e)
		{
			BaseView.UserCancelScan ();
		}

		public void ScanNowBtn_Click(object sender, EventArgs e)
		{
			BaseView.UserStartScan (false);
		}

		public void ScanFilterBtn_Click(object sender, EventArgs e)
		{
			BaseView.UserStartScan (true);
		}

		public void ShowScanProgress()
		{

		}


		public void HideScanProgress()
		{

		}


		public void UpdateScanProgress(int curGalaxy, int curSystem)
		{
			this.Activity.RunOnUiThread(() =>
				{
					BaseView.SetStatus("Now scanning system " + curSystem.ToString() + " in galaxy " + curGalaxy.ToString());


				});
		}
	}
}

