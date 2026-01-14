using NUnit.Framework;
using NUnit.Framework.Legacy;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using Warehouse.Persistence.MsSql;

namespace O7Y5MK_HSZF_2024251.Tests
{
    [TestFixture]
    public class DatabaseEntityTests
    {
        [Test]
        public void Cargo_Properties_SetCorrectly()
        {
            var cargo = new Cargo
            {
                ShipmentId = 1,
                Type = "Food",
                Quantity = 100,
                Value = 5000
            };

            ClassicAssert.AreEqual(1, cargo.ShipmentId, "ShipmentId is not set correctly.");
            ClassicAssert.AreEqual("Food", cargo.Type, "Type is not set correctly.");
            ClassicAssert.AreEqual(100, cargo.Quantity, "Quantity is not set correctly.");
            ClassicAssert.AreEqual(5000, cargo.Value, "Value is not set correctly.");
        }
        
        [Test]
        public void PirateShip_Properties_SetCorrectly()
        {
            var pirateShip = new PirateShip
            {
                Name = "Black Pearl",
                CaptainName = "Jack Sparrow",
                Capacity = 6000
            };

            ClassicAssert.AreEqual("Black Pearl", pirateShip.Name, "Name is not set correctly.");
            ClassicAssert.AreEqual("Jack Sparrow", pirateShip.CaptainName, "CaptainName is not set correctly.");
            ClassicAssert.AreEqual(6000, pirateShip.Capacity, "Capacity is not set correctly.");
        }
        
        [Test]
        public void Shipment_Properties_SetCorrectly()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(1); 
            var shipment = new Shipment
            {
                Id = 1,
                PirateShipId = 1,
                Date = futureDate 
            };

            // Assert
            Assert.That(shipment.Id, Is.EqualTo(1));
            Assert.That(shipment.PirateShipId, Is.EqualTo(1));
            Assert.That(shipment.Date, Is.EqualTo(futureDate));
            Assert.That(shipment.IsDelayed, Is.False, "IsDelayed property should initially be false.");
        }
    }
}
