using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using spbu.Data;

var builder = WebApplication.CreateBuilder(args);

string _dbConnectionString()
{
    var dbname = Environment.GetEnvironmentVariable("DB_NAME");
    var dbpassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var dbuser = Environment.GetEnvironmentVariable("DB_USER");
    var dbhost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbport = Environment.GetEnvironmentVariable("DB_PORT");

    //return $"Server={dbhost};Port={dbport};Database={dbname};User Id={dbuser};Password={dbpassword};";
    return builder.Configuration.GetConnectionString("DefaultConnection");
}

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(_dbConnectionString()));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
