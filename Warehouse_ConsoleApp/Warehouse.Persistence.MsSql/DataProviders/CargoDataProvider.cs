using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Persistence.MsSql.Repository;

namespace Warehouse.Persistence.MsSql
{
    public class CargoDataProvider : ICargoDataProvider<Cargo>
    {
        readonly AppDbContext context;

        public CargoDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(Cargo cargo)
        {
            context.Cargos.Add(cargo);
            context.SaveChanges();
        }

        public void Update(Cargo cargo)
        {
            context.Cargos.Update(cargo);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cargo = context.Cargos.Find(id);
            if (cargo != null)
            {
                context.Cargos.Remove(cargo);
                context.SaveChanges();
            }
        }

        public Cargo GetById(int id)
        {
            return context.Cargos
                .Include(c => c.Shipment)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Cargo> GetAll()
        {
            return context.Cargos
                .Include(c => c.Shipment)
                .ToList();
        }
        
        public Cargo GetCargoById(int id)
        {
            return context.Cargos
                .Include(c => c.Shipment) 
                .FirstOrDefault(c => c.Id == id);
        }

        // Search Cargo by Type
        public List<(string ShipName, DateTime ShipmentDate, int Quantity)> SearchByCargoType(string cargoType)
        {
            return context.Shipments
                .Include(s => s.PirateShip)
                .Include(s => s.Cargos)
                .Where(s => s.Cargos.Any(c => c.Type == cargoType))
                .Select(s => new
                {
                    ShipName = s.PirateShip.Name,
                    ShipmentDate = s.Date,
                    Quantity = s.Cargos.Where(c => c.Type == cargoType).Sum(c => c.Quantity)
                })
                .AsEnumerable()
                .Select(r => (r.ShipName, r.ShipmentDate, r.Quantity))
                .ToList();
        }
    }
}