using Microsoft.Extensions.DependencyInjection;
using RipperdocShop.Api.Modules;
using RipperdocShop.Api.Modules.Auth;
using RipperdocShop.Api.Modules.Auth.Commands;
using RipperdocShop.Api.Modules.Auth.Queries;
using RipperdocShop.Api.Modules.Brands.Commands;
using RipperdocShop.Api.Modules.Brands.Queries;
using RipperdocShop.Api.Modules.Categories.Commands;
using RipperdocShop.Api.Modules.Categories.Queries;
using RipperdocShop.Api.Modules.Customers.Queries;
using RipperdocShop.Api.Modules.Images.Commands;
using RipperdocShop.Api.Modules.Products.Commands;
using RipperdocShop.Api.Modules.Products.Queries;
using RipperdocShop.Api.Modules.Ratings.Commands;
using RipperdocShop.Api.Modules.Ratings.Queries;

namespace RipperdocShop.Tests.Modules;

public class AppModulesServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAppModules_Registers_Command_And_Query_Classes()
    {
        var services = new ServiceCollection();

        services.AddAppModules();

        AssertScoped<JwtService>(services);
        AssertScoped<LoginCommand>(services);
        AssertScoped<RegisterCommand>(services);
        AssertScoped<LogoutCommand>(services);
        AssertScoped<WhoAmIQuery>(services);

        AssertScoped<CreateBrandCommand>(services);
        AssertScoped<UpdateBrandCommand>(services);
        AssertScoped<SoftDeleteBrandCommand>(services);
        AssertScoped<RestoreBrandCommand>(services);
        AssertScoped<DeleteBrandPermanentlyCommand>(services);
        AssertScoped<ListAdminBrandsQuery>(services);
        AssertScoped<GetBrandByIdQuery>(services);
        AssertScoped<ListBrandsQuery>(services);
        AssertScoped<GetBrandBySlugQuery>(services);

        AssertScoped<CreateCategoryCommand>(services);
        AssertScoped<UpdateCategoryCommand>(services);
        AssertScoped<SoftDeleteCategoryCommand>(services);
        AssertScoped<RestoreCategoryCommand>(services);
        AssertScoped<DeleteCategoryPermanentlyCommand>(services);
        AssertScoped<ListAdminCategoriesQuery>(services);
        AssertScoped<GetCategoryByIdQuery>(services);
        AssertScoped<ListCategoriesQuery>(services);
        AssertScoped<GetCategoryBySlugQuery>(services);

        AssertScoped<ListAdminCustomersQuery>(services);
        AssertScoped<GetUserByIdQuery>(services);
        AssertScoped<UploadImageCommand>(services);

        AssertScoped<CreateProductCommand>(services);
        AssertScoped<UpdateProductCommand>(services);
        AssertScoped<SetProductFeaturedCommand>(services);
        AssertScoped<RemoveProductFeaturedCommand>(services);
        AssertScoped<SoftDeleteProductCommand>(services);
        AssertScoped<RestoreProductCommand>(services);
        AssertScoped<DeleteProductPermanentlyCommand>(services);
        AssertScoped<ListAdminProductsQuery>(services);
        AssertScoped<GetProductByIdQuery>(services);
        AssertScoped<ListProductsQuery>(services);
        AssertScoped<GetProductBySlugQuery>(services);
        AssertScoped<ListProductsByCategorySlugQuery>(services);
        AssertScoped<ListProductsByBrandSlugQuery>(services);
        AssertScoped<ListFeaturedProductsQuery>(services);

        AssertScoped<CreateProductRatingCommand>(services);
        AssertScoped<UpdateProductRatingCommand>(services);
        AssertScoped<SoftDeleteProductRatingCommand>(services);
        AssertScoped<RestoreProductRatingCommand>(services);
        AssertScoped<DeleteProductRatingPermanentlyCommand>(services);
        AssertScoped<GetProductRatingByIdQuery>(services);
        AssertScoped<ListProductRatingsByProductSlugQuery>(services);
        AssertScoped<ListAdminProductRatingsByProductQuery>(services);
        AssertScoped<ListAdminProductRatingsByUserQuery>(services);
    }

    private static void AssertScoped<TService>(IServiceCollection services)
    {
        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(TService) &&
            descriptor.ImplementationType == typeof(TService) &&
            descriptor.Lifetime == ServiceLifetime.Scoped);
    }
}
