
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
using System.Timers;
using System.Threading;
using System.Threading.Tasks;


namespace GEHelper
{
	public class ActionViewFragment : Android.Support.V4.App.Fragment
	{
		public GEHelper.Activities.SearchAndScanActivity BaseView;
        private Button sendSpyBtn;
        private Button startBombardBtn;
        private Button endBombardBtn;
        private Button copyResutlsBtn;
        private TextView bombardLabel;
        private TextView sentProbeField;
        private TextView receivedProbeField;
        private LinearLayout statusArea;
        private int numToSend = 0;
        private int numSent = 0;
        private int numReceived = 0;
        private System.Timers.Timer waitTimer = null;
        private CancellationToken token;
        private CancellationTokenSource tokenSource;
        private Task bombardTask = null;


		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate(Resource.Layout.ActionViewFragment, container, false);

            sentProbeField = view.FindViewById<TextView>(Resource.Id.sendProbeField);
            receivedProbeField = view.FindViewById<TextView>(Resource.Id.receivedProbeField);
            statusArea = view.FindViewById<LinearLayout>(Resource.Id.statusArea);
            sendSpyBtn = view.FindViewById<Button>(Resource.Id.SendSpiesBtn);
            sendSpyBtn.Click += sendSpyBtn_Click;

            startBombardBtn = view.FindViewById<Button>(Resource.Id.StartBombardBtn);
            endBombardBtn = view.FindViewById<Button>(Resource.Id.EndBombardBtn);
            copyResutlsBtn = view.FindViewById<Button>(Resource.Id.CopyResultsBtn);
            bombardLabel = view.FindViewById<TextView>(Resource.Id.bombardStatusField);

            startBombardBtn.Enabled = true;
            endBombardBtn.Enabled = false;

            copyResutlsBtn.Click += copyResutlsBtn_Click;
            startBombardBtn.Click += startBombardBtn_Click;
            endBombardBtn.Click += endBombardBtn_Click;

            statusArea.Visibility = ViewStates.Gone;

			return view;
		}

        public void UpdateForSelection()
        {
            if (bombardLabel != null)
                UpdateBombardStatus();
        }

        void UpdateBombardStatus()
        {
            this.BaseView.Activity.RunOnUiThread(() =>
            {
                if (BaseView.TargetPlanet != null)
                {
                    bombardLabel.Text = String.Format("bombard {0} ({1}:{2}:{3})", BaseView.TargetPlanet.name, BaseView.TargetPlanet.g, BaseView.TargetPlanet.s, BaseView.TargetPlanet.p);
                }
                else
                {
                    bombardLabel.Text = String.Format("bombard {0} targets", GEServer.Instance.FilteredScanResults.Count);
                }
            });

        }

        void endBombardBtn_Click(object sender, EventArgs e)
        {
            UserCancelBombard();
        }

        void startBombardBtn_Click(object sender, EventArgs e)
        {
            UserStartBombard(); 
        }

        public void UserCancelBombard()
        {
            if (bombardTask != null)
            {
                tokenSource.Cancel();
            }
        }

        public void UserStartBombard()
        {
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            bombardTask = new Task(StartBombard, token);
            bombardTask.Start();
        }

        private void StartBombard()
        {
            this.Activity.RunOnUiThread(() =>
            {
                endBombardBtn.Enabled = true;
                startBombardBtn.Enabled = false;
            });

            BombardActiveList();
        }

        private void BombardActiveList()
        {
            List<GEGalaxyPlanet> bombardList;

            if (BaseView.TargetPlanet == null)
                bombardList = new List<GEGalaxyPlanet>(GEServer.Instance.FilteredScanResults);
            else
            {
                bombardList = new List<GEGalaxyPlanet>();
                bombardList.Add(BaseView.TargetPlanet);

            }

            BombardNextPlanet(bombardList);
        }

