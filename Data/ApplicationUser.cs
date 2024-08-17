using Microsoft.AspNetCore.Identity;
// cấu hình Identity user 
public class ApplicationUser : IdentityUser
{
    // tạo mối kết nối với Line(chuyền)
    public List<Line> Lines { get; set; }
}