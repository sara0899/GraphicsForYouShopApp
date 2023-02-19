using GraphicsForYouShopApi.Data.Enums;
using GraphicsForYouShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GraphicsForYouShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphicController : ControllerBase
    {
        private readonly GraphicsDbContext _graphicsDbContext;

        public GraphicController(GraphicsDbContext graphicsDbContext)
        {
            _graphicsDbContext = graphicsDbContext;
        }

        [HttpGet]
        [Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var categoryList = await _graphicsDbContext.Categories.ToListAsync();
                return StatusCode(StatusCodes.Status200OK, categoryList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCollectionList")]
        public async Task<IActionResult> GetCollectionList()
        {
            try
            {
                var collectionList = await _graphicsDbContext.Collections.ToListAsync();
                return StatusCode(StatusCodes.Status200OK, collectionList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddGraphic")]
        public async Task<IActionResult> AddGraphic([FromBody] Graphic graphic)
        {
            try
            {
                await _graphicsDbContext.Graphics.AddAsync(graphic);
                await _graphicsDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllGraphics")]
        public async Task<IActionResult> GetAllGraphics()
        {
            try
            {
                var graphicList = await _graphicsDbContext.Graphics.ToListAsync();
                return StatusCode(StatusCodes.Status200OK, graphicList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPost]
        [Route("CreateCollection")]
        public async Task<IActionResult> CreateCollection([FromBody] Collection collection)
        {
            try
            {
                await _graphicsDbContext.Collections.AddAsync(collection);
                await _graphicsDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCollection/{id}")]
        public async Task<IActionResult> GetCollection(int id)
        {
            try
            {
                var collection = _graphicsDbContext.Collections.Where(x => x.Id == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, collection);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditCollection")]
        public async Task<IActionResult> EditCollection([FromBody] Collection collection)
        {
            try
            {
                var newCollection = _graphicsDbContext.Collections.Where(u => u.Id == collection.Id).FirstOrDefault();
                if (newCollection != null)
                {
                    newCollection.Name = collection.Name;
                    newCollection.Description = collection.Description;
                }
                await _graphicsDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }


        [HttpGet]
        [Route("GetGraphicById/{id}")]
        public async Task<IActionResult> GetGraphicById(int id)
        {
            try
            {
                var graphic = await _graphicsDbContext.Graphics.Where(x => x.Id == id).Select(graphic => new GraphicViewModel()
                {
                    Id = graphic.Id,
                    Name = graphic.Name,
                    Description = graphic.Description,
                    Price = graphic.Price,
                    PromotionPrice = graphic.PromotionPrice,
                    CategoryId = graphic.CategoryId,
                    Category = graphic.Category,
                    Collection = graphic.Collection,
                    CollectionId = graphic.CollectionId,
                    Availability = graphic.Availability,
                    MainPictureUrl = graphic.MainPictureUrl,
                    PicturesList = graphic.graphicPictures.Select(g => new Picture()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Url = g.Url
                    }).ToList()

                }).FirstOrDefaultAsync();
                return StatusCode(StatusCodes.Status200OK, graphic);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetGraphicsByCategoryId/{id}")]
        public async Task<IActionResult> GetGraphicsByCategoryId(int id)
        {
            try {
                var graphicList = await _graphicsDbContext.Graphics.Where(x => x.Category.Id == id && x.Availability == GraphicAvailability.Dostępna).ToListAsync();
                return StatusCode(StatusCodes.Status200OK, graphicList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message});
            }
        }

        [HttpGet]
        [Route("GetGraphicsByCollectionId/{id}")]
        public async Task<IActionResult> GetGraphicsByCollectionId(int id)
        {
            try { 
                var collectionId = await _graphicsDbContext.Graphics.Where(x => x.Id == id)
                                    .Select(x => x.CollectionId).FirstOrDefaultAsync();
                var graphicList = await _graphicsDbContext.Graphics
                                    .Where(x => x.Collection.Id == collectionId && x.Availability == GraphicAvailability.Dostępna).ToListAsync();
                return StatusCode(StatusCodes.Status200OK, graphicList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message});
            }
        }

        [HttpGet]
        [Route("GetGraphicsFromCollection/{id}")]
        public async Task<IActionResult> GetGraphicsFromCollection(int id)
        {
            try { 
                var graphicList = await _graphicsDbContext.Graphics
                                .Where(x => x.Collection.Id == id && x.Availability == GraphicAvailability.Dostępna).ToListAsync();
                return StatusCode(StatusCodes.Status200OK, graphicList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message});
            }
        }

        [HttpPost]
        [Route("AddPromotionToAll")]
        public async Task<IActionResult> AddPromotionToAll([FromBody] Promotion promotion)
        {
            try 
            { 
                var promotionList = _graphicsDbContext.Promotions.ToList();
                var graphicList = _graphicsDbContext.Graphics.Where(graphic => graphic.Availability == GraphicAvailability.Dostępna).ToList();
                foreach (var graphic in graphicList)
                {
                    bool failure = false;
                    var graphicInPromotions = promotionList.FindAll(g => g.GraphicId == graphic.Id).ToList();
                    if (graphicInPromotions.Any())
                    {
                        foreach (var inPromotion in graphicInPromotions)
                        {
                            if (promotion.DateFrom > inPromotion.DateTo || promotion.DateTo < inPromotion.DateFrom)
                                failure = false;
                            else
                                failure = true;
                        }
                    }

                    if (failure == false)
                    {
                        var newPromotion = new Promotion()
                        {
                            Name = promotion.Name,
                            GraphicId = graphic.Id,
                            DateFrom = promotion.DateFrom,
                            DateTo = promotion.DateTo,
                            Percentage = promotion.Percentage
                        };
                        await _graphicsDbContext.Promotions.AddAsync(newPromotion);
                        await _graphicsDbContext.SaveChangesAsync();
                    }
                }
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPost]
        [Route("AddPromotionToGraphic")]
        public async Task<IActionResult> AddPromotionToGraphic(Promotion promotion)
        {
            try { 
                var promotionList = _graphicsDbContext.Promotions.ToList();
                bool failure = false;
                var graphicInPromotions = promotionList.FindAll(g => g.GraphicId == promotion.GraphicId).ToList();
                if (graphicInPromotions.Any()) {
                    foreach (var inPromotion in graphicInPromotions)
                    {
                        if (promotion.DateFrom > inPromotion.DateTo || promotion.DateTo < inPromotion.DateFrom)
                            failure = false;
                        else
                            failure = true;
                    }
                }

                if (failure == false)
                {
                    await _graphicsDbContext.Promotions.AddAsync(promotion);
                    await _graphicsDbContext.SaveChangesAsync();
                }
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }
     
        [HttpGet]
        [Route("DeletePicture/{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var deleteGraphic = _graphicsDbContext.GraphicPictures.Where(graphic => graphic.Id == id).FirstOrDefault();
            _graphicsDbContext.GraphicPictures.Remove(deleteGraphic);
            _graphicsDbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("Search/{key}")]
        public IActionResult Search(string key)
        {
            try 
            { 
                var graphicList = _graphicsDbContext.Graphics
                                    .Where(g => g.Name.Contains(key) || g.Description.Contains(key)).ToList();
                return StatusCode(StatusCodes.Status200OK, graphicList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message});
            }
        }

        [HttpGet]
        [Route("GetPromotions")]
        public async Task<IActionResult> GetPromotions()
        {
            try
            {
                var graphicPromotions = await _graphicsDbContext.Promotions.ToListAsync();
                return StatusCode(StatusCodes.Status200OK, graphicPromotions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> Edit(Graphic graphic)
        {
            try
            {
                var newGraphic = _graphicsDbContext.Graphics.Where(u => u.Id == graphic.Id).FirstOrDefault();
                if (newGraphic != null)
                {
                    newGraphic.Name = graphic.Name;
                    newGraphic.Description = graphic.Description;
                    newGraphic.Price = graphic.Price;
                    newGraphic.PromotionPrice = graphic.Price;
                    newGraphic.CategoryId = graphic.CategoryId;
                    newGraphic.CollectionId = graphic.CollectionId;
                    if (graphic.MainPictureUrl != null)
                    {
                        newGraphic.MainPictureUrl = graphic.MainPictureUrl;
                    }
                    newGraphic.Availability = GraphicAvailability.Dostępna;
                    newGraphic.graphicPictures = graphic.graphicPictures;
                    await _graphicsDbContext.SaveChangesAsync();
                }
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { messageError = ex.Message });
            }
        }

    }
}
