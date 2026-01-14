    using NUnit.Framework;
    using Microsoft.EntityFrameworkCore;
    using Warehouse.Model;
    using Warehouse.Persistence.MsSql;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Warehouse.Application;

    namespace O7Y5MK_HSZF_2024251.Tests
    {
        [TestFixture]
        public class ReportServiceTests
        {
            AppDbContext context;
            ReportService reportService;
            PirateShipDataProvider pirateShipDataProvider;

            [SetUp]
            public void Setup()
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("TestDb") 
                    .Options;

                context = new AppDbContext(options);
                pirateShipDataProvider = new PirateShipDataProvider(context);
                reportService = new ReportService(pirateShipDataProvider);

                // Létrehozzuk a tesztadatokat
                var pirateShip = new PirateShip
                {
                    Name = "Black Pearl",
                    CaptainName = "Jack Sparrow",
                    Capacity = 5000,
                    Shipments = new[]
                    {
                        new Shipment
                        {
                            Date = DateTime.Now.AddDays(-1),
                            Cargos = new[]
                            {
                                new Cargo { Type = "Food", Quantity = 100, Value = 5000 },
                                new Cargo { Type = "Weapons", Quantity = 50, Value = 2000 }
                            }
                        }
                    }
                };
                context.PirateShips.Add(pirateShip);
                context.SaveChanges();
            }

            [TearDown]
            public void TearDown()
            {
                context.Database.EnsureDeleted();
                context.Dispose();
                
                var reportDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Black Pearl");
                if (Directory.Exists(reportDirectory))
                {
                    Directory.Delete(reportDirectory, true);
                }
            }

            [Test]
            public void GenerateShipmentReport_ShouldCreateShipmentReportXml()
            {
                // Act
                reportService.GenerateShipmentReport(1);  

                // Assert
                var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Black Pearl", "ShipmentReport.xml");
                Assert.That(File.Exists(reportPath), Is.True); 

                var xmlDoc = XDocument.Load(reportPath);
                var rootElement = xmlDoc.Root;
                Assert.That(rootElement?.Name.LocalName, Is.EqualTo("PirateShip")); 
                Assert.That(rootElement?.Element("Shipments")?.Elements("Shipment").Any(), Is.True);
            }

            [Test]
            public void GenerateCapacityUtilizationReport_ShouldCreateCapacityUtilizationReportXml()
            {
                // Act
                reportService.GenerateCapacityUtilizationReport(1);

                // Assert
                var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Black Pearl", "CapacityUtilizationReport.xml");
                Assert.That(File.Exists(reportPath), Is.True);

                var xmlDoc = XDocument.Load(reportPath);
                var rootElement = xmlDoc.Root;
                Assert.That(rootElement?.Name.LocalName, Is.EqualTo("PirateShip"));
                Assert.That(rootElement?.Element("CapacityUtilization")?.Attribute("UtilizationPercent")?.Value, Is.Not.Null);
            }

            [Test]
            public void GenerateReportWithReflection_ShouldCreateReflectionReportXml()
            {
                // Act
                var pirateShip = context.PirateShips.First();
                reportService.GenerateReportWithReflection(pirateShip, "PirateShipReflectionReport");

                // Assert
                var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "PirateShip", "PirateShipReflectionReport.xml");
                Assert.That(File.Exists(reportPath), Is.True);

                var xmlDoc = XDocument.Load(reportPath);
                var rootElement = xmlDoc.Root;
                Assert.That(rootElement?.Name.LocalName, Is.EqualTo("PirateShip"));
                Assert.That(rootElement?.Element("Name")?.Value, Is.EqualTo("Black Pearl"));
                Assert.That(rootElement?.Element("CaptainName")?.Value, Is.EqualTo("Jack Sparrow"));
            }
        }
    }
