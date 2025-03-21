using GestionaleHotel.Data;
using GestionaleHotel.Models;
using GestionaleHotel.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount =
       builder.Configuration.GetSection("Identity").GetValue<bool>("RequireConfirmedAccount");

    options.Password.RequiredLength =
        builder.Configuration.GetSection("Identity").GetValue<int>("RequiredLength");

    options.Password.RequireDigit =
        builder.Configuration.GetSection("Identity").GetValue<bool>("RequireDigit");

    options.Password.RequireLowercase =
        builder.Configuration.GetSection("Identity").GetValue<bool>("RequireLowercase");

    options.Password.RequireNonAlphanumeric =
        builder.Configuration.GetSection("Identity").GetValue<bool>("RequireNonAlphanumeric");

    options.Password.RequireUppercase =
        builder.Configuration.GetSection("Identity").GetValue<bool>("RequireUppercase");
})

    .AddEntityFrameworkStores<HotelDbContext>()

    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

// Ruoli
async Task SeedRolesAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    string[] roleNames = { "Admin", "Editor", "Viewer" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }
}
async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    string adminEmail = "admin@hotel.com";
    string adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "Hotel",
            BirthDate = new DateOnly(1980, 1, 1)
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}


// Cookies
builder.Services.AddAuthentication(
    options => {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options => {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        
        options.Cookie.Name = "GestionaleHotel";
    });

// Servicies
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IPrenotazioneService, PrenotazioneService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAsync(services);
    await SeedAdminUserAsync(services);
}

app.Run();
