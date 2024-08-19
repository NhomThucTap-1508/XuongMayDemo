using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderID { get; set; }

    public int Quantity { get; set; }

    public int ProductID { get; set; }

    public int CategoryID { get; set; }
    public Product Product { get; set; }
    public List<Task> Tasks { get; set; }
}