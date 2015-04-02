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

using GEHelper.Core;

namespace GEHelper.Activities
{
    [Activity(Label = "SearchAndScanActivity")]
    public class SearchAndScanActivity : Fragment
    {
        public SummaryScreen SummaryPage { get; set; }
        private int curGalaxy, curSystem;
        private CancellationToken token;
        private CancellationTokenSource tokenSource;
        private Task scanTask = null;

        private CheckBox showFilterCheckbox;
        private EditText userNameField;
        private CheckBox nearRankCheckbox;
        private CheckBox nearPlanetCheckbox;
        private CheckBox lanxableCheckbox;
        private EditText rankRangeField;
        private EditText planetRangeField;
        private TextView StatusField;

        private Button ScanNowBtn;
        private Button CancelScanBtn;
        private Button SaveScanBtn;
        private Button FilterNowBtn;
        private Button ClearFilterBtn;

        private LinearLayout FilterArea;
        private ListView TargetList;

        private string scanName;
        private bool useRank;
        private int rankRange;
        private bool usePlanets;
        private int planetRange;
        private bool useLanx;
        private int myRank;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.SearchAndScan, container, false);

            userNameField = view.FindViewById<EditText>(Resource.Id.usernameField);
            rankRangeField = view.FindViewById<EditText>(Resource.Id.rankRangeField);
            planetRangeField = view.FindViewById<EditText>(Resource.Id.nearPlanetField);

            showFilterCheckbox = view.FindViewById<CheckBox>(Resource.Id.showFiltersCheckbox);
            nearRankCheckbox = view.FindViewById<CheckBox>(Resource.Id.nearRankCheckbox);
            nearPlanetCheckbox = view.FindViewById<CheckBox>(Resource.Id.nearPlanetCheckbox);
            lanxableCheckbox = view.FindViewById<CheckBox>(Resource.Id.lanxCheckbox);

            ScanNowBtn = view.FindViewById<Button>(Resource.Id.scanBtn);
            CancelScanBtn = view.FindViewById<Button>(Resource.Id.cancelBtn);
            SaveScanBtn = view.FindViewById<Button>(Resource.Id.saveBtn);
            FilterNowBtn = view.FindViewById<Button>(Resource.Id.filterNowBtn);
            ClearFilterBtn = view.FindViewById<Button>(Resource.Id.clearFilterBtn);
            FilterArea = view.FindViewById <LinearLayout>(Resource.Id.FilterLayout);

            TargetList = view.FindViewById<ListView>(Resource.Id.enemyList);
            TargetList.Adapter = new TargetListAdapter(this.Activity, null);

            StatusField = view.FindViewById<TextView>(Resource.Id.statusField);


            ScanNowBtn.Click += ScanNowBtn_Click;
            CancelScanBtn.Click += CancelScanBtn_Click;
            FilterNowBtn.Click += FilterNowBtn_Click;
            ClearFilterBtn.Click += ClearFilterBtn_Click;
            FilterArea.Visibility = ViewStates.Gone;
            CancelScanBtn.Enabled = false;
            SaveScanBtn.Enabled = false;
            showFilterCheckbox.CheckedChange += showFilterCheckbox_CheckedChange;

            int.TryParse(GEServer.Instance.ServerState.user.rank, out myRank);
            
            return view;
        }

        void ClearFilterBtn_Click(object sender, EventArgs e)
        {
            ClearFilters();
            Refresh();
        }

        void FilterNowBtn_Click(object sender, EventArgs e)
        {
            scanName = userNameField.Text;
            useRank = nearRankCheckbox.Checked;
            if (useRank)
                int.TryParse(rankRangeField.Text, out rankRange);
            usePlanets = nearPlanetCheckbox.Checked;
            if (usePlanets)
                int.TryParse(planetRangeField.Text, out planetRange);
            useLanx = lanxableCheckbox.Checked;
            ApplyFilters();
            Refresh();
        }

        void showFilterCheckbox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (showFilterCheckbox.Checked)
                FilterArea.Visibility = ViewStates.Visible;
            else
                FilterArea.Visibility = ViewStates.Gone;
        }

        void CancelScanBtn_Click(object sender, EventArgs e)
        {
            if (scanTask != null)
            {
                tokenSource.Cancel();
            }
        }

        void ScanNowBtn_Click(object sender, EventArgs e)
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            scanTask = new Task(StartScan, token);
            scanTask.Start();
        }



        public void Refresh()
        {
            try
            {
                TargetList.InvalidateViews();
                TargetList.SmoothScrollToPosition(0);
                int total = GEServer.Instance.ScanResults.Count;
                int showing = GEServer.Instance.FilteredScanResults.Count;
                if (total == showing)
                    StatusField.Text = String.Format("{0} targets found", total);
                else if (showing == 0)
                    StatusField.Text = String.Format("None of the {0} results match filter", total);
                else
                    StatusField.Text = String.Format("Showing {0:0}/{1:0} targets", showing, total);

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
                    ScanNowBtn.Enabled = false;
                    CancelScanBtn.Enabled = true;
                    GEServer.Instance.ScanResults.Clear();
                    GEServer.Instance.FilteredScanResults.Clear();
                    TargetList.Enabled = false;

                });

            curGalaxy = 1;
            curSystem = 1;

            ScanNextSystem();
        }

        private void ContinueScan()
        {
            this.Activity.RunOnUiThread(() =>
            {
                ScanNowBtn.Enabled = false;
                CancelScanBtn.Enabled = true;

            });


            ScanNextSystem();
        }

        private void ScanNextSystem()
        {
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
                if (curSystem > 500)
                {
                    curSystem = 1;
                    curGalaxy++;
                }
                if (token.IsCancellationRequested || (curGalaxy > 5))
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
                ScanNowBtn.Enabled = true;
                CancelScanBtn.Enabled = false;
                StatusField.Text = "Scanning found " + GEServer.Instance.ScanResults.Count.ToString() + " targets";
                TargetList.Enabled = true;
                ClearFilters();
                Refresh();
            });

        }

        private void ApplyFilters()
        {
            GEServer.Instance.FilteredScanResults.Clear();
            foreach (GEGalaxyPlanet curPlanet in GEServer.Instance.ScanResults)
            {
                if (CheckName(curPlanet) && CheckRank(curPlanet) && CheckRange(curPlanet) && CheckLanx(curPlanet))
                    GEServer.Instance.FilteredScanResults.Add(curPlanet);
            }
        }

        private bool CheckName(GEGalaxyPlanet curPlanet)
        {
            if (String.IsNullOrWhiteSpace(scanName) || (String.Compare(scanName, curPlanet.username, StringComparison.OrdinalIgnoreCase) == 0))
                return true;
            else
                return false;
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

        private bool CheckRange(GEGalaxyPlanet curPlanet)
        {
            return true;
        }

        private bool CheckLanx(GEGalaxyPlanet curPlanet)
        {
            return true;
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
            this.Activity.RunOnUiThread(() =>
            {
                StatusField.Text = "Now scanning system " + sol.ToString() + " in galaxy " + gal.ToString();


            });

        }
    }
}