using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RipperdocShop.Api.Data;
using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Api.Modules.Brands;
using RipperdocShop.Api.Modules.Categories;
using RipperdocShop.Api.Modules.Products;
using RipperdocShop.Api.Modules.Products.Commands;
using RipperdocShop.Api.Modules.Products.Queries;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Tests.TestUtils;

namespace RipperdocShop.Tests.Modules.Products;

public class ProductUseCaseTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
            cfg.AddProfile<CategoryMappingProfile>();
            cfg.AddProfile<BrandMappingProfile>();
        });

        return config.CreateMapper();
    }

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
    public async Task GetProductByIdQuery_Returns_Product()
    {
        var context = await GetDbContextWithData("TestDb_ProductGet");
        var query = new GetProductByIdQuery(context);
        var id = context.Products.First().Id;

        var product = await query.ExecuteAsync(id);

        Assert.NotNull(product);
        Assert.Equal("Monowire", product!.Name);
    }

    [Fact]
    public async Task ListAdminProductsQuery_Includes_Category_And_Brand()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetWithDetails");
        var query = new ListAdminProductsQuery(context);

        var response = await query.ExecuteAsync(includeDeleted: false, 1, 10);
        var product = response.Products.Single();

        Assert.NotNull(product.Category);
        Assert.NotNull(product.Brand);
        Assert.Equal("Cyberware", product.Category.Name);
        Assert.Equal("Arasaka", product.Brand!.Name);
    }

    [Fact]
    public async Task GetProductBySlugQuery_Returns_Correct_Product()
    {
        var context = await GetDbContextWithData("TestDb_ProductGetBySlug");
        var query = new GetProductBySlugQuery(context, CreateMapper());
        var slug = context.Products.First().Slug;

        var product = await query.ExecuteAsync(slug);

        Assert.NotNull(product);
        Assert.Equal("Monowire", product!.Name);
    }

    [Fact]
    public async Task ListProductsByCategorySlugQuery_Respects_IncludeDeleted_Flag()
    {
        var context = await GetDbContextWithData("TestDb_ProductCategoryIncludeDeleted");
        var category = context.Categories.First();
        category.SoftDelete();
        await context.SaveChangesAsync();

        var query = new ListProductsByCategorySlugQuery(context, CreateMapper());

        var response1 = await query.ExecuteAsync(category.Slug, includeDeleted: false, 1, 10);
        var response2 = await query.ExecuteAsync(category.Slug, includeDeleted: true, 1, 10);

        Assert.Empty(response1.Products);
        Assert.NotEmpty(response2.Products);
    }

    [Fact]
    public async Task ListProductsByBrandSlugQuery_Returns_Products_When_NotDeleted()
    {
        var context = await GetDbContextWithData("TestDb_ProductBrandNotDeleted");
        var brand = context.Brands.First();

        var query = new ListProductsByBrandSlugQuery(context, CreateMapper());
        var response = await query.ExecuteAsync(brand.Slug, includeDeleted: false, 1, 10);

        Assert.NotEmpty(response.Products);
        Assert.Equal(1, response.TotalCount);
        Assert.Equal(1, response.TotalPages);
    }

    [Fact]
    public async Task ListAdminProductsQuery_Returns_All_When_IncludeDeleted()
    {
        var context = await GetDbContextWithData("TestDb_ProductListAdmin");
        var product = context.Products.First();
        product.SoftDelete();
        await context.SaveChangesAsync();

        var query = new ListAdminProductsQuery(context);

        var response1 = await query.ExecuteAsync(includeDeleted: false, 1, 10);
        var response2 = await query.ExecuteAsync(includeDeleted: true, 1, 10);

        Assert.Empty(response1.Products);
        Assert.Single(response2.Products);
    }

    [Fact]
    public async Task ListFeaturedProductsQuery_Returns_Only_Featured_Products()
    {
        var context = await GetDbContextWithData("TestDb_ProductFeatured");
        var query = new ListFeaturedProductsQuery(context, CreateMapper());

        var featured = await query.ExecuteAsync();

        Assert.Single(featured);
        Assert.True(featured.All(p => p.IsFeatured));
    }

    [Fact]
    public async Task CreateProductCommand_Creates_Product()
    {
        var context = await GetDbContextWithData("TestDb_ProductCreate");
        var category = context.Categories.First();
        var brand = context.Brands.First();
        var command = new CreateProductCommand(context);

        var product = await command.ExecuteAsync(new ProductCreateDto
        {
            Name = "Sandevistan",
            Description = "Speedware.",
            ImageUrl = "https://example.com/sandy.png",
            Price = 9999,
            IsFeatured = false,
            CategoryId = category.Id,
            BrandId = brand.Id
        });

        Assert.NotNull(product);
        Assert.Equal("Sandevistan", product!.Name);
        Assert.Equal(2, await context.Products.CountAsync());
    }

    [Fact]
    public async Task UpdateProductCommand_Updates_Product()
    {
        var context = await GetDbContextWithData("TestDb_ProductUpdate");
        var product = context.Products.First();
        var category = context.Categories.First();
        var command = new UpdateProductCommand(context);

        var updated = await command.ExecuteAsync(product.Id, new ProductCreateDto
        {
            Name = "Updated Monowire",
            Description = "Sharper.",
            ImageUrl = "https://example.com/updated.png",
            Price = 5000,
            CategoryId = category.Id
        });

        Assert.NotNull(updated);
        Assert.Equal("Updated Monowire", updated!.Name);
    }

    [Fact]
    public async Task SoftDeleteAndRestoreProductCommands_Update_Delete_State()
    {
        var context = await GetDbContextWithData("TestDb_ProductDeleteRestore");
        var product = context.Products.First();

        var softDelete = new SoftDeleteProductCommand(context);
        var restore = new RestoreProductCommand(context);

        var deleted = await softDelete.ExecuteAsync(product.Id);
        Assert.NotNull(deleted!.DeletedAt);

        var restored = await restore.ExecuteAsync(product.Id);
        Assert.Null(restored!.DeletedAt);
    }
}
