using System.Net;
using ECommercePlatform.Logic.CommonService;
using ECommercePlatform.Shared.Constants;
using ECommercePlatform.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.DataModel.Migrations;
using Odai.Domain.Extension;
using Odai.Shared.Auth;
using Odai.Shared.Models;

namespace ECommercePlatform.Logic.Services.Category
{
    public class CategoryService : BaseService<Odai.Domain.Entities.Category>, ICategoryService
    {
        private readonly OdaiDbContext _context;

        public CategoryService(OdaiDbContext context) : base(context)
        {
            context = _context;
        }

        public async Task<Response<CategoryModel>> AddEdit([FromBody] CategoryModel model)
        {
            UpladFileModel filePath = new UpladFileModel();

            if (model.ImagePath != null)
            {
                filePath = await Extension.UploadFile(model.ImagePath);
            }

            if (model.Id == null)
            {
                var category = new Odai.Domain.Entities.Category
                {
                    Name = model.Name,
                    FilePath = filePath.FileName,
                    ContentType = filePath.ContentType,
                    CreatonDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now
                };

                await Insert(category);
                await SaveChangesAsync();

                var resultModel = new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(category.FilePath)}"

                };

                return new Response<CategoryModel>(resultModel, HttpStatusCodes.Created, "Category added successfully.");
            }
            else
            {
                var category = await Get(c => c.Id == model.Id).FirstOrDefaultAsync();

                if (category == null)
                {
                    return new Response<CategoryModel>("No Content", HttpStatusCodes.NoContent);
                }
                if (model.ImagePath != null)
                {
                    category.FilePath = filePath.FileName;
                    category.ContentType = filePath.ContentType;
                }
                category.Name = model.Name;
                category.LastUpdateDate = DateTime.Now;

                Update(category);
                await SaveChangesAsync();

                var resultModel = new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(category.FilePath)}"

                };
                return new Response<CategoryModel>(resultModel, HttpStatusCodes.Created, "Category updated successfully.");

            }
        }
        public  async Task<Response< CategoryModel>> GetCategoryById(int id)
        {
            var category = await GetById(id);
            if (category != null)
            {
                var result = new CategoryModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    ImageUrl = $"https://localhost:7036/Images/{Path.GetFileName(category.FilePath)}"
                };
                return new Response<CategoryModel>(result, HttpStatusCodes.OK, ResponseMessages.OperationSucceeded);
            }
            return new Response<CategoryModel>("Setting not found", HttpStatusCodes.NotFound);

        }
    }
}
