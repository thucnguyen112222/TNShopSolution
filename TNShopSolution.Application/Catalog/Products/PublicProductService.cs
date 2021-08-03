using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Catalog.Products;
using TNShopSolution.ViewModels.Common;
using TNShopSulotion.Data.EntityFramework;

namespace TNShopSolution.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly TNShopdbContext db;
        public PublicProductService(TNShopdbContext _db)
        {
            db = _db;
        }

        public async Task<List<ProductViewModel>> GetAll(string languageId)
        {
            var query = from p in db.Products
                        join pt in db.ProductTranslations on p.Id equals pt.ProductId
                        join pic in db.ProductInCategories on p.Id equals pic.ProductId
                        join c in db.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            var data = query.Select(x => new ProductViewModel()
               {
                   Id = x.p.Id,
                   Name = x.pt.Name,
                   DateCreated = x.p.DateCreated,
                   Description = x.pt.Description,
                   Details = x.pt.Details,
                   LanguageId = x.pt.LanguageId,
                   OriginalPrice = x.p.OriginalPrice,
                   Price = x.p.Price,
                   SeoAlias = x.pt.SeoAlias,
                   SeoDescription = x.pt.SeoDescription,
                   SeoTitle = x.pt.SeoTitle,
                   Stock = x.p.Stock,
                   ViewCount = x.p.ViewCount,
               }).ToListAsync();
            return await data;
        }

        public async Task<PageViewModel<ProductViewModel>> GetAllCategoryById(string languageId,GetPublicProductPagingRequest request)
        {
            var query = from p in db.Products
                        join pt in db.ProductTranslations on p.Id equals pt.ProductId
                        join pic in db.ProductInCategories on p.Id equals pic.ProductId
                        join c in db.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            int totalRow =  query.Count();
            var data =await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                }).ToListAsync();

            var pageResult = new PageViewModel<ProductViewModel>
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
    }
}
