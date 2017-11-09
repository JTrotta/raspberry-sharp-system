
using System;
using NUnit.Framework;
using Raspberry;

namespace Tests.Raspberry.System
{
    [TestFixture]
    public class TestNewStyleRevisionParsing
    {
        [TestCase]
        public void CanParseAPlusRevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( model, Model.APlus );
                Assert.AreEqual( version, new Version( 1, 1 ) );
                Assert.AreEqual( memSize, 512 );
                Assert.AreEqual( proc, Processor.Bcm2835 );
            };
            Board.ParseRevisionCode( 0x900021, setModelParams );
        }

        [TestCase]
        public void CanParseBPlusRevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( model, Model.BPlus );
                Assert.AreEqual( version, new Version( 1, 2 ) );
                Assert.AreEqual( memSize, 512 );
                Assert.AreEqual( proc, Processor.Bcm2835 );
            };
            Board.ParseRevisionCode( 0x900032, setModelParams );
        }

        [TestCase]
        public void CanParseZero12RevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.Zero, model );
                Assert.AreEqual( new Version( 1, 2 ), version );
                Assert.AreEqual( 512, memSize );
                Assert.AreEqual( Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x900092, setModelParams );
        }

        [TestCase]
        public void CanParseZero13RevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.Zero, model );
                Assert.AreEqual( new Version( 1, 3 ), version );
                Assert.AreEqual( 512, memSize );
                Assert.AreEqual( Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x900093, setModelParams );
            Board.ParseRevisionCode( 0x920093, setModelParams );
        }

        [TestCase]
        public void CanParseZeroWRevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.ZeroW, model );
                Assert.AreEqual( new Version( 1, 1 ), version );
                Assert.AreEqual( 512, memSize );
                Assert.AreEqual( Processor.Bcm2835, proc );
            };
            Board.ParseRevisionCode( 0x9000C1, setModelParams );
        }

        [TestCase]
        public void CanParse2B10RevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.B2, model );
                Assert.AreEqual( new Version( 1, 0 ), version );
                Assert.AreEqual( 1024, memSize );
                Assert.AreEqual( Processor.Bcm2836, proc );
            };
            Board.ParseRevisionCode( 0xA01040, setModelParams );
        }

        [TestCase]
        public void CanParse2B11RevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.B2, model );
                Assert.AreEqual( new Version( 1, 1 ), version );
                Assert.AreEqual( 1024, memSize );
                Assert.AreEqual( Processor.Bcm2836, proc );
            };
            Board.ParseRevisionCode( 0xA01041, setModelParams );
            Board.ParseRevisionCode( 0xA21041, setModelParams );
        }

        [TestCase]
        public void CanParse2B12RevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.B2, model );
                Assert.AreEqual( new Version( 1, 2 ), version );
                Assert.AreEqual( 1024, memSize );
                Assert.AreEqual( Processor.Bcm2837, proc );
            };
            Board.ParseRevisionCode( 0xA22042, setModelParams );
        }

        [TestCase]
        public void CanParse3BRevisionCode()
        {
            Action<Model, Version, int, Processor> setModelParams = ( model, version, memSize, proc ) =>
            {
                Assert.AreEqual( Model.B3, model );
                Assert.AreEqual( new Version( 1, 2 ), version );
                Assert.AreEqual( 1024, memSize );
                Assert.AreEqual( Processor.Bcm2837, proc );
            };
            Board.ParseRevisionCode( 0xA02082, setModelParams );
            Board.ParseRevisionCode( 0xA22082, setModelParams );
            Board.ParseRevisionCode( 0xA32082, setModelParams );
        }
    }
}
