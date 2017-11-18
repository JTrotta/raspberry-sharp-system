namespace Raspberry
{
    /// <summary>
    /// The Raspberry Pi processor.
    /// </summary>
    public enum Processor
    {
        /// <summary>
        /// Processor is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Processor is a BCM2835. (Used to be referred to as BCM2708.)
        /// </summary>
        Bcm2835,

        /// <summary>
        /// Processor is a BCM2836. (Used to be referred to as BCM2709.)
        /// </summary>
        Bcm2836,

        /// <summary>
        /// Processor is BCM2837
        /// </summary>
        Bcm2837
    }
}