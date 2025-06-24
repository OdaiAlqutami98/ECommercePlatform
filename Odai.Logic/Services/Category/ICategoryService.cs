using ECommercePlatform.Logic.ICommonService;
using ECommercePlatform.Shared.Models;
using Odai.Shared.Auth;
using Odai.Shared.Models;

namespace ECommercePlatform.Logic.Services.Category
{
   public interface ICategoryService : IBaseService<Odai.Domain.Entities.Category>
    {
        Task<Response<CategoryModel>> AddEdit(CategoryModel model);
        Task<Response<CategoryModel>> GetCategoryById(int id);
    }
}
