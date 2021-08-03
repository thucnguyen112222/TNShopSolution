using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TNShopSolution.Application.Common;
using TNShopSolution.Utilities.Utilities;
using TNShopSolution.ViewModels.Catalog.ProductImage;
using TNShopSolution.ViewModels.Catalog.Products;
using TNShopSolution.ViewModels.Common;
using TNShopSulotion.Data.Entities;
using TNShopSulotion.Data.EntityFramework;

namespace TNShopSolution.Application.Catalog.Products
{
    public class ManagerProductSevice : IManagerProductSevice
    {
        private readonly TNShopdbContext db;

        private readonly IStorageService _storageService;
        public ManagerProductSevice(TNShopdbContext context, IStorageService storageService)
        {
            db = context;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int ProductId, ProductImageCreateRequest request)
        {
            var productImg = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = ProductId,
                SortOrder = request.SortOrder,
            };
            if (request.ImageFile != null)
            {
                productImg.ImagePath = await SaveFile(request.ImageFile);
                productImg.FileSize = request.ImageFile.Length;
            }
            db.ProductImages.Add(productImg);
             await db.SaveChangesAsync();
            return productImg.Id;
        }
        public async Task<int> DeleteImage(int ImgId)
        {
            var productImage =await db.ProductImages.FindAsync(ImgId);
            if (productImage == null)
                throw new TNShopException($"cannot find image with id {ImgId}");
            db.ProductImages.Remove(productImage);
            return await db.SaveChangesAsync();

        }
        public async Task<List<ProductImageViewModel>> GetListImages(int ProductId)
        {
            return await db.ProductImages.Where(m => m.ProductId == ProductId).Select(i => new ProductImageViewModel()
            {
                Caption = i.Caption,
                DateCreated = i.DateCreated,
                FileSize = i.FileSize,
                Id = i.Id,
                ImagePath = i.ImagePath,
                IsDefault = i.IsDefault,
                SortOrder = i.SortOrder,
                ProductId = i.ProductId,
            }).ToListAsync();
        }
        public async Task<int> UpdateImage( int ImgId, ProductImageUpdateRequest request)
        {
            var productImg = await db.ProductImages.FindAsync(ImgId);
            if (productImg == null)
                throw new TNShopException($"cannot find image with id {ImgId}");
            if (request.ImageFile != null)
            {
                productImg.ImagePath = await SaveFile(request.ImageFile);
                productImg.FileSize = request.ImageFile.Length;
            }
            db.ProductImages.Update(productImg);
            return await db.SaveChangesAsync();
        }
        public async Task AddViewCount(int ProductId)
        {
            var product = await db.Products.FindAsync(ProductId);
            product.ViewCount++;
            await db.SaveChangesAsync();
        }
        public async Task<int> Create(ProductCreateRequest r)
        {
            var product = new Product()
            {
                Price = r.Price,
                OriginalPrice = r.OriginalPrice,
                Stock = r.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = r.Name,
                        Description = r.Description,
                        Details = r.Details,
                        SeoDescription = r.SeoDescription,
                        SeoAlias = r.SeoAlias,
                        SeoTitle = r.SeoTitle,
                        LanguageId = r.LanguageId,
                    }
                }
            };

            if (r.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail",
                        DateCreated = DateTime.Now,
                        ImagePath = await this.SaveFile(r.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1,
                        FileSize = r.ThumbnailImage.Length,
                    }
                };
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product.Id;
        }
        public async Task<int> Delete(int ProductId)
        {
            var product = await db.Products.FindAsync(ProductId);
            if (product == null)
            {
                throw new TNShopException($"Cannot find a product {ProductId}");
            }
            var img = db.ProductImages.Where(m => m.ProductId == ProductId);
            foreach (var item in img)
            {
                await _storageService.DeleteFile(item.ImagePath);

            }
            db.Products.Remove(product);
            return db.SaveChanges();

        }
        public List<ProductViewModel> GetAll()
        {
            throw new NotImplementedException();
        }
        public PageViewModel<ProductViewModel> GetAllPaging(GetManagerProductPagingRequest request)
        {
            var query = from p in db.Products
                        join pt in db.ProductTranslations on p.Id equals pt.ProductId
                        join pic in db.ProductInCategories on p.Id equals pic.ProductId
                        join c in db.Categories on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            if (!string.IsNullOrEmpty(request.Keywork))
            {
                query = query.Where(m => m.pt.Name.Contains(request.Keywork));
            }
            if (request.CategoryIDs.Count > 0)
            {
                query = query.Where(p => request.CategoryIDs.Contains(p.pic.CategoryId));
            }
            int totalRow = query.Count();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
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
                }).ToList();

            var pageResult = new PageViewModel<ProductViewModel>
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }
        public async Task<ProductViewModel> GetById(int ProductId, string LanguageId)
        {
            var product = await db.Products.FindAsync(ProductId);
            var productTranlation = await db.ProductTranslations
                .FirstOrDefaultAsync(m => m.ProductId == ProductId && m.LanguageId == LanguageId);
            var ProductViewModel = new ProductViewModel()
            {
                Id = product.Id,
                DateCreated = product.DateCreated,
                Description = productTranlation != null ? productTranlation.Description : null,
                LanguageId = productTranlation.LanguageId,
                Details = productTranlation != null ? productTranlation.Details : null,
                Name = productTranlation != null ? productTranlation.Name : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranlation != null ? productTranlation.SeoAlias : null,
                SeoDescription = productTranlation != null ? productTranlation.SeoDescription : null,
                SeoTitle = productTranlation != null ? productTranlation.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
            };
            return ProductViewModel;
        }
        public async Task<int> Update(ProductEditRequet request)
        {
            var product = db.Products.Find(request.Id);
            var productTranlation = db.ProductTranslations.FirstOrDefault(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            if (product == null || productTranlation == null)
            {
                throw new TNShopException($"Cannot find a product with id{request.Id}");
            }
            productTranlation.Name = request.Name;
            productTranlation.SeoAlias = request.SeoAlias;
            productTranlation.Description = request.Description;
            productTranlation.SeoDescription = request.SeoDESC;
            productTranlation.SeoTitle = request.SeoTitle;
            productTranlation.Details = request.Details;

            if (request.ThumbnailImage != null)
            {
                var img = db.ProductImages.FirstOrDefault(m => m.IsDefault == true && m.ProductId == request.Id);
                if (img != null)
                {
                    img.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    img.FileSize = request.ThumbnailImage.Length;
                    db.ProductImages.Update(img);
                }

            }

            db.Entry(productTranlation).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public async Task<bool> UpdatePrice(int ProductID, decimal newPrice)
        {
            var product = await db.Products.FindAsync(ProductID);
            if (product == null)
            {
                throw new TNShopException($"Cannot find a product with id{ProductID}");
            }
            product.Price = newPrice;
            db.Entry(product).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        public bool UpdateStock(int ProductID, int Quantity)
        {
            var product = db.Products.Find(ProductID);
            if (product == null)
            {
                throw new TNShopException($"Cannot find a product with id{ProductID}");
            }
            product.Stock = Quantity;
            db.Entry(product).State = EntityState.Modified;
            return db.SaveChanges() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFile(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ProductImageViewModel> GetImageById(int Id)
        {
            var Image =  await db.ProductImages.FindAsync(Id);
            if(Image != null)
            {
                ProductImageViewModel viewModel = new ProductImageViewModel()
                {
                    Caption = Image.Caption,
                    DateCreated = Image.DateCreated,
                    FileSize = Image.FileSize,
                    Id = Image.Id,
                    ImagePath = Image.ImagePath,
                    IsDefault = Image.IsDefault,
                    SortOrder = Image.SortOrder,
                    ProductId = Image.ProductId,
                };
                return viewModel;
            }
            else
            {
                throw new TNShopException("cannot find with id {Id}");
            }
            
        }
    }
}
