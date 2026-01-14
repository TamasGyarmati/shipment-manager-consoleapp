using System;
using System.Xml;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Persistence.MsSql;

public class XmlDataLoader : IDataLoader
{
     readonly AppDbContext context;
     
    public XmlDataLoader(AppDbContext context)
    {
        this.context = context;
    }
    
    public void SeedData(string xmlFilePath)
    {
        try
        {
            var xmlDocument = XDocument.Load(xmlFilePath);

            var pirateShips = xmlDocument.Descendants("PirateShip").Select(pirateShipElement =>
            {
                var shipName = (string)pirateShipElement.Element("Name");
                
                var existingPirateShip = context.PirateShips
                    .Include(p => p.Shipments)
                    .ThenInclude(s => s.Cargos)
                    .FirstOrDefault(p => p.Name == shipName);

                if (existingPirateShip != null) 
                {
                    return existingPirateShip; 
                }
                
                var pirateShip = new PirateShip
                {
                    Name = shipName,
                    CaptainName = (string)pirateShipElement.Element("CaptainName"),
                    Capacity = (int?)pirateShipElement.Element("Capacity") ?? 0,
                    Shipments = new List<Shipment>() 
                };
                
                foreach (var shipmentElement in pirateShipElement.Descendants("Shipment"))
                {
                    var shipmentDate = (DateTime?)shipmentElement.Element("Date");
                    
                    var existingShipment = context.Shipments
                        .FirstOrDefault(s => s.PirateShipId == pirateShip.Id && s.Date == shipmentDate);
                    
                    if (existingShipment != null)
                    {
                        pirateShip.Shipments.Add(existingShipment);
                        //continue;
                    }
                    
                    var shipment = new Shipment 
                    {
                        PirateShip = pirateShip,
                        Date = shipmentDate ?? throw new Exception("ShipmentDate is missing or empty in XML."),
                        Cargos = new List<Cargo>()
                    };
                    
                    foreach (var cargoElement in shipmentElement.Descendants("Cargo"))
                    {
                        
                        var cargoType = (string)cargoElement.Element("Type");
                        var cargoQuantity = (int)cargoElement.Element("Quantity");
                        
                        var existingCargo = context.Cargos
                            .FirstOrDefault(c => c.ShipmentId == shipment.Id && c.Type == cargoType && c.Quantity == cargoQuantity);

                        
                        if (existingCargo == null)
                        {
                            var cargo = new Cargo 
                            {
                                Shipment = shipment,
                                Type = cargoType,
                                Quantity = cargoQuantity,
                                Value = (decimal)cargoElement.Element("Value")
                            };
                            shipment.Cargos.Add(cargo);
                        }
                        else 
                        {
                            shipment.Cargos.Add(existingCargo);
                        }
                    }  
                    pirateShip.Shipments.Add(shipment); 
                }
                return pirateShip; 
            }).ToList();
            
            context.PirateShips.AddRange(pirateShips.Where(p => context.PirateShips.All(existing => existing.Name != p.Name)));
            
            context.SaveChanges();
        }
        catch (FileNotFoundException ex) 
        {
            Console.WriteLine($"Error: File not found - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred - {ex.Message}");
        }
    }
}