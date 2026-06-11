using Microsoft.Extensions.DependencyInjection;
using RipperdocShop.Api.Modules;
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

namespace RipperdocShop.Tests.Modules;

public class AppModulesServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAppModules_Registers_Module_Services()
    {
        var services = new ServiceCollection();

        services.AddAppModules();

        AssertServiceRegistered<IBrandCoreService, BrandCoreService>(services);
        AssertServiceRegistered<IAdminBrandService, AdminBrandService>(services);
        AssertServiceRegistered<ICustomerBrandService, CustomerBrandService>(services);

        AssertServiceRegistered<ICategoryCoreService, CategoryCoreService>(services);
        AssertServiceRegistered<IAdminCategoryService, AdminCategoryService>(services);
        AssertServiceRegistered<ICustomerCategoryService, CustomerCategoryService>(services);

        AssertServiceRegistered<IProductCoreService, ProductCoreService>(services);
        AssertServiceRegistered<IAdminProductService, AdminProductService>(services);
        AssertServiceRegistered<ICustomerProductService, CustomerProductService>(services);

        AssertServiceRegistered<IProductRatingCoreService, ProductRatingCoreService>(services);
        AssertServiceRegistered<IAdminProductRatingService, AdminProductRatingService>(services);
        AssertServiceRegistered<ICustomerProductRatingService, CustomerProductRatingService>(services);

        AssertServiceRegistered<IAdminCustomerListService, AdminCustomerListService>(services);
        AssertServiceRegistered<IUserService, UserService>(services);
        AssertServiceRegistered<IImageService, ImageService>(services);
        AssertServiceRegistered<JwtService, JwtService>(services);
    }

    private static void AssertServiceRegistered<TService, TImplementation>(IServiceCollection services)
    {
        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(TService) &&
            descriptor.ImplementationType == typeof(TImplementation) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);
    }
}
