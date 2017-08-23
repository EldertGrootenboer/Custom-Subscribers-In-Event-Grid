using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Eldert.EventGrid.Subscriber.Models;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Eldert.EventGrid.Subscriber.Controllers
{
    public class SubscribersController : ApiController
    {
        /// <summary>
        /// Storage account used to store to Table Storage.
        /// </summary>
        private readonly CloudStorageAccount _storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureStorageConnectionString"));

        /// <summary>
        /// Receives all events.
        /// </summary>
        [ActionName("All")]
        public async Task<StatusCodeResult> PostAll([FromBody] List<ShipEventValue> value)
        {
            await InsertToTable(value, "All");
            return new StatusCodeResult(HttpStatusCode.Created, this);
        }

        /// <summary>
        /// Receives all types of events for the ship Hydra.
        /// </summary>
        [ActionName("Hydra")]
        public async Task<StatusCodeResult> PostHydra([FromBody] List<ShipEventValue> value)
        {
            await InsertToTable(value, "Hydra");
            return new StatusCodeResult(HttpStatusCode.Created, this);
        }

        /// <summary>
        /// Receives repairs for all ships.
        /// </summary>
        [ActionName("Repairs")]
        public async Task<StatusCodeResult> PostRepairs([FromBody] List<ShipEventValue> value)
        {
            await InsertToTable(value, "Repairs");
            return new StatusCodeResult(HttpStatusCode.Created, this);
        }

        /// <summary>
        /// Receives orders for the ship Aeris.
        /// </summary>
        [ActionName("AerisOrders")]
        public async Task<StatusCodeResult> PostAerisOrders([FromBody] List<ShipEventValue> value)
        {
            await InsertToTable(value, "AerisOrders");
            return new StatusCodeResult(HttpStatusCode.Created, this);
        }

        /// <summary>
        /// Insert Ship Event into table storage.
        /// </summary>
        private async Task InsertToTable(IReadOnlyList<ShipEventValue> value, string tableName)
        {
            // Check if any events were received
            if (value == null || value.Count == 0)
            {
                return;
            }

            // Create the table client
            var tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist
            table.CreateIfNotExists();

            // Create a new ship event entity
            var shipEventEntity = new ShipEventEntity(value[0].Data.Ship, value[0].EventTime)
            {
                Type = value[0].Data.Type,
                Product = value[0].Data.Product,
                Amount = value[0].Data.Amount,
                Device = value[0].Data.Device,
                Description = value[0].Data.Description
            };

            // Create the TableOperation object that inserts the customer entity
            var insertOperation = TableOperation.Insert(shipEventEntity);

            // Execute the insert operation
            await table.ExecuteAsync(insertOperation);
        }
    }
}