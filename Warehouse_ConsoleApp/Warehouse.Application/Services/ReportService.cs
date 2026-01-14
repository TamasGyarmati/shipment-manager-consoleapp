using System.Xml.Linq;
using System.Reflection;
using Warehouse.Model;
using Microsoft.EntityFrameworkCore;
using Warehouse.Persistence.MsSql;
using Warehouse.Persistence.MsSql.DataProviders;

namespace Warehouse.Application
{
    public class ReportService
    {
        PirateShipDataProvider pirateShipDataProvider;

        public ReportService(PirateShipDataProvider pirateShipDataProvider)
        {
            this.pirateShipDataProvider = pirateShipDataProvider;
        }
        
        public void GenerateShipmentReport(int pirateShipId)
        {
            var ship = pirateShipDataProvider.PirateShipReportGenerator(pirateShipId); 

            if (ship != null && ship.Shipments.Any()) 
            {
                var xmlDoc = new XDocument(
                    new XElement("PirateShip",
                        new XAttribute("Name", ship.Name),
                        new XElement("Shipments", 
                                ship.Shipments
                                .Where(s => s.Cargos.Any()) 
                                .Select(shipment =>
                                    new XElement("Shipment",
                                        new XAttribute("Date", shipment.Date), 
                                        new XAttribute("CargoCount", shipment.Cargos.Count), 
                                        new XAttribute("TotalValue", shipment.Cargos.Sum(c => c.Value))
                                    )
                                )
                        )
                    )
                );
                var reportDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Reports", ship.Name);
                Directory.CreateDirectory(reportDirectory); 
                
                var reportPath = Path.Combine(reportDirectory, "ShipmentReport.xml");
                xmlDoc.Save(reportPath); 
            }
        }
        
        public void GenerateCapacityUtilizationReport(int pirateShipId)
        {
            var ship = pirateShipDataProvider.PirateShipReportGenerator(pirateShipId);
            
            var totalCargoWeight = ship.Shipments
                .Where(s => s.Cargos != null)
                .Sum(s => s.Cargos.Sum(c => c.Quantity));
            
            double capacityUtilization = totalCargoWeight > 0
                ? (double)totalCargoWeight / ship.Capacity * 100
                : 0;
            
            var xmlDoc = new XDocument(
                new XElement("PirateShip",
                    new XAttribute("Name", ship.Name),
                    new XElement("CapacityUtilization",
                        new XAttribute("TotalCargo", totalCargoWeight),
                        new XAttribute("Capacity", ship.Capacity),
                        new XAttribute("UtilizationPercent", capacityUtilization))));
            
            var reportDirectory = Path.Combine("Reports", ship.Name);
            Directory.CreateDirectory(reportDirectory); 
            
            var reportPath = Path.Combine(reportDirectory, "CapacityUtilizationReport.xml");
            xmlDoc.Save(reportPath); 
        }
        
        public void GenerateReportWithReflection<T>(T obj, string reportName)
        {
            var objType = obj.GetType();
            var properties = objType.GetProperties();
            
            var xmlDoc = new XDocument(
                new XElement(objType.Name, 
                    properties              
                        .Where(p => !typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string)) 
                        .Select(p => new XElement(p.Name, p.GetValue(obj) ?? "null"))));
                        
            var reportDirectory = Path.Combine("Reports", objType.Name);  
            Directory.CreateDirectory(reportDirectory);
            
            var reportPath = Path.Combine(reportDirectory, $"{reportName}.xml");
            xmlDoc.Save(reportPath);
        }
    }
}
