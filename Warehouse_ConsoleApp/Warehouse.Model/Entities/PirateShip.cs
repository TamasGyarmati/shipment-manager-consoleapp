using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Model;

public class PirateShip
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } 

    [Required]
    [MaxLength(50)]
    public string CaptainName { get; set; } 

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than zero.")]
    public int Capacity { get; set; }
    public virtual ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
}