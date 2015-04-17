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
using System.Threading;
using System.Threading.Tasks;
using Android.Support.V4.View;
using Android.Support.V4.App;

using GEHelper.Core;
using com.refractored;

namespace GEHelper.Activities
{
    [Activity(Label = "SearchAndScanActivity")]
	public class SearchAndScanActivity : Android.Support.V4.App.Fragment
    {
        public SummaryScreen SummaryPage { get; set; }
        private int curGalaxy, curSystem;
        private CancellationToken token;
        private CancellationTokenSource tokenSource;
        private Task scanTask = null;
		private TextView StatusField;
		private TextView TotalCountField;
		private TextView ShownCountField;
		private TextView SelectedCountField;


		public static FilterViewFragment FilterView;
		public static ScanViewFragment ScanView;
		public static ActionViewFragment ActionView;

		// filters
		public string scanName;
		public bool useRank;
		public int rankRange;
		public bool usePlanets;
		public int planetRange;
		public bool useLanx;
		public bool useUsername;
		public bool useAlliance;
		public string allianceName;
		public bool useDebris;
		public long debrisSize;
		public bool includeOwnPlanets;
        public bool inactiveOnly;
        public bool useLimits;
        public int startGalaxy;
        public int startSystem;
        public int endGalaxy;
        public int endSystem;



		private int myRank;

		public class ScanPageAdapter : FragmentPagerAdapter
        {
			private  string[] Titles = {"Scan", "Filter", "Action"};


			public ScanPageAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
			{
			}

			public override Java.Lang.ICharSequence GetPageTitleFormatted (int position)
			{
				return new Java.Lang.String (Titles [position]);
			}

			public override int Count {
				get {
					return Titles.Length;
				}
			}

			public override Android.Support.V4.App.Fragment GetItem (int position)
			{
				Android.Support.V4.App.Fragment theItem = null;
				switch (position) {
				case 0:
					theItem = SearchAndScanActivity.ScanView;
					break;

				case 1:
					theItem = SearchAndScanActivity.FilterView;
					break;

				case 2:
					theItem = SearchAndScanActivity.ActionView;
					break;

				}
				return theItem;
			}
		}


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ClearFilters();
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SearchAndScan, container, false);
			StatusField = view.FindViewById<TextView>(Resource.Id.statusField);
			TotalCountField = view.FindViewById<TextView>(Resource.Id.totalCount);
			ShownCountField = view.FindViewById<TextView>(Resource.Id.shownCount);
			SelectedCountField = view.FindViewById<TextView>(Resource.Id.selectedCount);

			FilterView = new FilterViewFragment ();
			FilterView.BaseView = this;

			ScanView = new ScanViewFragment ();
			ScanView.BaseView = this;

			ActionView = new ActionViewFragment ();
			ActionView.BaseView = this;

			int.TryParse(GEServer.Instance.ServerState.user.rank, out myRank);

            var pager = view.FindViewById<ViewPager>(Resource.Id.scan_pager);
            pager.Adapter = new ScanPageAdapter(this.FragmentManager);

            var tabs = view.FindViewById<PagerSlidingTabStrip>(Resource.Id.scan_tabs);
            tabs.SetViewPager(pager);

