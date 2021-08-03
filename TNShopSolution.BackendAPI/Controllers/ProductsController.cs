
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TNShopSolution.Application.Catalog.Products;
using TNShopSolution.ViewModels.Catalog.ProductImage;
using TNShopSolution.ViewModels.Catalog.Products;

namespace TNShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManagerProductSevice _managerProductService;
        public ProductsController(IPublicProductService publicProductService, IManagerProductSevice managerProductSevice)
        {
            _managerProductService = managerProductSevice;
            _publicProductService = publicProductService;
        }
        //https://localhost:api/products?index=1&pagesize=10&categoryid=1
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var product = await _publicProductService.GetAllCategoryById(languageId, request);
            return Ok(product);
        }
        //https://localhost:api/product/1      
        [HttpGet("{ProductId}/{languageId}")]
        public async Task<IActionResult> GetById(int ProductId, string languageId)
        {
            var product = await _managerProductService.GetById(ProductId, languageId);
            if (product == null)
            {
                return BadRequest("canot find product");
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _managerProductService.Create(request);
            if (productId == 0)
                return BadRequest();
            var product = await _managerProductService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductEditRequet request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _managerProductService.Update(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _managerProductService.Delete(productId);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPatch("{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            var result = await _managerProductService.UpdatePrice(id, newPrice);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _managerProductService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();
            var image = await _managerProductService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }
        [HttpPost("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resutl = await _managerProductService.UpdateImage(imageId, request);
            if (resutl == 0)
                return BadRequest();
            return Ok();
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resutl = await _managerProductService.DeleteImage(imageId);
            if (resutl == 0)
                return BadRequest();

            return Ok();
        }
        [HttpGet("{productId}/images/{ImageId}")]
        public async Task<IActionResult> GetImageById(int productId, int ImageId)
        {
            var img = await _managerProductService.GetImageById(ImageId);
            if (img == null)
            {
                return BadRequest("canot find product");
            }
            return Ok(img);
        }
    }
}
