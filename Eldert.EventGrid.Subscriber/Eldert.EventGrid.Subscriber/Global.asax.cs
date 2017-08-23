using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Eldert.EventGrid.Subscriber.Models;

using Microsoft.Azure;

using Newtonsoft.Json;

namespace Eldert.EventGrid.Subscriber
{
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// Subscriptions to be created.
        /// </summary>
        private readonly List<Subscription> _subscriptions = new List<Subscription>
        {
            new Subscription { Name = "All" },
            new Subscription { Name = "Hydra", PrefixFilter = "Hydra" },
            new Subscription { Name = "Repairs", SuffixFilter = "Repair" },
            new Subscription { Name = "AerisOrders", PrefixFilter = "Aeris", SuffixFilter = "Order" }
        };

        /// <summary>
        /// Entry point of application.
        /// </summary>
        protected async void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            await CreateSubscriptions();
        }

        /// <summary>
        /// Create subscriptions that don't exist.
        /// </summary>
        private async Task CreateSubscriptions()
        {            
            // Check if subscriptions can be created, this will only be done if the endpoint of this API App has been updated in the settings
            if (CloudConfigurationManager.GetSetting("ApiAppUrl").ToLowerInvariant().Contains("tobereplaced"))
            {
                return;
            }
            
            // Loop through subsriptions
            foreach (var subscription in _subscriptions)
            {
                // Check if subscription already exists
                if (await SubscriptionExists(subscription.Name))
                {
                    continue;
                }

                // Create subscription
                await CreateSubscription(subscription.Name, subscription.PrefixFilter, subscription.SuffixFilter);

                // Wait for a while, to prevent throttling
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Check if subscription exists.
        /// </summary>
        private static async Task<bool> SubscriptionExists(string subscription)
        {
            // Check if subscription exists
            var result = await CreateHttpClient()
                .GetAsync(
                    $"https://management.azure.com/subscriptions/{CloudConfigurationManager.GetSetting("SubscriptionID")}/resourceGroups/{CloudConfigurationManager.GetSetting("ResourceGroup")}/providers/Microsoft.EventGrid/topics/{CloudConfigurationManager.GetSetting("EventGridTopic")}/providers/Microsoft.EventGrid/eventSubscriptions/{subscription}?api-version=2017-06-15-preview");
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// Create subscription with filters.
        /// </summary>
        private static async Task CreateSubscription(string subscription, string prefixFilter, string suffixFilter)
        {
            // Set up create subscription message
            var createSubscription = new
            {
                properties = new
                {
                    destination = new { endpointType = "webhook", properties = new { endpointUrl = $"{CloudConfigurationManager.GetSetting("ApiAppUrl")}/api/Subscribers/{subscription}" } },
                    filter = new { includedEventTypes = new[] { "shipevent" }, subjectBeginsWith = prefixFilter, subjectEndsWith = suffixFilter, subjectIsCaseSensitive = "false" }
                }
            };

            // Create content to be sent
            var json = JsonConvert.SerializeObject(createSubscription);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Create subscription
            await CreateHttpClient()
                .PutAsync(
                    $"https://management.azure.com/subscriptions/{CloudConfigurationManager.GetSetting("SubscriptionID")}/resourceGroups/{CloudConfigurationManager.GetSetting("ResourceGroup")}/providers/Microsoft.EventGrid/topics/{CloudConfigurationManager.GetSetting("EventGridTopic")}/providers/Microsoft.EventGrid/eventSubscriptions/{subscription}?api-version=2017-06-15-preview",
                    content);
        }

        /// <summary>
        /// Create HTTP client used to communicate with Azure.
        /// </summary>
        /// <returns></returns>
        private static HttpClient CreateHttpClient()
        {
            // Create a HTTP client
            var httpClient = new HttpClient();

            // Add key in the request headers
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {CloudConfigurationManager.GetSetting("AuthorizationToken")}");

            // Return the HTTP client
            return httpClient;
        }
    }
}