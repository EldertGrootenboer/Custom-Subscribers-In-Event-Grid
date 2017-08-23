namespace Eldert.EventGrid.Subscriber.Models
{
    /// <summary>
    /// Data which can be sent with various ship events.
    /// </summary>
    public class ShipEventData
    {
        /// <summary>
        /// Name of the ship.
        /// </summary>
        public string Ship { get; set; }

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