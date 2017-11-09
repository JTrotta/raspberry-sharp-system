
namespace Raspberry
{
    /// <summary>
    /// Contains information about an R-Pi core.
    /// </summary>
    public class Core
    {
        public Core( int procNumber )
        {
            ProcessorNumber = procNumber;
        }

        /// <summary>
        /// The number of this core.
        /// </summary>
        public int ProcessorNumber { get; }

        /// <summary>
        /// The model name of this core.
        /// </summary>
        public string ModelName { get; internal set; }

        /// <summary>
        /// Bogus MIPS estimate for this core.
        /// </summary>
        public double BogoMips { get; internal set; }

        /// <summary>
        /// Features of this core.
        /// </summary>
        public string Features { get; internal set; }

        /// <summary>
        /// Converts core information to string format.
        /// </summary>
        public override string ToString()
        {
            return $"Core:\t\t{ProcessorNumber}\n" +
                   $"Model name:\t{ModelName}\n" +
                   $"BogoMIPS:\t{BogoMips}\n" +
                   $"Features:\t{Features}\n";
        }

        internal void SetState( string key, string val )
        {
            switch ( key )
            {
                case "MODEL NAME":
                    ModelName = val;
                    break;

                case "BOGOMIPS":
                    double bmips;
                    if (double.TryParse(val, out bmips))
                    {
                        BogoMips = bmips;
                    }
                    break;

                case "FEATURES":
                    Features = val;
                    break;

                case "CPU IMPLEMENTER":
                    break;

            }
        }
    }
}
