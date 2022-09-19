using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.Util;
using Google.Apis.Auth.OAuth2;

using System;
using System.Diagnostics;
using System.IO;

namespace Maseebat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3) return;
            var authJson = args[0];
            var devId = args[1];
            var loginCustomerId = args[2];
            
            EnableTrace("Maseebat");
            (GoogleAdsClient AdsClient, UserCredential userCred) = Auth.Authorise(authJson, devId, loginCustomerId);
            var reachPlanServiceClient = Reach.GetReachPlanServiceClient(AdsClient);
            var locations = Reach.GetPlannableLocations(reachPlanServiceClient);
            if (locations == null) return;
            var products = Reach.GetPlannableProducts(reachPlanServiceClient, "1000676");

        }

        internal static void EnableTrace(string filenamePrefix)
        {
            TraceUtilities.Configure(TraceUtilities.DETAILED_REQUEST_LOGS_SOURCE,
                         Path.Combine(Path.GetTempPath(), $"{filenamePrefix}_{DateTime.UtcNow:yyyy'-'MM'-'dd'-'HH'-'mm'-'ss'-'ffff}.log"),
                         SourceLevels.All);
        }
    }
}
