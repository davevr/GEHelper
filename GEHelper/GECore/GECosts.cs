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

namespace GEHelper.Core
{
    class GECosts
    {
        public static Resources RocketLauncherCost = new Resources(2000, 0, 0, 0);
        public static Resources LightLaserCost = new Resources(1500, 500, 0, 0);
        public static Resources heavyLaserCost = new Resources(8000, 2000, 0, 0);
        public static Resources GaussCannonCost = new Resources(20000, 15000, 2000, 0);
        public static Resources IonCannonCost = new Resources(2000, 6000, 0, 0);
        public static Resources PlasmaCannotCost = new Resources(50000, 50000, 30000, 0);
        public static Resources SmallShieldDomeCost = new Resources(10000, 10000, 0, 0);
        public static Resources LargeShieldDomeCost = new Resources(50000, 50000, 0, 0);
        public static Resources AntiBallisticMissileCost = new Resources(8000, 2000, 0, 0);
        public static Resources InterPlanetaryMissileCost = new Resources(12500, 2500, 10000, 0);


        public static Resources SmallCargoCost = new Resources(2000, 2000, 0, 0);
        public static Resources LargeCargoCost = new Resources(6000, 6000, 0, 0);
        public static Resources LightFighterCost = new Resources(3000, 1000, 0, 0);
        public static Resources HeavyFighterCost = new Resources(6000, 4000, 0, 0);
        public static Resources CruiserCost = new Resources(20000, 7000, 2000, 0);
        public static Resources BattleshipCost = new Resources(45000, 15000, 0, 0);
        public static Resources ColonyShipCost = new Resources(10000, 20000, 10000, 0);
        public static Resources RecyclerCost = new Resources(10000, 6000, 2000, 0);
        public static Resources EspionageProbeCost = new Resources(0, 1000, 0, 0);
        public static Resources BomberCost = new Resources(50000, 25000, 15000, 0);
        public static Resources SolarSatelliteCost = new Resources(0, 2000, 500, 0);
        public static Resources DestroyerCost = new Resources(60000, 50000, 15000, 0);
        public static Resources DeathstarCost = new Resources(5000000, 4000000, 1000000, 0);
        public static Resources BattleCruiserCost = new Resources(30000, 40000, 15000, 0);

       

    }
}