// Copyright (c) Zorgdoc.  All Rights Reserved.  Licensed under the AGPLv3.

namespace MedMij.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml;
    using global::Xunit;

    public class TestZorgaanbiedersCollection
    {
        [Theory]
        [InlineData(TestData.ZorgaanbiedersCollectionExampleXML)]
        [InlineData(TestData.ZorgaanbiedersCollectionEmptyExampleXML)]
        public void ZorgaanbiedersCollectionParseOK(string xmlData)
        {
            ZorgaanbiedersCollection.FromXMLData(xmlData);
        }

        [Theory]
        [InlineData(TestData.ZorgaanbiedersCollectionExampleXML)]
        [InlineData(TestData.ZorgaanbiedersCollectionEmptyExampleXML)]
        public void ZorgaanbiedersCollectionIsIterable(string xmlData)
        {
            var col = ZorgaanbiedersCollection.FromXMLData(xmlData);
            foreach (var c in col)
            {
                System.Console.WriteLine(c.Naam);
            }
        }

        [Theory]
        [InlineData(TestData.InvalidXML)]
        public void ZorgaanbiedersCollectionInvalidXML(string xmlData)
        {
            Assert.ThrowsAny<XmlException>(() => ZorgaanbiedersCollection.FromXMLData(xmlData));
        }

        [Theory]
        [InlineData(TestData.ZorgaanbiedersCollectionXSDFail1)]
        [InlineData(TestData.ZorgaanbiedersCollectionXSDFail2)]
        public void ZorgaanbiedersCollectionXSDFail(string xmlData)
        {
            Assert.ThrowsAny<System.Xml.Schema.XmlSchemaException>(
                () => ZorgaanbiedersCollection.FromXMLData(xmlData));
        }

        [Theory]
        [InlineData(TestData.ZorgaanbiedersCollectionURL)]
        public async Task<ZorgaanbiedersCollection> ZorgaanbiedersCollectionDownload(string uri)
        {
            var httpClientFactory = new StringHttpClienFactoryMock(TestData.ZorgaanbiedersCollectionExampleXML);
            return await ZorgaanbiedersCollection.FromURLAsync(new Uri(uri), httpClientFactory);
        }

        [Theory]
        [InlineData("umcharderwijk@medmij")]
        [InlineData("radiologencentraalflevoland@medmij")]
        public void ZorgaanbiedersCollectionContains(string name)
        {
            var zorgaanbieders = ZorgaanbiedersCollection.FromXMLData(TestData.ZorgaanbiedersCollectionExampleXML);
            Assert.NotNull(zorgaanbieders.GetByName(name));
        }

        [Theory]
        [InlineData(TestData.ZorgaanbiedersCollectionExampleXML, "umcharderwijk")]
        [InlineData(TestData.ZorgaanbiedersCollectionExampleXML, "UMCHarderwijk@medmij")]
        [InlineData(TestData.ZorgaanbiedersCollectionExampleXML, "")]
        [InlineData(TestData.ZorgaanbiedersCollectionEmptyExampleXML, "")]
        [InlineData(TestData.ZorgaanbiedersCollectionEmptyExampleXML, "umcharderwijk@medmij")]
        public void ZorgaanbiedersCollectionNotContains(string xml, string name)
        {
            var zorgaanbieders = ZorgaanbiedersCollection.FromXMLData(xml);
            Assert.Throws<KeyNotFoundException>(() => zorgaanbieders.GetByName(name));
        }

    }
}
