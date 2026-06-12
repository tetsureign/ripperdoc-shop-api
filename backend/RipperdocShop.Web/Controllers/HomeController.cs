using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Web.Models;
using RipperdocShop.Web.Services;
using RipperdocShop.Web.Models.ViewModels;

namespace RipperdocShop.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IProductService productService,
    ICategoryService categoryService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var categories = await categoryService.GetAllAsync();
        var featured = await productService.GetFeaturedAsync();
        
        var vm = new HomeViewModel
        {
            Categories = categories,
            FeaturedProducts = featured
        };
        
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
