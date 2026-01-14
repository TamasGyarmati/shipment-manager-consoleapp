using Warehouse.Model;
using Warehouse.Persistence.MsSql;

namespace Warehouse.Application;

public class PirateShipService
{
    readonly PirateShipDataProvider pirateShipProvider;
    readonly ShipmentDataProvider shipmentProvider;
    readonly CargoDataProvider cargoProvider;

    public PirateShipService(PirateShipDataProvider pirateShipProvider, ShipmentDataProvider shipmentProvider, CargoDataProvider cargoProvider)
    {
        this.pirateShipProvider = pirateShipProvider;
        this.shipmentProvider = shipmentProvider;
        this.cargoProvider = cargoProvider;
    }
    
    public IEnumerable<PirateShip> GetAllPirateShips()
    {
        var pirateShips = pirateShipProvider.GetAll();

        if (pirateShips != null)
        {
            return pirateShips;
        }
        throw new ArgumentNullException();
    }
}