using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Tests.TestUtils;

namespace RipperdocShop.Tests.Models.Entities;

public class ProductTests
{
    [Fact]
    public async Task Can_Create_Product_With_Category_And_Optional_Brand()
    {
        var context = TestDbContextFactory.CreateInMemoryDbContext("TestDb_Product");

        var category = new Category("Cyberware", "Upgrade your meat");
        var brand = new Brand("Arasaka", "We own you");

        context.Categories.Add(category);
        context.Brands.Add(brand);
        await context.SaveChangesAsync();

        var product = new Product(
            name: "Monowire",
            description: "For slicing meatbags with elegance",
            imageUrl: "https://example.com/monowire.png",
            price: 9999,
            category: category,
            isFeatured: true,
            brand: brand
        );

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var result = context.Products
            .FirstOrDefault(p => p.Name == "Monowire");

        Assert.NotNull(result);
        Assert.Equal("Monowire", result!.Name);
        Assert.Equal(category.Id, result.CategoryId);
        Assert.Equal(brand.Id, result.BrandId);
        Assert.True(result.IsFeatured);
    }

    [Fact]
    public async Task Can_Update_Product_Details()
    {
        var context = TestDbContextFactory.CreateInMemoryDbContext("TestDb_ProductUpdate");

        var category1 = new Category("Cyberware", "First gen stuff");
        var category2 = new Category("Chrome", "Second gen ultra-sleek");

        var brand1 = new Brand("Arasaka", "Still evil");
        var brand2 = new Brand("Militech", "Just differently evil");

        context.Categories.AddRange(category1, category2);
        context.Brands.AddRange(brand1, brand2);
        await context.SaveChangesAsync();

        var product = new Product("Smartgun", "Auto-aim pewpew", "https://img/smartgun.jpg", 5000, category1, false, brand1);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        product.UpdateDetails("Smartgun 2.0", "Even smarter pewpew", "https://img/smartgun2.jpg", 7500, category2, brand2);
        await context.SaveChangesAsync();

        var updated = context.Products.FirstOrDefault(p => p.Id == product.Id);
        Assert.Equal("Smartgun 2.0", updated!.Name);
        Assert.Equal(7500, updated.Price);
        Assert.Equal(category2.Id, updated.CategoryId);
        Assert.Equal(brand2.Id, updated.BrandId);
    }
}
