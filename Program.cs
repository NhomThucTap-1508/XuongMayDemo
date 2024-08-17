using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



/// Thêm dịch vụ vào đây từ folder Service
builder.Services.AddScoped<CategoryService>();




/// cấu hình cors cho phép cổng 3000 kết nối
var MyAllowSpecificOrigins = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());

});
// khai báo dịch vụ Context, kết nối đến sql server
builder.Services.AddDbContext<DBContextUser>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql"));
});
// khai báo dịch vụ Identity quản lý người dùng
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DBContextUser>()
    .AddDefaultTokenProviders();

/// Cấu hình xác thực người dùng(JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secretKey"]))
    };
});

/// Cấu hình phân quyền user
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("LineLeader", policy => policy.RequireClaim("Role", "LineLeader"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/// cấu hình xác thực và phân quyền người dùng
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
