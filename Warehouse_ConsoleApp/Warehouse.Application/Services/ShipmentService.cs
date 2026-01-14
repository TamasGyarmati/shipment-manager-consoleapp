using Warehouse.Model;
using Warehouse.Persistence.MsSql.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Persistence.MsSql;

namespace Warehouse.Application;

public class ShipmentService
{
    readonly PirateShipDataProvider pirateShipProvider;
    readonly ShipmentDataProvider shipmentProvider;
    readonly CargoDataProvider cargoProvider;

    public ShipmentService(PirateShipDataProvider pirateShipProvider, ShipmentDataProvider shipmentProvider, CargoDataProvider cargoProvider)
    {
        this.pirateShipProvider = pirateShipProvider;
        this.shipmentProvider = shipmentProvider;
        this.cargoProvider = cargoProvider;
    }

    public void AddShipment(int pirateShipId, DateTime date)
    {
        var shipment = new Shipment
        {
            PirateShipId = pirateShipId,
            Date = date
        };
        
        shipmentProvider.Add(shipment);
    }

    public void UpdateShipment(int shipmentId, DateTime newDate)
    {
        var shipment = shipmentProvider.GetShipmentById(shipmentId);
        
        if (shipment != null) 
        {
            shipment.Date = newDate; 
            shipmentProvider.Update(shipment);
        }
    }

    public void DeleteShipment(int shipmentId)
    {
        var shipment = shipmentProvider.GetShipmentById(shipmentId); 

        if (shipment != null)
        {
            shipmentProvider.Delete(shipmentId);
        }
    }

    public IEnumerable<Shipment> GetAllShipments()
    {
        var shipments = shipmentProvider.GetAll();

        if (shipments != null)
        {
            return shipments;
        }
        throw new ArgumentNullException();
    }
}