        private void BombardNextPlanet(List<GEGalaxyPlanet> curList)
        {
            GEGalaxyPlanet curPlanet = curList[0];

            curList.RemoveAt(0);
            int maxRange = (5 * int.Parse(GEServer.Instance.ServerState.user.impulse_drive_tech)) - 1;
            GEPlanet basePlanet = GEServer.Instance.GetNearestPlanetInGalaxy(curPlanet.g, curPlanet.s, maxRange);

            if (basePlanet != null)
            {
                if (basePlanet != GEServer.Instance.CurrentPlanet)
                {
                    GEServer.Instance.SetPlanet(basePlanet.id, (theResult) =>
                    {
                        BombardPlanet(curPlanet, curList);
                    });
                }
                else
                    BombardPlanet(curPlanet, curList);
            }
            else
            {
                FinishBombardPlanet(curList);
            }
            
            
        }

        private void BombardPlanet(GEGalaxyPlanet curPlanet, List<GEGalaxyPlanet> curList)
        {
            Defense defList = new Defense();
            int maxMissiles = int.Parse(GEServer.Instance.CurrentPlanet.missile_silo) * 5;
            maxMissiles -= int.Parse(GEServer.Instance.CurrentPlanet.anti_ballistic_missiles) / 2;
            int needed = maxMissiles - int.Parse(GEServer.Instance.CurrentPlanet.interplanetary_missiles);
            defList.interplanetary_missiles = needed;
            int sleepTime = (int)defList.GetBuildTime();

            GEServer.Instance.BuildDefense(defList, (theBuildResult) =>
                {
                    Thread.Sleep(sleepTime * 1000); // let them build before launch
                    // next planet
                    GEServer.Instance.Bombard(curPlanet.g, curPlanet.s, curPlanet.p, maxMissiles, (theResult) =>
                       {
                            FinishBombardPlanet(curList);
                       });

                });
           

        }

        private void FinishBombardPlanet(List<GEGalaxyPlanet> curList)
        {
            if (token.IsCancellationRequested)
            {
                AsyncTaskComplete();
            }
            else
                ContinueBombardment(curList);
        }

        private void ContinueBombardment(List<GEGalaxyPlanet> curList)
        {

            if (curList.Count > 0)
                BombardNextPlanet(curList);
            else
                BombardActiveList();
        }


        private void AsyncUpdateCount(int gal, int sol)
        {
            //ScanView.UpdateScanProgress(gal, sol);

        }

        private void AsyncTaskComplete()
        {
            this.Activity.RunOnUiThread(() =>
            {
                endBombardBtn.Enabled = false;
                startBombardBtn.Enabled = true;
            });

        }

        void copyResutlsBtn_Click(object sender, EventArgs e)
        {
            ClipboardManager clipboard = (ClipboardManager)this.Activity.GetSystemService(Context.ClipboardService);
            string theStr = GetResultString();
            var clip = ClipData.NewPlainText("fleet", theStr);
            clipboard.PrimaryClip = clip;
        }

        private string GetResultString()
        {
            string theStr = "";

            foreach (GEGalaxyPlanet curPlanet in GEServer.Instance.FilteredScanResults)
            {
                theStr += String.Format("{0}:{1}:{2} - {3}",
                    curPlanet.g, curPlanet.s, curPlanet.p, curPlanet.name);
                theStr += System.Environment.NewLine;

                theStr += String.Format("    owned by {0}", curPlanet.username);
                theStr += System.Environment.NewLine;
                theStr += System.Environment.NewLine;
            }

            return theStr;
        }

        void sendSpyBtn_Click(object sender, EventArgs e)
        {
            sendSpyBtn.Enabled = false;
            numToSend = GEServer.Instance.FilteredScanResults.Count;
            numSent = 0;
            numReceived = 0;

            statusArea.Visibility = ViewStates.Visible;
            GEServer.Instance.DeleteAllMail("0", (theStr) =>
                {
                    GalaxyList scanList = new GalaxyList();

                    foreach (GEGalaxyPlanet curPlanet in GEServer.Instance.FilteredScanResults)
                    {
                        scanList.Add(curPlanet);
                        if (curPlanet.moon != null)
                            numToSend++;
                    }
                    UpdateStatusText();
                    MaybeScanNextPlanet(scanList);

                });
        }

        void UpdateStatusText()
        {
            this.Activity.RunOnUiThread(() =>
                {
                    sentProbeField.Text = String.Format("{0}/{1}", numSent, numToSend);
                    receivedProbeField.Text = String.Format("{0}/{1}", numReceived, numSent);
                });
        }

