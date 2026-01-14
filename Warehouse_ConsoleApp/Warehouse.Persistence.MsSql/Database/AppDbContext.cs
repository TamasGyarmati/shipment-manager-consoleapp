using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using Warehouse.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Warehouse.Persistence.MsSql;

public class AppDbContext : DbContext
{
    public DbSet<PirateShip> PirateShips { get; set; }
    public DbSet<Shipment> Shipments { get; set; } 
    public DbSet<Cargo> Cargos { get; set; } 
    
    public AppDbContext()
    {
        //this.Database.EnsureDeleted(); 
        this.Database.EnsureCreated();
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            string connectionString ="Data Source=localhost,1433;Initial Catalog=OnePieceDatabase;User ID=sa;Password=SqlServerPassw0rd!;TrustServerCertificate=True";
            optionsBuilder.UseSqlServer(connectionString).UseLazyLoadingProxies();
        }
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PirateShip>()
                .HasMany(p => p.Shipments) 
                .WithOne(s => s.PirateShip)
                .HasForeignKey(s => s.PirateShipId)
                .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Shipment>()
                .HasMany(s => s.Cargos) 
                .WithOne(c => c.Shipment)
                .HasForeignKey(c => c.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder); 
    }
}