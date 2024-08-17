using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Line
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LineID { get; set; }
    public string LineName { get; set; }
    [StringLength(450)]
    public string Id { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public List<Task> Tasks { get; set; }
}