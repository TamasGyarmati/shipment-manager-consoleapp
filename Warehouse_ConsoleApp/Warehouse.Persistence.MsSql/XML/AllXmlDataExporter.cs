using System.Xml.Linq;

namespace Warehouse.Persistence.MsSql;

public class AllXmlDataExporter
{
    readonly AppDbContext context;

    public AllXmlDataExporter(AppDbContext context)
    {
        this.context = context;
    }
    
    public void ExportDatabaseToXml()
        {
            var pirateShipElementName = XName.Get("PirateShip");
            var idAttributeName = XName.Get("Id");
            var nameAttributeName = XName.Get("Name");
            var captainNameAttributeName = XName.Get("CaptainName");
            var capacityAttributeName = XName.Get("Capacity");

            var shipmentElementName = XName.Get("Shipment");
            var dateAttributeName = XName.Get("Date");
            var pirateShipIdAttributeName = XName.Get("PirateShipId");

            var cargoElementName = XName.Get("Cargo");
            var shipmentIdAttributeName = XName.Get("ShipmentId");
            var typeAttributeName = XName.Get("Type");
            var quantityAttributeName = XName.Get("Quantity");
            var valueAttributeName = XName.Get("Value");

            var xmlDoc = new XDocument(
                new XElement("Database",
                    new XElement("PirateShips",
                        context.PirateShips.Select(ship =>
                            new XElement(pirateShipElementName,
                                new XAttribute(idAttributeName, ship.Id),
                                new XAttribute(nameAttributeName, ship.Name),
                                new XAttribute(captainNameAttributeName, ship.CaptainName),
                                new XAttribute(capacityAttributeName, ship.Capacity)
                            )
                        )
                    ),
                    new XElement("Shipments",
                        context.Shipments.Select(shipment =>
                            new XElement(shipmentElementName,
                                new XAttribute(idAttributeName, shipment.Id),
                                new XAttribute(dateAttributeName, shipment.Date),
                                new XAttribute(pirateShipIdAttributeName, shipment.PirateShipId)
                            )
                        )
                    ),
                    new XElement("Cargos",
                        context.Cargos.Select(cargo =>
                            new XElement(cargoElementName,
                                new XAttribute(idAttributeName, cargo.Id),
                                new XAttribute(shipmentIdAttributeName, cargo.ShipmentId),
                                new XAttribute(typeAttributeName, cargo.Type),
                                new XAttribute(quantityAttributeName, cargo.Quantity),
                                new XAttribute(valueAttributeName, cargo.Value)
                            )
                        )
                    )
                )
            );

            string reportPath = Path.Combine("Reports", "DatabaseExport.xml");
            Directory.CreateDirectory("Reports");
            xmlDoc.Save(reportPath);
        }
}