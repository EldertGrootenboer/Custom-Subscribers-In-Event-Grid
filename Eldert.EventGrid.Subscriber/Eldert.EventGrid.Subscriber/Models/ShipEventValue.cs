namespace Eldert.EventGrid.Subscriber.Models
{
    /// <summary>
    /// Class used to receive ship event values.
    /// </summary>
    public class ShipEventValue
    {
        /// <summary>
        /// Time when event was created.
        /// </summary>
        public string EventTime;

        /// <summary>
        /// Data of the event.
        /// </summary>
        public ShipEventData Data;
    }
}