using Microsoft.EntityFrameworkCore;
using Warehouse.Model;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Persistence.MsSql.Repository;
using Warehouse.Persistence.MsSql.DataProviders;

namespace Warehouse.Persistence.MsSql
{
    public class ShipmentDataProvider : IShipmentDataProvider<Shipment>
    {
        readonly AppDbContext context;

        public ShipmentDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(Shipment shipment)
        {
            context.Shipments.Add(shipment);
            context.SaveChanges();
        }

        public void Update(Shipment shipment)
        {
            context.Shipments.Update(shipment);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var shipment = context.Shipments.Find(id);
            if (shipment != null)
            {
                context.Shipments.Remove(shipment);
                context.SaveChanges();
            }
        }

        public Shipment GetById(int id)
        {
            return context.Shipments
                .Include(s => s.Cargos)
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Shipment> GetAll()
        {
            return context.Shipments
                .Include(s => s.Cargos)
                .ToList();
        }
        
        public Shipment GetShipmentById(int id)
        {
            return context.Shipments
                .Include(s => s.Cargos)
                .FirstOrDefault(s => s.Id == id);
        }
        
        public Shipment GetShipmentByDate(DateTime date)
        {
            return context.Shipments
                .Include(s => s.Cargos) 
                .FirstOrDefault(s => s.Date.Date == date.Date);
        }
    
        public Shipment GetShipmentByDateAndId(int pirateShipId, DateTime date)
        {
            return context.Shipments
                .Include(s => s.Cargos) 
                .FirstOrDefault(s => s.PirateShipId == pirateShipId && s.Date.Date == date.Date); 
        }
    }
}