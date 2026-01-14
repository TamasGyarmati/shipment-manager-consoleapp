using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Model;

public class ShipmentEventArgs : EventArgs 
{
    public int ShipmentId { get; set; }
    public DateTime Date { get; set; }
}

public class Shipment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int PirateShipId { get; set; } 

    [Required]
    public DateTime Date { get; set; }
    public event EventHandler<ShipmentEventArgs> ShipmentDelayed; // Esemény, amely figyeli, ha a szállítmány késik
    public bool IsDelayed => Date < DateTime.Now; 
    public virtual PirateShip PirateShip { get; set; } 
    public virtual ICollection<Cargo> Cargos { get; set; } = new HashSet<Cargo>(); 

    public void CheckForDelay()
    {
        if (IsDelayed)
        {
            OnShipmentDelayed();
        }
    }
    protected virtual void OnShipmentDelayed()
    {
        ShipmentDelayed?.Invoke(this, new ShipmentEventArgs { ShipmentId = Id, Date = Date });
    }
}