using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Products.Core;
using RipperdocShop.Tests.TestUtils;

namespace RipperdocShop.Tests.Services.Core;

public class ProductCoreServiceTests
{
    private async Task<ApplicationDbContext> GetDbContextWithData(string dbName)
    {
        var context = TestDbContextFactory.CreateInMemoryDbContext(dbName);

        var cat = new Category("Cyberware", "For your chrome needs.");
        var brand = new Brand("Arasaka", "Big corpo toys.");
        var product = new Product(
            name: "Monowire",
            description: "Slice and dice, stealth-style.",
            imageUrl: "https://example.com/monowire.png",
            price: 4999,
            category: cat,
            brand: brand,
            isFeatured: true
        );

        context.Categories.Add(cat);
        context.Brands.Add(brand);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Product()
    {
        // Arrange
        var context = await GetDbContextWithData("TestDb_ProductGet");
        var service = new ProductCoreService(context);
        var id = context.Products.First().Id;

        // Act
        var product = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(product);
        Assert.Equal("Monowire", product!.Name);
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_Includes_Category_And_Brand()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetWithDetails");
        var service = new ProductCoreService(context);
        var id = context.Products.First().Id;

        var product = await service.GetByIdWithDetailsAsync(id);

        Assert.NotNull(product);
        Assert.NotNull(product!.Category);
        Assert.NotNull(product.Brand);
        Assert.Equal("Cyberware", product.Category.Name);
        Assert.Equal("Arasaka", product.Brand!.Name);
    }

    [Fact]
    public async Task GetBySlugWithDetailsAsync_Returns_Correct_Product()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetWithDetails");
        var service = new ProductCoreService(context);
        var slug = context.Products.First().Slug;

        var product = await service.GetBySlugWithDetailsAsync(slug);

        Assert.NotNull(product);
        Assert.Equal("Monowire", product!.Name);
    }

    [Fact]
    public async Task GetByCategorySlugAsync_Respects_IncludeDeleted_Flag()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetWithDetailsIncludeDeleted");
        var category = context.Categories.First();
        category.SoftDelete();
        await context.SaveChangesAsync();

        var service = new ProductCoreService(context);

        var (products1, _, _) = await service.GetByCategorySlugAsync(category.Slug, includeDeleted: false, 1, 10);
        var (products2, _, _) = await service.GetByCategorySlugAsync(category.Slug, includeDeleted: true, 1, 10);

        Assert.Empty(products1);
        Assert.NotEmpty(products2);
    }

    [Fact]
    public async Task GetByBrandSlugAsync_Returns_Products_When_NotDeleted()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetWithDetailsNotDeleted");
        var brand = context.Brands.First();

        var service = new ProductCoreService(context);
        var (products, totalCount, totalPages) =
            await service.GetByBrandSlugAsync(brand.Slug, includeDeleted: false, 1, 10);

        Assert.NotEmpty(products);
        Assert.Equal(1, totalCount);
        Assert.Equal(1, totalPages);
    }

    [Fact]
    public async Task GetAllAsync_Returns_All_When_IncludeDeleted()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetAll");
        var product = context.Products.First();
        product.SoftDelete();
        await context.SaveChangesAsync();

        var service = new ProductCoreService(context);

        var (products1, _, _) = await service.GetAllAsync(includeDeleted: false, 1, 10);
        var (products2, _, _) = await service.GetAllAsync(includeDeleted: true, 1, 10);

        Assert.Empty(products1);
        Assert.Single(products2);
    }

    [Fact]
    public async Task GetFeaturedAsync_Returns_Only_Featured_Products()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetFeatured");
        var service = new ProductCoreService(context);

        var featured = await service.GetFeaturedAsync();

        Assert.Single(featured);
        Assert.True(featured.All(p => p.IsFeatured));
    }
}
