using dccportal.org.Helper;
using AutoMapper;
using dccportal.org.Interface;
using dccportal.org.Repository;
using dccportal.org.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<dccportaldbContext>(options =>
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    string connStr;
    connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    
    options.UseSqlServer(connStr);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Home");
                    o.SlidingExpiration = true;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    //o.Cookie.Expiration = TimeSpan.FromMinutes(60);
                });
builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));
        
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

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

app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

