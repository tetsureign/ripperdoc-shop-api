using RipperdocShop.Api.Models.Entities;
using RipperdocShop.Tests.TestUtils;
using Xunit;

namespace RipperdocShop.Tests.Models.Entities;

public class CategoryTests
{
    [Fact]
    public void Constructor_ShouldCreateValidCategory()
    {
        const string name = "Cyberware";
        const string desc = "Implants for the body and soul.";

        var category = new Category(name, desc);

        Assert.Equal(name, category.Name);
        Assert.Equal(desc, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotNull(category.Slug);
        Assert.Null(category.DeletedAt);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdateFields()
    {
        var category = new Category("Old", "Old Desc");
        category.UpdateDetails("New Name", "New Desc");

        Assert.Equal("New Name", category.Name);
        Assert.Equal("New Desc", category.Description);
        Assert.NotNull(category.Slug);
    }

    [Fact]
    public void SoftDelete_ShouldSetDeletedAt()
    {
        var category = new Category("Name", "Desc");
        category.SoftDelete();

        Assert.NotNull(category.DeletedAt);
    }

    [Fact]
    public void Restore_ShouldClearDeletedAt()
    {
        var category = new Category("Name", "Desc");
        category.SoftDelete();
        category.Restore();

        Assert.Null(category.DeletedAt);
    }
    
    [Fact]
    public async Task Can_Create_And_Fetch_Category()
    {
        // Arrange
        var context = TestDbContextFactory.CreateInMemoryDbContext("TestDb_Categories");
        var category = new Category("Cyberware", "Implants and enhancements for your chrome addiction");

        // Act
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Assert
        var result = context.Categories.FirstOrDefault(c => c.Name == "Cyberware");

        Assert.NotNull(result);
        Assert.Equal("Cyberware", result!.Name);
        Assert.Equal("Implants and enhancements for your chrome addiction", result.Description);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Null(result.DeletedAt);
    }
    
    [Fact]
    public async Task Can_SoftDelete_And_Restore_Category()
    {
        // Arrange
        var context = TestDbContextFactory.CreateInMemoryDbContext("TestDb_SoftDelete");
        var category = new Category("Weapons", "For turning meatbags into scrap");

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Act
        category.SoftDelete();
        await context.SaveChangesAsync();

        // Assert soft delete
        var softDeleted = context.Categories.FirstOrDefault(c => c.Id == category.Id);
        Assert.NotNull(softDeleted!.DeletedAt);

        // Act restore
        category.Restore();
        await context.SaveChangesAsync();

        // Assert restore
        var restored = context.Categories.FirstOrDefault(c => c.Id == category.Id);
        Assert.Null(restored!.DeletedAt);
    }

}
