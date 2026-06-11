using RipperdocShop.Api.Modules.Auth;
using RipperdocShop.Api.Modules.Brands.Admin;
using RipperdocShop.Api.Modules.Brands.Core;
using RipperdocShop.Api.Modules.Brands.Customer;
using RipperdocShop.Api.Modules.Categories.Admin;
using RipperdocShop.Api.Modules.Categories.Core;
using RipperdocShop.Api.Modules.Categories.Customer;
using RipperdocShop.Api.Modules.Customers.Admin;
using RipperdocShop.Api.Modules.Customers.Core;
using RipperdocShop.Api.Modules.Images;
using RipperdocShop.Api.Modules.Products.Admin;
using RipperdocShop.Api.Modules.Products.Core;
using RipperdocShop.Api.Modules.Products.Customer;
using RipperdocShop.Api.Modules.Ratings.Admin;
using RipperdocShop.Api.Modules.Ratings.Core;
using RipperdocShop.Api.Modules.Ratings.Customer;

namespace RipperdocShop.Api.Modules;

public static class AppModulesServiceCollectionExtensions
{
    public static IServiceCollection AddAppModules(this IServiceCollection services)
    {
        services.AddScoped<IBrandCoreService, BrandCoreService>();
        services.AddScoped<IAdminBrandService, AdminBrandService>();
        services.AddScoped<ICustomerBrandService, CustomerBrandService>();

        services.AddScoped<ICategoryCoreService, CategoryCoreService>();
        services.AddScoped<IAdminCategoryService, AdminCategoryService>();
        services.AddScoped<ICustomerCategoryService, CustomerCategoryService>();

        services.AddScoped<IProductCoreService, ProductCoreService>();
        services.AddScoped<IAdminProductService, AdminProductService>();
        services.AddScoped<ICustomerProductService, CustomerProductService>();

        services.AddScoped<IProductRatingCoreService, ProductRatingCoreService>();
        services.AddScoped<IAdminProductRatingService, AdminProductRatingService>();
        services.AddScoped<ICustomerProductRatingService, CustomerProductRatingService>();

        services.AddScoped<IAdminCustomerListService, AdminCustomerListService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<JwtService>();

        return services;
    }
}
