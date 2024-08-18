using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }
    [StringLength(256)]
    [Required]
    public required string ProductName { get; set; }
    [Required]
    public required decimal Price { get; set; }

    public int CategoryID { get; set; }

    public required Category category { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}