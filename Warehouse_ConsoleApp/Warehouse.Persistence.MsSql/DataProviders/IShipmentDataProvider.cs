using Warehouse.Model;

namespace Warehouse.Persistence.MsSql.DataProviders;

public interface IShipmentDataProvider<T> where T : Shipment
{
    void Add(T shipment);
    void Update(T shipment);
    void Delete(int id);
    T GetById(int id);
    IEnumerable<T> GetAll();
    T GetShipmentById(int id);
    T GetShipmentByDate(DateTime date);
    T GetShipmentByDateAndId(int pirateShipId, DateTime date);
}