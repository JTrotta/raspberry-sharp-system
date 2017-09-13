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
        /// Processor is a BCM2708.
        /// </summary>
        Bcm2708,

        /// <summary>
        /// Processor is a BCM2709.
        /// </summary>
        Bcm2709,

        /// <summary>
        /// Processor is BCM2835
        /// </summary>
        BCM2835 // <- added this one JJ FIX per RB3/CM3
    }
}