using Warehouse.Model;

namespace Warehouse.Persistence.MsSql.DataProviders;

public interface IPirateShipDataProvider<T> where T : PirateShip
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    T GetById(int id);
    IEnumerable<PirateShip> GetAll();
}