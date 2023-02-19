using GraphicsForYouShopApp.Models;
using GraphicsForYouShopApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GraphicsForYouShopApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGraphicsApiService _graphicsApiService;

        public HomeController(ILogger<HomeController> logger, IGraphicsApiService graphicsApiService)
        {
            _logger = logger;
            _graphicsApiService = graphicsApiService;
        }

       
        public async Task<IActionResult> Index()
        {
            try
            {
                var categoryList = await _graphicsApiService.GetCategoryList();
                return View(categoryList);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> OrdersProcess()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Contact()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> About()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}