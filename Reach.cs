using Google.Ads.GoogleAds;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V11.Errors;
using Google.Ads.GoogleAds.V11.Services;
using Google.Protobuf.Collections;

using System;

namespace Maseebat
{
    internal static class Reach
    {
        internal static ReachPlanServiceClient GetReachPlanServiceClient(GoogleAdsClient client)
        {
            return client.GetService(Services.V11.ReachPlanService);
        }

        public static RepeatedField<PlannableLocation> GetPlannableLocations(ReachPlanServiceClient reachPlanServiceClient)
        {
            ListPlannableLocationsRequest request = new ListPlannableLocationsRequest();
            ListPlannableLocationsResponse response = null;
            try
            {
                response = reachPlanServiceClient.ListPlannableLocations(request);
            }
            catch (GoogleAdsException e)
            {
                Console.WriteLine(e.Failure.ToString());
                return null;
            }
            return response.PlannableLocations;
        }

        internal static ListPlannableProductsResponse GetPlannableProducts(ReachPlanServiceClient reachPlanServiceClient, string locationId)
        {
            ListPlannableProductsRequest request = new ListPlannableProductsRequest
            {
                PlannableLocationId = locationId
            };
            ListPlannableProductsResponse response;
            try
            {
                response = reachPlanServiceClient.ListPlannableProducts(request);
            }
            catch (GoogleAdsException)
            {
                return null;
            }
            return response;
        }
    }
}
