namespace Warehouse.Persistence.MsSql;

public interface IDataLoader
{
    public void SeedData(string path);
}