using Warehouse.Model;

namespace Warehouse.Persistence.MsSql.Repository;

public interface ICargoDataProvider<T> where T : Cargo
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    T GetById(int id);
    IEnumerable<T> GetAll();
    T GetCargoById(int id);
    List<(string ShipName, DateTime ShipmentDate, int Quantity)> SearchByCargoType (string cargoType);
}