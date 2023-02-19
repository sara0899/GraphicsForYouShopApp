using GraphicsForYouShopApp.Data.Enums;
using GraphicsForYouShopApp.Models;
using GraphicsForYouShopApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Dynamic;

namespace GraphicsForYouShopApp.Controllers
{
    public class GraphicController : Controller
    {
        private readonly IGraphicsApiService _graphicsForYouApiService;

        public GraphicController(IGraphicsApiService graphicsForYouApiService)
        {
            _graphicsForYouApiService = graphicsForYouApiService;
        }

        public async Task<IActionResult> GraphicView(int id)
        {
            try { 
                dynamic dynamicShopObject = new ExpandoObject();
                var product = await _graphicsForYouApiService.GetGraphicById(id);
                dynamicShopObject.product = product;
                var collection = await _graphicsForYouApiService.GetGraphicsByCollectionId(id);
                dynamicShopObject.collection = collection;
                var enums = Enum.GetValues(typeof(GraphicAvailability));
                dynamicShopObject.enums = enums;
                return View(dynamicShopObject);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categoryList = new SelectList(await _graphicsForYouApiService.GetCategoryList(), "Id", "Name");
                ViewBag.categoryList = categoryList.Items;
                var collectionList = new SelectList(await _graphicsForYouApiService.GetCollectionList(), "Id", "Name");
                ViewBag.collectionList = collectionList.Items;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> AddPromotionToAll(Promotion promotion)
        {
            try 
            { 
                await _graphicsForYouApiService.AddPromotionToAll(promotion);
                return RedirectToAction("Promotions");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> AddPromotionToChosen(Promotion promotion, string graphics)
        {
            try { 
                var graphicIdList = JsonConvert.DeserializeObject<List<int>>(graphics);
                foreach (var graphicId in graphicIdList)
                {
                    promotion.GraphicId = graphicId;
                    await _graphicsForYouApiService.AddPromotionToGraphic(promotion);
                }
                return RedirectToAction("Promotions");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> Create(GraphicViewModel graphic)
        {
            try
            {
                string picturesFolderName = "wwwroot/Images/";
                if (ModelState.IsValid)
                {
                    if (graphic.MainPicture != null)
                    {
                        string pictureFileName = $"{Guid.NewGuid()}-{graphic.MainPicture.FileName}";
                        graphic.MainPictureUrl = pictureFileName;
                        await GetPicturePath(picturesFolderName, graphic.MainPicture, pictureFileName);
                    }

                    graphic.PicturesList = new List<Picture>();
                    if (graphic.Pictures != null)
                    {
                        foreach (IFormFile file in graphic.Pictures)
                        {
                            string pictureFileName = $"{Guid.NewGuid()}-{file.FileName}";
                            Picture picture = new Picture(){
                                Name = pictureFileName,
                                Url = await GetPicturePath(picturesFolderName, file, pictureFileName)
                            };
                            graphic.PicturesList.Add(picture);
                        }
                    }

                    await _graphicsForYouApiService.AddGraphic(graphic);
                    return RedirectToAction("GraphicsManagement");
                }
                else
                {
                    var categoryList = new SelectList(await _graphicsForYouApiService.GetCategoryList(), "Id", "Name");
                    ViewBag.categoryList = categoryList.Items;
                    var collectionList = new SelectList(await _graphicsForYouApiService.GetCollectionList(), "Id", "Name");
                    ViewBag.collectionList = collectionList.Items;
                    return View(graphic);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task<string> GetPicturePath(string picturesFolderName, IFormFile file, string pictureFileName)
        {
            await file.CopyToAsync(new FileStream($"{picturesFolderName}{pictureFileName}", FileMode.Create));
            return picturesFolderName;
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> GraphicsManagement()
        {
            try
            {
                var graphicList = await _graphicsForYouApiService.GetAllGraphics();
                return View(graphicList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> EditGraphic(int id)
        {
            try
            {
                    ViewBag.categoryList = new SelectList(await _graphicsForYouApiService.GetCategoryList(), "Id", "Name");


                var categories = await _graphicsForYouApiService.GetCategoryList();
                var categoryList = new List<Category>();
                foreach (var category in categories)
                {
                    categoryList.Add(category);
                }
                ViewBag.categoryList = categoryList;


                var collections = await _graphicsForYouApiService.GetCollectionList();
                var collectionList = new List<Collection>();
                foreach (var collection in collections)
                {
                    collectionList.Add(collection);
                }
                ViewBag.collectionList = collectionList;


                var existingGraphic = await _graphicsForYouApiService.GetGraphicById(id);

                return View(existingGraphic);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        public void Delete(int id)
        {
            _graphicsForYouApiService.DeletePicture(id);
        }

        [Authorize(Roles = "Designer")]
        public async Task<IActionResult> Edit(GraphicViewModel graphic)
        {
            if (graphic.MainPicture != null)
            {
                string folder = "wwwroot/Images/";
                var fName = Guid.NewGuid().ToString() + "_" + graphic.MainPicture.FileName;
                graphic.MainPictureUrl = fName;

                await GetPicturePath(folder, graphic.MainPicture, fName);

            }

            if (graphic.Pictures != null)
            {
                string folder = "wwwroot/Images/";

                graphic.PicturesList = new List<Picture>();

                foreach (var file in graphic.Pictures)
                {
                    var fName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var picture = new Picture()
                    {
                        Name = fName,
                        Url = await GetPicturePath(folder, file, fName)
                    };
                    graphic.PicturesList.Add(picture);
                }
            }
            await _graphicsForYouApiService.Edit(graphic);
            return RedirectToAction("GraphicsManagement");

        }


        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> CollectionManagement()
        {
            try
            {
                var collectionList = new SelectList(await _graphicsForYouApiService.GetCollectionList(), "Id", "Name");
                return View(collectionList.Items);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        public async Task<IActionResult> CreateCollection()
        {
            return View();
        }

        [Authorize(Roles = "Designer")]
        [HttpPost]
        public async Task<IActionResult> CreateCollection(Collection collection)
        {
            try
            {
                await _graphicsForYouApiService.CreateCollection(collection);
                return RedirectToAction("CollectionManagement");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> Collection(int id)
        {
            try
            {
                var existingCollection = await _graphicsForYouApiService.GetCollection(id);
                return View(existingCollection);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        public async Task<IActionResult> EditCollection(Collection collection)
        {
            try
            {
                await _graphicsForYouApiService.EditCollection(collection);
                return RedirectToAction("CollectionManagement");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Search(string key, string categories, string collections)
        {
            try { 
                var graphicIdByKeyList = new List<int>();
                var graphicList = new List<Graphic>();
                var graphicByCategoryId = new List<Graphic>();
                var graphicIdByCategoryList = new List<int>();
                var graphicIdByCollectionList = new List<int>();
                var graphicByCollectionId = new List<Graphic>();

                if (key != null)
                {
                    var graphicListByKey = await _graphicsForYouApiService.Search(key);
                    if (graphicListByKey.Count != 0)
                    {
                        foreach (var graphic in graphicListByKey)
                        {
                            graphicIdByKeyList.Add(graphic.Id);
                        }
                    }
                }

                if (categories != null)
                {
                    var categoryListDeserialize = JsonConvert.DeserializeObject<List<int>>(categories);

                    foreach (var categoryId in categoryListDeserialize)
                    {
                        var graphicListByCategoryId = await _graphicsForYouApiService.GetGraphicsByCategoryId(categoryId);
                        if (graphicListByCategoryId != null)
                        {
                            foreach (var graphic in graphicListByCategoryId)
                            {
                                graphicByCategoryId.Add(graphic);
                                graphicIdByCategoryList.Add(graphic.Id);
                            }
                        }
                    }
                }

                if (collections != null)
                {
                    var collectionListDeserialize = JsonConvert.DeserializeObject<List<int>>(collections);

                    foreach (var collectionId in collectionListDeserialize)
                    {
                        var graphicListByCollectionId = await _graphicsForYouApiService.GetGraphicsFromCollection(collectionId);
                        if (graphicListByCollectionId != null)
                        {
                            foreach (var graphic in graphicListByCollectionId)
                            {
                                graphicByCollectionId.Add(graphic);
                                graphicIdByCollectionList.Add(graphic.Id);
                            }
                        }
                    }
                }

                var listOfLists = new List<List<int>>() { graphicIdByCollectionList, graphicIdByCategoryList, graphicIdByKeyList };
                var listOfGraphicLists = new List<List<int>>();

                foreach (var list in listOfLists)
                {
                    if (list.Count != 0)
                    {
                        listOfGraphicLists.Add(list);
                    }
                }

                if (listOfGraphicLists.Count != 0)
                {

                    var graphicIdList = listOfGraphicLists.Aggregate<IEnumerable<int>>((previousGraphicList, nextGraphicList) =>
                                             previousGraphicList.Intersect(nextGraphicList)).ToList();

                    foreach (int id in graphicIdList)
                    {
                        var graphic = await _graphicsForYouApiService.GetSingleGraphic(id);
                        if (graphic != null)
                        {
                            graphicList.Add(graphic);
                        }
                    }
                }

                dynamic dynamicShopObject = new ExpandoObject();
                var categoryList = await _graphicsForYouApiService.GetCategoryList();
                dynamicShopObject.categoryList = categoryList;
                var collectionList = await _graphicsForYouApiService.GetCollectionList();
                dynamicShopObject.collectionList = collectionList;
                dynamicShopObject.graphicList = graphicList;

                return View(dynamicShopObject);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> Promotions()
        {
            try 
            { 
                var promotionList = await _graphicsForYouApiService.GetPromotions();
                return View(promotionList);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Designer")]
        [HttpGet]
        public async Task<IActionResult> CreatePromotion()
        {
            try
            {
                var graphicList = await _graphicsForYouApiService.GetAllGraphics();
                ViewBag.graphicList = graphicList;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
