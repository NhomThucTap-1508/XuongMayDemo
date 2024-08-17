using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryID { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}