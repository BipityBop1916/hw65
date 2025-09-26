using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using chatmvc.Data;
using chatmvc.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity configuration with Russian describer
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false; // optional
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        // Lockout settings if desired
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(3650);
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddErrorDescriber<ErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    async Task Seed()
    {
        var roles = new[] { "admin", "user" };
        foreach (var r in roles)
            if (!await roleManager.RoleExistsAsync(r))
                await roleManager.CreateAsync(new IdentityRole(r));

        var adminEmail = "admin@mychat.local";
        var adminUserName = "admin";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                DateOfBirth = new DateTime(1990, 1, 1),
                EmailConfirmed = true
            };
            var create = await userManager.CreateAsync(admin, "Admin123!");
            if (create.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }

    Seed().GetAwaiter().GetResult();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();