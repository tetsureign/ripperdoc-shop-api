using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Interceptors;
using RipperdocShop.Api.Mapping;
using RipperdocShop.Api.Models.Identities;
using RipperdocShop.Api.Services;
using RipperdocShop.Api.Services.Admin;
using RipperdocShop.Api.Services.Core;
using RipperdocShop.Api.Services.Customer;

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["Jwt:Secret"];

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Database services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// DB Interceptors services
builder.Services.AddSingleton<TimestampInterceptor>();

// ASP.NET Identity services
builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

// AutoMapper services
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Logger
builder.Services.AddHttpLogging(options => { });

// Cookie configurations
if (builder.Environment.IsDevelopment())
{
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}

// App Services
builder.Services.AddScoped<IBrandCoreService, BrandCoreService>();
builder.Services.AddScoped<IAdminBrandService, AdminBrandService>();
builder.Services.AddScoped<ICategoryCoreService, CategoryCoreService>();
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IProductCoreService, ProductCoreService>();
builder.Services.AddScoped<IAdminProductService, AdminProductService>();
builder.Services.AddScoped<IProductRatingCoreService, ProductRatingCoreService>();
builder.Services.AddScoped<IAdminProductRatingService, AdminProductRatingService>();
builder.Services.AddScoped<IAdminCustomerListService, AdminCustomerListService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<ICustomerProductService, CustomerProductService>();
builder.Services.AddScoped<ICustomerCategoryService, CustomerCategoryService>();
builder.Services.AddScoped<ICustomerBrandService, CustomerBrandService>();
builder.Services.AddScoped<ICustomerProductRatingService, CustomerProductRatingService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();

// Authentication services
builder.Services.AddAuthentication()
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
     {
         if (builder.Environment.IsDevelopment()) 
             options.RequireHttpsMetadata = false;
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidAudience = builder.Configuration["Jwt:Audience"],
             ValidateIssuerSigningKey = true,
             ValidateLifetime = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                 jwtSecret ?? throw new InvalidOperationException())),
             ClockSkew = TimeSpan.Zero
         };
         options.Events = new JwtBearerEvents
         {
             OnMessageReceived = context =>
             {
                 if (context.Request.Cookies.TryGetValue("AccessToken", out var token))
                     context.Token = token;
                 return Task.CompletedTask;
             }
         };
     });

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // JWT Bearer
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put only your JWT Bearer token here.",
        
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    options.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtScheme,  [] }
    });
});

// CORS config

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins(builder.Configuration["AdminSite:URL"] ?? string.Empty)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMigrationsEndPoint();
}

app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

// Seed Roles & an Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var adminEmail = builder.Configuration["Admin:Email"] ?? Environment.GetEnvironmentVariable("ADMIN_EMAIL");
        var adminPassword = builder.Configuration["Admin:Password"] ?? Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new Exception("Admin:Email or Admin:Password is not set. Please set them in the appsettings.json file or in the environment variables ADMIN_EMAIL and ADMIN_PASSWORD.");
        }
        await IdentitySeeder.SeedAsync(services, adminEmail, adminPassword);;
        logger.LogInformation("Identity seeding complete.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding identity roles and users.");
        throw;
    }
}

app.Run();