			//Refresh ();
            return view;
     
        }

		public void UpdateCounters()
		{
			int totalCount = GEServer.Instance.ScanResults.Count;
			int shownCount = GEServer.Instance.FilteredScanResults.Count;
			int selectedCount = ScanView.TargetList.CheckedItemCount;

			TotalCountField.Text = string.Format ("{0} total", totalCount);
			ShownCountField.Text = string.Format ("{0} shown", shownCount);
			SelectedCountField.Text = string.Format ("{0} selected", selectedCount);
		}

		public void SetStatus(string statusText)
		{
			StatusField.Text = statusText;
		}


        public void UserClearFilters()
        {
            ClearFilters();
            Refresh();
        }

        public void UserApplyFilters()
        {
            ApplyFilters();
            Refresh();
        }


		public void UserCancelScan()
        {
            if (scanTask != null)
            {
                tokenSource.Cancel();
            }
        }

		public void UserStartScan()
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            scanTask = new Task(StartScan, token);
			GEPlanet bestPlanet = FindPlanetWithMostDeut ();

			if (bestPlanet == GEServer.Instance.CurrentPlanet)
				scanTask.Start ();
			else {
				GEServer.Instance.SetPlanet(bestPlanet.id, (theResult) =>
					{
						scanTask.Start();
					});
			}
        }



        public void Refresh()
        {
            try
            {
				ScanView.Refresh();
				UpdateCounters();

            }
            catch (Exception exp)
            {
                System.Console.WriteLine("Refresh failed:  " + exp.Message);
                // do nothing
            }
        }

        private void StartScan()
        {
            this.Activity.RunOnUiThread(() =>
                {
					ScanView.ShowScanProgress();
                    ScanView.ScanNowBtn.Enabled = false;
					ScanView.CancelScanBtn.Enabled = true;
                    GEServer.Instance.ScanResults.Clear();
                    GEServer.Instance.FilteredScanResults.Clear();
					ScanView.TargetList.Enabled = false;

                });

            if (!useLimits)
            {
                startGalaxy = 1;
                startSystem = 1;
                endGalaxy = 5;
                endSystem = 500;
            }

            curGalaxy = startGalaxy;
            curSystem = startSystem;

			ScanNextSystem();
        }

        private void ContinueScan()
        {
            this.Activity.RunOnUiThread(() =>
            {
				ScanView.ScanNowBtn.Enabled = false;
				ScanView.CancelScanBtn.Enabled = true;

            });


            ScanNextSystem();
        }

		private GEPlanet FindPlanetWithMostDeut()
		{
			double curMax = double.MinValue;
			GEPlanet bestPlanet = null;

			foreach (GEPlanet curPlanet in GEServer.Instance.ServerState.planetList) {
				if (curPlanet.deuterium > curMax) {
					curMax = curPlanet.deuterium;
					bestPlanet = curPlanet;
				}
			}
			return bestPlanet;
		}

        private void ScanNextSystem()
        {
            bool scanEnded = false;
            AsyncUpdateCount(curGalaxy, curSystem);
            GEServer.Instance.ScanGalaxy(curGalaxy, curSystem, (resultList) =>
            {
                if (resultList != null)
                {
                    foreach (GEGalaxyPlanet curPlanet in resultList)
                    {
                        GEServer.Instance.ScanResults.Add(curPlanet);
                    }
                }

                curSystem++;
                if  (curGalaxy == endGalaxy)
                {
                    if (curSystem > endSystem)
                        scanEnded = true;
                }
                else
                {
                    if (curSystem > 500)
                    {
                        curSystem = 1;
                        curGalaxy++;
                        if (curGalaxy > endGalaxy)
                            scanEnded = true;
                    }
                }
                
                if (token.IsCancellationRequested || scanEnded)
                {
                    AsyncTaskComplete();
                }
                else
                    ScanNextSystem();
            });

        }

        private void AsyncTaskComplete()
        {
            this.Activity.RunOnUiThread(() =>
	            {
					ScanView.HideScanProgress();
					ScanView.ScanNowBtn.Enabled = true;
					ScanView.CancelScanBtn.Enabled = false;
					SetStatus("Scan complete");
					UpdateCounters();
					ScanView.TargetList.Enabled = true;
	                ClearFilters();
	                Refresh();
	            });

        }

        private void ApplyFilters()
        {
            GEServer.Instance.FilteredScanResults.Clear();
            foreach (GEGalaxyPlanet curPlanet in GEServer.Instance.ScanResults)
            {
                if (CheckName(curPlanet) && 
					CheckRank(curPlanet) && 
					CheckRange(curPlanet) && 
					CheckAlliance(curPlanet) &&
					CheckDebris(curPlanet) &&
					CheckOwnPlanet(curPlanet) &&
                    CheckInactive(curPlanet) &&
                    CheckScanLimit(curPlanet) &&
					CheckLanx(curPlanet))
                    GEServer.Instance.FilteredScanResults.Add(curPlanet);
            }
        }

        private bool CheckName(GEGalaxyPlanet curPlanet)
        {
			if (!useUsername || (String.IsNullOrWhiteSpace(scanName) || (String.Compare(scanName, curPlanet.username, StringComparison.OrdinalIgnoreCase) == 0)))
                return true;
            else
                return false;
        }

		private bool CheckAlliance(GEGalaxyPlanet curPlanet)
		{
			if (!useAlliance || (String.IsNullOrWhiteSpace(allianceName) || (String.Compare(allianceName, curPlanet.ally_name, StringComparison.OrdinalIgnoreCase) == 0)))
				return true;
			else
				return false;
		}

        private bool CheckInactive(GEGalaxyPlanet curPlanet)
        {
            if (!inactiveOnly || !curPlanet.IsActive)
                return true;
            else
                return false;
        }

        private bool CheckScanLimit(GEGalaxyPlanet curPlanet)
        {
            if (useLimits)
            {
                if (curPlanet.g >= startGalaxy && curPlanet.g <= endGalaxy &&
                    curPlanet.s >= startSystem && curPlanet.s <= endSystem)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        private bool CheckRank(GEGalaxyPlanet curPlanet)
        {
            int rank = 0;
            int.TryParse(curPlanet.rank, out rank);
            if (!useRank || ((rank > myRank - rankRange) && (rank < myRank + rankRange) && (curPlanet.vacation == "0")))
                return true;
            else
                return false;
        }

		private bool CheckDebris(GEGalaxyPlanet curPlanet)
		{
			if (useDebris) {
				long	crystal = 0, metal = 0;
				long.TryParse (curPlanet.debries_metal, out metal);
				long.TryParse (curPlanet.debries_crystal, out crystal);
				long totalDebris = metal + crystal;
				return (totalDebris >= debrisSize);

			} else
				return true;
			
		}

		private bool CheckOwnPlanet(GEGalaxyPlanet curPlanet)
		{
			if (includeOwnPlanets)
				return true;
			else
                return (curPlanet.user_id != GEServer.Instance.ServerState.user.id);
		}

        private bool CheckRange(GEGalaxyPlanet curPlanet)
        {
			if (!usePlanets || (GEServer.Instance.GetNearestPlanetInGalaxy (curPlanet.g, curPlanet.s, planetRange) != null))
				return true;
			else
				return false;
        }

        private bool CheckLanx(GEGalaxyPlanet curPlanet)
        {
			if (!useLanx || (GEServer.Instance.GetLanxable (curPlanet.g, curPlanet.s) != null))
				return true;
			else
				return false;
        }

        private void ClearFilters()
        {
            GEServer.Instance.FilteredScanResults.Clear();
           
            foreach (GEGalaxyPlanet curPlanet in GEServer.Instance.ScanResults)
            {
                GEServer.Instance.FilteredScanResults.Add(curPlanet);
            }
        }

        private void AsyncUpdateCount(int gal, int sol)
        {
			ScanView.UpdateScanProgress (gal, sol);

        }
    }
}