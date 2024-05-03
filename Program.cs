using Microsoft.EntityFrameworkCore;
using ThreeFriends.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


/*
builder.Services.AddDbContext<Appdbcontxt>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
*/


builder.Services.AddDbContext<Appdbcontxt>(options =>
{
    // Configure the DbContext to use SQLite with the connection string
    options.UseSqlite("Data Source=DataBase\\CoinGuard.db");
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
