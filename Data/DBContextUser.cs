using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DBContextUser : IdentityDbContext<ApplicationUser>
{

    /// <summary>
    /// Khai báo các tbl
    /// </summary>
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Line> Line { get; set; }
    public DbSet<Task> Task { get; set; }

    public DBContextUser(DbContextOptions<DBContextUser> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        /// Khởi tạo tbl
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("User");
        builder.Entity<IdentityRole>().ToTable("Role");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");


        builder.Entity<Category>();
        builder.Entity<Product>();
        builder.Entity<Order>();
        builder.Entity<Line>();
        builder.Entity<Task>();


    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

}