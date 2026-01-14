using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Warehouse.Persistence.MsSql;
using Warehouse.Model;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace O7Y5MK_HSZF_2024251.Tests
{
    [TestFixture]
    public class XMLDataLoaderTests
    {
        AppDbContext context;
        XmlDataLoader xmlLoader;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            context = new AppDbContext(options);
            xmlLoader = new XmlDataLoader(context);
            
            var xmlContent = @"
                <Root>
                    <PirateShip>
                        <Id>1</Id>
                        <Name>Black Pearl</Name>
                        <CaptainName>Jack Sparrow</CaptainName>
                        <Capacity>5000</Capacity>
                        <Shipments>
                            <Shipment>
                                <Id>1</Id>
                                <PirateShipId>1</PirateShipId>
                                <Date>2024-10-01</Date>
                                <Cargos>
                                    <Cargo>
                                        <Id>1</Id>
                                        <ShipmentId>1</ShipmentId>
                                        <Type>Food</Type>
                                        <Quantity>100</Quantity>
                                        <Value>5000.0</Value>
                                    </Cargo>
                                </Cargos>
                            </Shipment>
                        </Shipments>
                    </PirateShip>
                </Root>";

            File.WriteAllText("testData.xml", xmlContent);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
            if (File.Exists("testData.xml"))
            {
                File.Delete("testData.xml");
            }
        }
        
        [Test]
        public void XMLDataLoader_ShouldLoadPirateShipsCorrectly()
        {
            // Act
            xmlLoader.SeedData("testData.xml");

            // Assert
            var pirateShip = context.PirateShips.FirstOrDefault();
            Assert.That(pirateShip, Is.Not.Null);
            Assert.That(pirateShip.Name, Is.EqualTo("Black Pearl"));
            Assert.That(pirateShip.CaptainName, Is.EqualTo("Jack Sparrow"));
            Assert.That(pirateShip.Capacity, Is.EqualTo(5000));
        }
        
        [Test]
        public void XMLDataLoader_ShouldLoadShipmentsAndCargosCorrectly()
        {
            // Act
            xmlLoader.SeedData("testData.xml");

            // Assert
            var shipment = context.Shipments.Include(s => s.Cargos).FirstOrDefault();
            Assert.That(shipment, Is.Not.Null);
            Assert.That(shipment.Date, Is.EqualTo(new DateTime(2024, 10, 1)));

            var cargo = shipment.Cargos.FirstOrDefault();
            Assert.That(cargo, Is.Not.Null);
            Assert.That(cargo.Type, Is.EqualTo("Food"));
            Assert.That(cargo.Quantity, Is.EqualTo(100));
            Assert.That(cargo.Value, Is.EqualTo(5000.0m));
        }
    }
}
