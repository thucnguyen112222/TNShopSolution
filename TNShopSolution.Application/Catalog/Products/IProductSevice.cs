using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Catalog.ProductImage;
using TNShopSolution.ViewModels.Catalog.Products;
using TNShopSolution.ViewModels.Common;

namespace TNShopSolution.Application.Catalog.Products
{
    public interface IProductSevice
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductEditRequet request);
        Task<bool> UpdatePrice(int ProductID, decimal newPrice);
        Task AddViewCount(int ProductId);
        Task<int> Delete(int ProductId);
        bool UpdateStock(int ProductID, int Quantity);
        Task<ProductViewModel> GetById(int ProductId,string LanguageId);
        List<ProductViewModel> GetAll();
        PageResult<ProductViewModel> GetAllPaging(GetManagerProductPagingRequest request);
        Task<int> AddImage(int ProductId, ProductImageCreateRequest productImages);
        Task<int> UpdateImage( int ImgId, ProductImageUpdateRequest productImages);
        Task<int> DeleteImage(int ImgId);
        Task<ProductImageViewModel> GetImageById(int Id);
        Task<List<ProductImageViewModel>> GetListImages(int ProductId);
        Task<PageResult<ProductViewModel>> GetAllCategoryById(string languageId, GetPublicProductPagingRequest request);

    }
}
