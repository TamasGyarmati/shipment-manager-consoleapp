using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Persistence.MsSql.Repository;
using Warehouse.Persistence.MsSql.DataProviders;

namespace Warehouse.Persistence.MsSql
{
    public class PirateShipDataProvider : IPirateShipDataProvider<PirateShip>
    {
        readonly AppDbContext context;

        public PirateShipDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(PirateShip pirateShip)
        {
            context.PirateShips.Add(pirateShip);
            context.SaveChanges();
        }

        public void Update(PirateShip pirateShip)
        {
            context.PirateShips.Update(pirateShip);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pirateShip = context.PirateShips.Find(id);
            if (pirateShip != null)
            {
                context.PirateShips.Remove(pirateShip);
                context.SaveChanges();
            }
        }

        public PirateShip GetById(int id)
        {
            return context.PirateShips
                .Include(p => p.Shipments)
                .ThenInclude(s => s.Cargos)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<PirateShip> GetAll()
        {
            return context.PirateShips
                .Include(p => p.Shipments)
                .ThenInclude(s => s.Cargos)
                .ToList();
        }

        // ReportService-hez kell
        public PirateShip PirateShipReportGenerator(int pirateShipId)
        {
            return context.PirateShips
                .Include(s => s.Shipments)          // Betöltjük a szállítmányokat
                .ThenInclude(c => c.Cargos)         // Betöltjük a rakományokat a szállítmányokhoz
                .FirstOrDefault(p => p.Id == pirateShipId);
        }
    }
}