        void MaybeScanNextPlanet(GalaxyList theList)
        {
            if (!GEServer.Instance.IsFleetSlotAvailable)
            {
                DateTime minTime = GEServer.Instance.GetFirstFleetReturnTime();
                TimeSpan waitTime = minTime - DateTime.Now;
                System.Threading.Thread.Sleep(5000);
                GEServer.Instance.Refresh((theResult) =>
                    {
                        MaybeScanNextPlanet(theList);
                    });

            }
            else
            {
                GEGalaxyPlanet curPlanet = theList[0];
                GEPlanet bestPlanet = GEServer.Instance.GetNearestPlanet(curPlanet.g, curPlanet.s, curPlanet.p);

                if (bestPlanet == GEServer.Instance.CurrentPlanet)
                    ScanNextPlanet(theList);
                else
                {
                    GEServer.Instance.SetPlanet(bestPlanet.id, (theResult) =>
                    {
                        ScanNextPlanet(theList);
                    });
                }
            }
        }

        void ScanNextPlanet(GalaxyList theList)
        {
            GEGalaxyPlanet curPlanet = theList[0];
            theList.RemoveAt(0);

            GEServer.Instance.SpyPlanet(curPlanet.g, curPlanet.s, curPlanet.p, "1", (theResult) =>
                {
                    if (theResult == "ok")
                        numSent++;
                    if (curPlanet.moon != null)
                    {
                        MaybeScanMoon(curPlanet, theList);
                    }
                    else
                        ScanNextPlanetInList(theList);
                });
        }

        private void MaybeScanMoon(GEGalaxyPlanet thePlanet, GalaxyList theList)
        {
            if (!GEServer.Instance.IsFleetSlotAvailable)
            {
                DateTime minTime = GEServer.Instance.GetFirstFleetReturnTime();
                TimeSpan waitTime = minTime - DateTime.Now;
                System.Threading.Thread.Sleep(5000);
                GEServer.Instance.Refresh((theResult) =>
                {
                    MaybeScanMoon(thePlanet, theList);
                });

            }
            else
                ScanMoon(thePlanet, theList);
        }

        private void ScanMoon(GEGalaxyPlanet curPlanet, GalaxyList theList)
        {
            GEServer.Instance.SpyPlanet(curPlanet.g, curPlanet.s, curPlanet.p, "3", (theMoonResult) =>
            {
                if (theMoonResult == "ok")
                    numSent++;
                ScanNextPlanetInList(theList);
            });
        }

        void ScanNextPlanetInList(GalaxyList theList)
        {
            UpdateStatusText();
            if (theList.Count > 0)
                MaybeScanNextPlanet(theList);
            else
                WaitForScanToComplete();
        }


        void WaitForScanToComplete()
        {
            waitTimer = new System.Timers.Timer();
            waitTimer.Interval = 2000;
            waitTimer.AutoReset = false;
            waitTimer.Elapsed += waitTmer_Elapsed;
            waitTimer.Start();

        }

        void waitTmer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GEServer.Instance.CheckAllMail((mailCat) =>
                {
                    numReceived = mailCat.SpyReports.totalCount;
                    UpdateStatusText();
                    
                    if (numReceived >= numSent)
                        CompleteSpyMission(mailCat);
                    else
                        waitTimer.Start();
                     

                });
        }

        void CompleteSpyMission(MailCatalog reportList)
        {
            /*
           foreach (MailItem curItem in reportList)
           {
               int g = curItem.message_data.target_g;
               int s = curItem.message_data.target_s;
               int p = curItem.message_data.target_p;

               GEGalaxyPlanet curPlanet = GEServer.Instance.ScanResults.First(planet =>
                   (planet.g == g) && (planet.s == s) && (planet.p == p));

               if (curItem.message_data.target_planet_type == "1")
               {
                   curPlanet.resources = curItem.message_data.resources;
                   curPlanet.fleet = curItem.message_data.ships;
               }
               else
               {
                   GEGalaxyMoon curMoon = curPlanet.moon;
                   curMoon.resources = curItem.message_data.resources;
                   curMoon.fleet = curItem.message_data.ships;
               }
           }
            */

           this.Activity.RunOnUiThread(() =>
               {
                   sendSpyBtn.Enabled = true;
                   statusArea.Visibility = ViewStates.Gone;
               });
            

        }
	}
}

