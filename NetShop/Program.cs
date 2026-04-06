using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetShop;
using NetShop.Components;
using NetShop.Data;
using NetShop.Interfaces;
using NetShop.Models;
using NetShop.Services;
using Stripe;
using System.Text;

// SETUP NPM DEPENDENCIES
const string PrelineSourcePath = "node_modules/preline/dist/preline.js";
const string ToastifySourcePath = "node_modules/toastify-js/src/toastify.js";
const string JSDirectory = "wwwroot/js";

if (!System.IO.File.Exists(PrelineSourcePath) || !System.IO.File.Exists(ToastifySourcePath)) throw new InvalidOperationException("Missing JS dependencies. Run 'npm install'");

Directory.CreateDirectory(JSDirectory);

System.IO.File.Copy(PrelineSourcePath, Path.Combine(JSDirectory, Path.GetFileName(PrelineSourcePath)), true);
System.IO.File.Copy(ToastifySourcePath, Path.Combine(JSDirectory, Path.GetFileName(ToastifySourcePath)), true);

var builder = WebApplication.CreateBuilder(args);

// STRIPE SETUP
var stripeSection = builder.Configuration.GetSection("Stripe");
builder.Services.Configure<StripeSettings>(stripeSection);

var stripeSettings = stripeSection.Get<StripeSettings>() ?? throw new InvalidOperationException("Stripe settings are missing.");

StripeConfiguration.ApiKey = stripeSettings.SecretKey;

// JWT SETUP
var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JWTSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JWTSettings>() ?? throw new InvalidOperationException("JWT settings are missing.");

builder.Services.AddAuthentication(option => option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IProductService, NetShop.Services.ProductService>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<IUploadService, UploadService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await SeedData.InitializeAsync(app.Services);

app.Run();
