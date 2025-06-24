using ECommercePlatform.Logic.ICommonService;
using Odai.Domain.Enums;
using Odai.Shared.Auth;
using Odai.Shared.Models;

namespace ECommercePlatform.Logic.Services.Product
{
    public interface IProductService:IBaseService<Odai.Domain.Entities.Product>
    {
        Task<Response<ProductModel>> AddEdit(ProductModel model);

        Task<Response<List<ProductModel>>> GetProductBySort(int? categoryId, ProductSortOption sortBy);
        Task<Response<ProductModel>> GetProductById(int id);
    }
}
