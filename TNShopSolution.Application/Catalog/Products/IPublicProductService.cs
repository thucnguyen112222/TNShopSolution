using System.Collections.Generic;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Catalog.Products;
using TNShopSolution.ViewModels.Common;

namespace TNShopSolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageViewModel<ProductViewModel>> GetAllCategoryById(string languageId,GetPublicProductPagingRequest request);
        Task<List<ProductViewModel>> GetAll(string languageId);
    }
}
