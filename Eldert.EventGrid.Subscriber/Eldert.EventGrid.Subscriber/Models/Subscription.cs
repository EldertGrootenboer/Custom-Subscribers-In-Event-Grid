namespace Eldert.EventGrid.Subscriber.Models
{
    /// <summary>
    /// Defines a subscription with its filters.
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// Name of the subscription.
        /// </summary>
        public string Name;

        /// <summary>
        /// Filter which will look at the start of the subscription's subject.
        /// </summary>
        public string PrefixFilter;

        /// <summary>
        /// Filter which will look at the end of the subscription's subject.
        /// </summary>
        public string SuffixFilter;
    }
}