#region References

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

namespace Raspberry
{
    /// <summary>
    /// Represents the Raspberry Pi mainboard.
    /// </summary>
    /// <remarks>
    /// Version and revisions are based on <see cref="https://www.raspberrypi.org/documentation/hardware/raspberrypi/revision-codes/README.md"/>.
    /// <see cref="http://www.raspberrypi-spy.co.uk/2012/09/checking-your-raspberry-pi-board-version/"/> for information.
    /// NOTES on V3.1.2:
    /// <list type="bullet">
    /// <item>IsOverClocked has been removed. This used the "warranty" flag (i.e. void if overclocked) but this policy was changed.</item>
    /// <item>Firmware has been changed to RevisionCode.</item>
    /// <item>Details about the cores are also loaded.</item>
    /// </list>
    /// </remarks>
    public class Board
    {

        #region Fields

        private static Board board;
        private uint revisionCode;
        private IList<Core> cores = new List<Core>();

        #endregion

        #region Instance Management

        private Board()
        {
            LoadBoard();            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current mainboard configuration.
        /// </summary>
        public static Board Current => board ?? ( board = new Board() );

        /// <summary>
        /// Gets a value indicating whether this instance is a Raspberry Pi.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a Raspberry Pi; otherwise, <c>false</c>.
        /// </value>
        public bool IsRaspberryPi { get; private set; }

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>
        /// The processor.
        /// </value>
        public Processor Processor { get; private set; }

        /// <summary>
        /// Gets the board revision code. This is the number from cpuinfo containing many details about the board.
        /// </summary>
        public uint RevisionCode
        {
            get => revisionCode;
            private set
            {
                revisionCode = value;
                ParseRevisionCode( value, SetBoardParams );
                ConnectorPinout = LoadConnectorPinout();
            }
        }

        /// <summary>
        /// Gets the board revision, i.e. 1.1 etc.
        /// </summary>
        public Version Revision { get; private set; }

        /// <summary>
        /// Gets the memory size in MB.
        /// </summary>
        public int MemorySize { get; private set; }

        /// <summary>
        /// Gets the serial number.
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public Model Model { get; private set; }

        /// <summary>
        /// Gets the connector revision.
        /// </summary>
        /// <value>
        /// The connector revision.
        /// </value>
        /// <remarks>See <see cref="http://raspi.tv/2014/rpi-gpio-quick-reference-updated-for-raspberry-pi-b"/> for more information.</remarks>
        public ConnectorPinout ConnectorPinout { get; private set; }

        /// <summary>
        /// Gets the list of cores on this board.
        /// </summary>
        public IList<Core> Cores => cores;

        /// <summary>
        /// Pretty prints the board info.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder( $"Board:\t\t{Model.GetDisplayName()}\n" +
                   $"Revision code:\t0x{RevisionCode:X}\n" +
                   $"Model:\t\t{Model}\n" +
                   $"Revision:\t{Revision}\n" +
                   $"Processor:\t{Processor}\n" +
                   $"Memory size:\t{MemorySize} MB\n" +
                   $"Connector:\t{ConnectorPinout}\n" +
                   $"Serial#:\t{SerialNumber}\n" );

            foreach ( var core in Cores ) sb.Append( "\n" + core );

            return sb.ToString();
        }

        #endregion

        #region Private Helpers

        private void LoadBoard()
        {
            IsRaspberryPi = true;
            string[] cpuInfo;
            try
            {
                const string filePath = "/proc/cpuinfo";
                Tracing.TraceInfo( $"Loading board information from '{filePath}'." );
                cpuInfo = File.ReadAllLines( filePath );
            }
            catch ( Exception ex )
            {
                Tracing.TraceError( "Unable to read cpuinfo - are you sure this is a Pi? " + ex.Message );
                IsRaspberryPi = false;
                return;
            }

            Core currentCore = null;
            foreach ( var l in cpuInfo )
            {
                var separator = l.IndexOf( ':' );

                if ( !string.IsNullOrWhiteSpace( l ) && separator > 0 )
                {
                    var key = l.Substring( 0, separator ).Trim().ToLower();
                    var val = l.Substring( separator + 1 ).Trim();
                    switch ( key )
                    {
                        case "revision":
                            uint revision;
                            if ( uint.TryParse( val, NumberStyles.HexNumber, null, out revision ) )
                            {
                                RevisionCode = revision;
                            }
                            else
                            {
                                Tracing.TraceError( $"Unable to parse revision number string - {val}." );
                            }
                            break;

                        case "serial":
                            SerialNumber = val;
                            break;

                        case "processor":
                            int coreNum;
                            if ( int.TryParse( val, out coreNum ) )
                            {
                                currentCore = new Core( coreNum );
                                cores.Add( currentCore );
                            }
                            else
                            {
                                currentCore = null;
                            }
                            break;

                        default:
                            currentCore?.SetState( key, val );
                            break;
                    }
                }
            }
            Tracing.TraceInfo( "Board information loading complete." );
        }

