using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Model;

public class Cargo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    [Required]
    public int ShipmentId { get; set; } 

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } 

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Value cannot be negative.")]
    public decimal Value { get; set; }
    public virtual Shipment Shipment { get; set; }
}