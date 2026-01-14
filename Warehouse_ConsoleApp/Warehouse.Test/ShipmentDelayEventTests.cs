using NUnit.Framework;
using NUnit.Framework.Legacy;
using Warehouse.Model;
using Warehouse.Application;
using System;

namespace O7Y5MK_HSZF_2024251.Tests
{
    [TestFixture]
    public class ShipmentDelayEventTests
    {
        bool eventTriggered;
        int eventShipmentId;
        DateTime eventDate;

        [SetUp]
        public void Setup()
        {
            eventTriggered = false;
            eventShipmentId = 0;
            eventDate = default;
        }

        [Test]
        public void ShipmentDelayedEvent_ShouldTrigger_WhenShipmentIsDelayed()
        {
            // Arrange
            var shipment = new Shipment
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-1) 
            };

            // Eseményfigyelő beállítása
            shipment.ShipmentDelayed += OnShipmentDelayed;

            // Act
            shipment.CheckForDelay(); 

            // Assert
            ClassicAssert.IsTrue(eventTriggered, "The delay event should be triggered.");
            ClassicAssert.AreEqual(1, eventShipmentId, "The delayed shipment ID should match.");
            ClassicAssert.AreEqual(shipment.Date, eventDate, "The delayed shipment date should match the event date.");
        }

        // Eseménykezelő
        void OnShipmentDelayed(object sender, ShipmentEventArgs e)
        {
            eventTriggered = true;
            eventShipmentId = e.ShipmentId;
            eventDate = e.Date;
        }
    }
}