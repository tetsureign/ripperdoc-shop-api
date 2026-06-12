using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Shared.DTOs;
using RipperdocShop.Web.Models.ViewModels;
using RipperdocShop.Web.Services;

namespace RipperdocShop.Web.Controllers;

public class ProductsController(
    IProductService productService,
    ICategoryService categoryService,
    IProductRatingService productRatingService) : Controller
{
    public async Task<IActionResult> Details(string slug)
    {
        var product = await productService.GetBySlugAsync(slug);
        var ratingsResult = await productRatingService.GetByProductSlug(slug);

        if (product == null)
        {
            return NotFound();
        }

        var result = new ProductDetailsViewModel
        {
            Product = product,
            Ratings = ratingsResult,
        };

        return View(result);
    }

    public async Task<IActionResult> ByCategory(string slug)
    {
        var products = await productService.GetByCategorySlugAsync(slug);
        if (products == null)
        {
            return NotFound();
        }

        var category = await categoryService.GetBySlugAsync(slug);

        var byCategory = new CategoryProductsViewModel
        {
            Category = category,
            Products = products
        };

        return View(byCategory);
    }

    public async Task<IActionResult> Index()
    {
        var products = await productService.GetAllAsync();
        return View(products);
    }
}
