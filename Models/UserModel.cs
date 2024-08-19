using System.ComponentModel.DataAnnotations;

public class UserModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}