using GraphicsForYouShopApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphicsForYouShopApp.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly IGraphicsApiService _graphicsApiService;

        public CategoryViewComponent(IGraphicsApiService graphicsApiService)
        {
            _graphicsApiService = graphicsApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var categories = await _graphicsApiService.GetCategoryList();
                return View(categories);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}