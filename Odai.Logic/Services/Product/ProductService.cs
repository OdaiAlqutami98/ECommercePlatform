using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommercePlatform.Logic.CommonService;
using ECommercePlatform.Shared.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Entities;
using Odai.Domain.Enums;
using Odai.Domain.Extension;
using Odai.Logic.Manager;
using Odai.Shared.Auth;
using Odai.Shared.Models;

namespace ECommercePlatform.Logic.Services.Product
{
    public class ProductService : BaseService<Odai.Domain.Entities.Product>, IProductService
    {
        public ProductService(OdaiDbContext context):base(context) 
        {
        }

        public async Task<Response<ProductModel>> AddEdit(ProductModel model)
        {
            UpladFileModel filePath = new UpladFileModel();
            if (model.ImagePath != null)
            {

                filePath = await Extension.UploadFile(model.ImagePath);
            }
            if (model.Id == null)
            {
                var product = new Odai.Domain.Entities.Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Discount = model.Discount,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Favorite = model.Favorite,
                    Stock = model.Stock,
                    FilePath = filePath.FileName,
                    ContentType = filePath.ContentType,
                    Status = (ProductStatus)model.Status,
                    CreatonDate = DateTime.Now,

                };
                await Insert(product);
                await SaveChangesAsync();
                var resultModel = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Favorite = product.Favorite,
                    Stock = product.Stock,
                    Discount = product.Discount,
                    Status=(int)product.Status,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(product.FilePath)}"

                };

                return new Response<ProductModel>(resultModel, HttpStatusCodes.Created, ResponseMessages.SavedSuccess);
            }
            else
            {
                var product = await Get(c => c.Id == model.Id).FirstOrDefaultAsync();

                if (product == null)
                {
                    return new Response<ProductModel>("No Content", HttpStatusCodes.NoContent);
                }
                if (model.ImagePath != null)
                {
                    product.FilePath = filePath.FileName;
                    product.ContentType = filePath.ContentType;
                }


                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.Favorite = model.Favorite;
                product.Stock = model.Stock;
                product.Discount = model.Discount;
                product.Status=(ProductStatus)model.Status;
                product.LastUpdateDate = DateTime.Now;

                Update(product);
                await SaveChangesAsync();

                var resultModel = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Favorite = product.Favorite,
                    Stock = product.Stock,
                    Discount = product.Discount,
                    Status = (int)product.Status,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(product.FilePath)}"

                };


                return new Response<ProductModel>(resultModel, HttpStatusCodes.Created, ResponseMessages.UpdatedSuccess);
            }
        }

        public async Task<Response<List<ProductModel>>> GetProductBySort(int? categoryId, ProductSortOption sortBy)
        {
            var productsQuery = _context.Product.AsQueryable();

            if (categoryId != null)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);

            }
            var sortedProducts = sortBy switch
            {
                ProductSortOption.Popular => productsQuery.OrderByDescending(p => p.TotalSold),
                ProductSortOption.PriceHighToLow => productsQuery.OrderByDescending(p => p.Price),
                ProductSortOption.PriceLowToHigh => productsQuery.OrderBy(p => p.Price),
                //ProductSortOption.TopRated=>products.OrderByDescending(p=>p.Rating)
                ProductSortOption.Newest => productsQuery.OrderByDescending(p => p.CreatonDate),
                _ => productsQuery
            };
            var products = await sortedProducts.ToListAsync();
            var result = products.Select(p => new ProductModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Status = (int)p.Status,
                Stock = p.Stock,
                Discount = p.Discount,
                ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(p.FilePath)}"

            }).ToList();
            return new Response<List<ProductModel>>(result, HttpStatusCodes.OK);
        }
        public async Task<Response<ProductModel>> GetProductById(int id)
        {
            var product = await GetById(id);
            if (product != null)
            {
                var result = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description= product.Description,
                    Discount = product.Discount,
                    CategoryId=product.CategoryId,
                    Price=product.Price,
                    Stock=product.Stock,
                    Status=(int)product.Status,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(product.FilePath)}"
                };
                return new Response<ProductModel>(result, HttpStatusCodes.OK, ResponseMessages.OperationSucceeded);
            }
            return new Response<ProductModel>("Setting not found", HttpStatusCodes.NotFound);

        }
    }
}