        /// <summary>
        /// Parses the Pi's revision code.
        /// </summary>
        public static void ParseRevisionCode( uint revisionCode, Action<Model, Version, int, Processor> setBoardParams )
        {
            var oldStyle = ( revisionCode & 0x00800000 ) == 0; // Bit 24
            if ( oldStyle )
            {
                ParseOldRevisionCode( revisionCode, setBoardParams );
                return;
            }

            var memSizeBits = ( revisionCode & 0x00700000 ) >> 20; // Bits 21-23
            var memSize = 256;
            switch ( memSizeBits )
            {
                case 1:
                    memSize = 512;
                    break;
                case 2:
                    memSize = 1024;
                    break;
            }

            var procBits = ( revisionCode & 0x000F000 ) >> 12; // Bits 13-16
            var proc = Processor.Bcm2835;
            switch ( procBits )
            {
                case 1:
                    proc = Processor.Bcm2836;
                    break;
                case 2:
                    proc = Processor.Bcm2837;
                    break;
            }

            var typeBits = ( revisionCode & 0x0000FF0 ) >> 4; // Bits 5-12
            var type = Model.Unknown;
            switch ( typeBits )
            {
                case 0x00:
                    type = Model.A;
                    break;
                case 0x02:
                    type = Model.APlus;
                    break;
                case 0x03:
                    type = Model.BPlus;
                    break;
                case 0x04:
                    type = Model.B2;
                    break;
                case 0x06:
                    type = Model.ComputeModule;
                    break;
                case 0x08:
                    type = Model.B3;
                    break;
                case 0x09:
                    type = Model.Zero;
                    break;
                case 0x0a:
                    type = Model.ComputeModule3;
                    break;
                case 0x0c:
                    type = Model.ZeroW;
                    break;
            }

            var revisionBits = revisionCode & 0x000000F; // Bottom 4 bits
            var revision = new Version( 1, (int)revisionBits );
            setBoardParams( type, revision, memSize, proc );
        }

        private static void ParseOldRevisionCode( uint revisionCode, Action<Model, Version, int, Processor> setBoardParams )
        {
            switch ( revisionCode & 0xFFFF )
            {
                case 0x2:
                case 0x3:
                    setBoardParams( Model.BRev1, new Version( 1, 0 ), 256, Processor.Bcm2835 );
                    break;

                case 0x4:
                case 0x5:
                case 0x6:
                    setBoardParams( Model.BRev2, new Version( 2, 0 ), 256, Processor.Bcm2835 );
                    break;

                case 0x7:
                case 0x8:
                case 0x9:
                    setBoardParams( Model.A, new Version( 2, 0 ), 256, Processor.Bcm2835 );
                    break;

                case 0xd:
                case 0xe:
                case 0xf:
                    setBoardParams( Model.BRev2, new Version( 2, 0 ), 512, Processor.Bcm2835 );
                    break;

                case 0x10:
                    setBoardParams( Model.BPlus, new Version( 1, 0 ), 512, Processor.Bcm2835 );
                    break;

                case 0x11:
                case 0x14:
                    setBoardParams( Model.ComputeModule, new Version( 1, 0 ), 512, Processor.Bcm2835 );
                    break;

                case 0x12:
                    setBoardParams( Model.APlus, new Version( 1, 1 ), 256, Processor.Bcm2835 );
                    break;

                case 0x13:
                    setBoardParams( Model.BPlus, new Version( 1, 2 ), 512, Processor.Bcm2835 );
                    break;

                case 0x15:
                    setBoardParams( Model.APlus, new Version( 1, 1 ), 512, Processor.Bcm2835 ); // Ambiguous - could have 256MB.
                    break;

                default:
                    setBoardParams( Model.Unknown, new Version( 0, 0 ), 0, Processor.Bcm2835 );
                    break;
            }
        }

        private void SetBoardParams(Model model, Version revision, int memSize, Processor proc )
        {
            Model = model;
            Revision = revision;
            MemorySize = memSize;
            Processor = proc;
        }

        private ConnectorPinout LoadConnectorPinout()
        {
            switch (Model)
            {
                case Model.BRev1:
                    return ConnectorPinout.Rev1;

                case Model.BRev2:
                case Model.A:
                    return ConnectorPinout.Rev2;

                case Model.BPlus:
                case Model.ComputeModule:
                case Model.APlus:
                case Model.B2:
                case Model.Zero:
                case Model.ZeroW:
                case Model.B3:
                case Model.ComputeModule3:
                    return ConnectorPinout.Plus;

                default:
                    return ConnectorPinout.Unknown;
            }
        }

        #endregion
    }
}
