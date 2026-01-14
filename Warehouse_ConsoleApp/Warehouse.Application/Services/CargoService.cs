using Warehouse.Model;
using Warehouse.Persistence.MsSql;

namespace Warehouse.Application;

public class CargoService
{
    readonly CargoDataProvider cargoProvider;
    readonly ShipmentDataProvider shipmentProvider;
    readonly PirateShipDataProvider pirateShipProvider;

    public CargoService(CargoDataProvider cargoProvider, ShipmentDataProvider shipmentProvider, PirateShipDataProvider pirateShipProvider)
    {
        this.cargoProvider = cargoProvider;
        this.shipmentProvider = shipmentProvider;
        this.pirateShipProvider = pirateShipProvider;
    }
    
    public void AddCargo(int shipmentId, string type, int quantity, decimal value)
    {
        var cargo = new Cargo
        {
            ShipmentId = shipmentId,
            Type = type,
            Quantity = quantity,
            Value = value
        };
        
        cargoProvider.Add(cargo);
    }
    
    public void UpdateCargo(int cargoId, string newType, int newQuantity, decimal newValue)
    {
        var cargo = cargoProvider.GetAll().FirstOrDefault(c => c.Id == cargoId);
        
        if (cargo != null)
        {
            cargo.Type = newType;
            cargo.Quantity = newQuantity;
            cargo.Value = newValue;
            cargoProvider.Update(cargo);
        }
    }
    
    public void DeleteCargo(int cargoId)
    {
        var cargo = cargoProvider.GetCargoById(cargoId); 
        
        if (cargo != null) 
        {
            cargoProvider.Delete(cargoId);
        }
    }
    
    public IEnumerable<Cargo> GetAllCargos()
    {
        var cargos = cargoProvider.GetAll();

        if (cargos != null)
        {
            return cargos;
        }
        throw new ArgumentNullException();
    }
}