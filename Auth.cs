using Google.Ads.GoogleAds.Config;
using Google.Ads.GoogleAds.Lib;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maseebat
{
    internal static class Auth
    {
        internal static (GoogleAdsClient adsClient, UserCredential credential) Authorise(string jsonFile, string devToken, string loginCustomerId)
        {
            if (!File.Exists(jsonFile))
            {
                return (null, null);
            }

            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText(jsonFile));

            if (null == jsonObj.installed)
            {
                return (null, null);
            }

            // Load the JSON secrets.
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = (string)jsonObj.installed.client_id.Value,
                ClientSecret = (string)jsonObj.installed.client_secret,

            };

            // Authorize the user using desktop application flow.
            Task<UserCredential> task = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets,
                "https://www.googleapis.com/auth/adwords".Split(','),
                "user",
                CancellationToken.None,
                new FileDataStore($"Maseebat-" + Path.GetFileNameWithoutExtension(jsonFile), false)
            );
            UserCredential credential = task.Result;

            // Store this token for future use.

            // To make a call, set the refreshtoken to the config, and
            // create the GoogleAdsClient.
            GoogleAdsClient client = new GoogleAdsClient(new GoogleAdsConfig
            {
                OAuth2RefreshToken = credential.Token.RefreshToken,
                DeveloperToken = devToken,
                LoginCustomerId = loginCustomerId,
                OAuth2ClientId = (string)jsonObj.installed.client_id.Value,
                OAuth2ClientSecret = (string)jsonObj.installed.client_secret
            });
            // var cfgdata = client.Config;
            // Now use the client to create services and make API calls.
            // ...
            return (client, credential);
        }
    }
}
