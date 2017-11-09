using System;
using NUnit.Framework;
using Raspberry;

namespace Tests.Raspberry.System
{
    [TestFixture]
    public class TestOldStyleRevisionParsing
    {
        [TestCase]
        public void CanParseBRev1RevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual(Model.BRev1, model );
                Assert.AreEqual(new Version(1, 0), version );
                Assert.AreEqual( 256, memSize );
                Assert.AreEqual(Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x0002, setBoardParams );
            Board.ParseRevisionCode( 0x0003, setBoardParams );
        }

        [TestCase]
        public void CanParseBRev2RevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.BRev2, model);
                Assert.AreEqual( new Version( 2, 0 ), version);
                Assert.AreEqual( 256, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x0004, setBoardParams );
            Board.ParseRevisionCode( 0x0005, setBoardParams );
            Board.ParseRevisionCode( 0x0006, setBoardParams );
        }

        public void CanParseARev2RevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.BRev1, model);
                Assert.AreEqual( new Version( 2, 0 ), version );
                Assert.AreEqual( 256, memSize );
                Assert.AreEqual( Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x0007, setBoardParams );
            Board.ParseRevisionCode( 0x0008, setBoardParams );
            Board.ParseRevisionCode( 0x0009, setBoardParams );
        }

        [TestCase]
        public void CanParseBRev2512MbRevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.BRev2,model );
                Assert.AreEqual( new Version( 2, 0 ), version);
                Assert.AreEqual( 512, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x000d, setBoardParams );
            Board.ParseRevisionCode( 0x000e, setBoardParams );
            Board.ParseRevisionCode( 0x000f, setBoardParams );
        }

        [TestCase]
        public void CanParseBPlusRevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.BPlus, model);
                Assert.AreEqual( new Version( 1, 0 ), version);
                Assert.AreEqual( 512, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x0010, setBoardParams );
        }

        [TestCase]
        public void CanParseComputerModuleRevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.ComputeModule, model);
                Assert.AreEqual( new Version( 1, 0 ), version);
                Assert.AreEqual( 512, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x0011, setBoardParams );
            Board.ParseRevisionCode( 0x0014, setBoardParams );
        }

        [TestCase]
        public void CanParseAPlusRevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.APlus, model);
                Assert.AreEqual( new Version( 1, 1 ), version);
                Assert.AreEqual( 256, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x0012, setBoardParams );
        }

        [TestCase]
        public void CanParseBPlusV12RevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.BPlus, model);
                Assert.AreEqual( new Version( 1, 2 ), version);
                Assert.AreEqual( 512, memSize);
                Assert.AreEqual( Processor.Bcm2835, proc);
            };
            Board.ParseRevisionCode( 0x0013, setBoardParams );
        }

        [TestCase]
        public void CanParseAPlus512MbRevisionCode()
        {
            Action<Model, Version, int, Processor> setBoardParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.APlus, model );
                Assert.AreEqual( new Version( 1, 1 ), version );
                Assert.AreEqual( 512, memSize );
                Assert.AreEqual( Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x0015, setBoardParams );
        }
    }
}
