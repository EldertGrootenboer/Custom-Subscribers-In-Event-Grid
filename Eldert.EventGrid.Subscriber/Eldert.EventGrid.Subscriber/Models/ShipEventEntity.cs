using Microsoft.WindowsAzure.Storage.Table;

namespace Eldert.EventGrid.Subscriber.Models
{
    /// <summary>
    /// Used to insert ship events to Table Storage.
    /// </summary>
    public class ShipEventEntity : TableEntity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ShipEventEntity(string ship, string dateTime)
        {
            PartitionKey = ship;
            RowKey = dateTime;
        }
        
        /// <summary>
        /// Type of event.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Device received in the event.
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Description received in the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product received in the event.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Amount received in the event.
        /// </summary>
        public int? Amount { get; set; }
    }
}