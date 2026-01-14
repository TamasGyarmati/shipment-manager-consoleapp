using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using Warehouse.Persistence.MsSql;
using Warehouse.Application;
using System;
using System.Linq;

namespace O7Y5MK_HSZF_2024251.Tests
{
    [TestFixture]
    public class CargoTypeSearchTests
    {
        AppDbContext context;
        ShipmentService shipmentService;
        CargoDataProvider cargoDataProvider;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb") 
                .Options;

            context = new AppDbContext(options);
            cargoDataProvider = new CargoDataProvider(context);
            
            var ship = new PirateShip
            {
                Name = "Black Pearl",
                CaptainName = "Jack Sparrow",
                Capacity = 5000,
                Shipments = new[]
                {
                    new Shipment
                    {
                        Date = DateTime.Now.AddDays(-5),
                        Cargos = new[]
                        {
                            new Cargo { Type = "Gold", Quantity = 100, Value = 10000 },
                            new Cargo { Type = "Rum", Quantity = 50, Value = 5000 }
                        }
                    },
                    new Shipment
                    {
                        Date = DateTime.Now.AddDays(-3),
                        Cargos = new[]
                        {
                            new Cargo { Type = "Gold", Quantity = 200, Value = 20000 },
                            new Cargo { Type = "Weapons", Quantity = 30, Value = 15000 }
                        }
                    }
                }
            };
            context.PirateShips.Add(ship);
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted(); 
            context.Dispose(); 
        }

        [Test]
        public void SearchByCargoType_ShouldReturnCorrectResultsForGold()
        {
            // Act
            var results = cargoDataProvider.SearchByCargoType("Gold");

            // Assert
            Assert.That(results.Count, Is.EqualTo(2)); 
            Assert.That(results.Any(r => r.ShipName == "Black Pearl"), Is.True); 
            
            Assert.That(results[0].ShipName, Is.EqualTo("Black Pearl"));
            Assert.That(results[0].Quantity, Is.EqualTo(100));
            Assert.That(results[1].ShipName, Is.EqualTo("Black Pearl"));
            Assert.That(results[1].Quantity, Is.EqualTo(200));

            // Ezt lehet törölni kéne innen
            foreach (var result in results)
            {
                Console.WriteLine($"Ship: {result.ShipName}, Date: {result.ShipmentDate}, Quantity: {result.Quantity}");
            }
        }
    }
}
