using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Task
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskID { get; set; }
    [StringLength(256)]
    public string TaskName { get; set; }
    [Column(TypeName = "ntext")]
    [StringLength(256)]
    public string Note { get; set; }

    public int OrderID { get; set; }
    public Order Order { get; set; }

}