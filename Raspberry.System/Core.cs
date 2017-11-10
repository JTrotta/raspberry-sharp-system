
using System.Globalization;

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
        /// Gets the APU implementer code
        /// </summary>
        public int CpuImplementer { get; internal set; }

        /// <summary>
        /// Gets the CPU architecture code.
        /// </summary>
        public int CpuArchitecture { get; internal set; }

        /// <summary>
        /// Gets the CPU variant code.
        /// </summary>
        public int CpuVariant { get; internal set; }

        /// <summary>
        /// Gets the CPU part code.
        /// </summary>
        public int CpuPart { get; internal set; }

        /// <summary>
        /// Gets the CPU revision code.
        /// </summary>
        public int CpuRevision { get; internal set; }

        /// <summary>
        /// Converts core information to string format.
        /// </summary>
        public override string ToString()
        {
            return $"Core:\t\t{ProcessorNumber}\n" +
                   $"  -> Model:\t{ModelName}\n" +
                   $"  -> BogoMIPS:\t{BogoMips}\n" +
                   $"  -> Features:\t{Features}\n" +
                   $"  -> CPU:\timplementer=0x{CpuImplementer:X}, arch={CpuArchitecture}, variant=0x{CpuVariant:X}, " +
                   $"part=0x{CpuPart:X}, rev={CpuRevision}\n";
        }

        internal void SetState( string key, string val )
        {
            switch ( key )
            {
                case "model name":
                    ModelName = val;
                    break;

                case "bogomips":
                    double bmips;
                    if ( double.TryParse( val, out bmips ) )
                    {
                        BogoMips = bmips;
                    }
                    break;

                case "features":
                    Features = val;
                    break;

                case "cpu implementer":
                    CpuImplementer = ParseInt( val, NumberStyles.HexNumber );
                    break;

                case "cpu architecture":
                    CpuArchitecture = ParseInt( val );
                    break;

                case "cpu variant":
                    CpuVariant = ParseInt( val, NumberStyles.HexNumber );
                    break;

                case "cpu part":
                    CpuPart = ParseInt( val, NumberStyles.HexNumber );
                    break;

                case "cpu revision":
                    CpuRevision = ParseInt( val );
                    break;
            }
        }

        private int ParseInt( string val, NumberStyles numberStyle = NumberStyles.Integer )
        {
            int.TryParse( val, numberStyle, null, out int ret );
            return ret;
        }
    }
}
