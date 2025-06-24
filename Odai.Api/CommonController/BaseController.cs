using ECommercePlatform.Logic.ICommonService;
using ECommercePlatform.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.DataModel;
using Odai.Domain.Common;
using Odai.Logic.Common;
using Odai.Shared.Auth;

namespace ECommercePlatform.Api.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity, TService> : ControllerBase
        where TEntity : BaseEntity 
        where TService : IBaseService<TEntity>
    {
        protected readonly TService _service;
        public BaseController(TService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public virtual async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll().ToListAsync();
            return Ok(new Response<List<TEntity>>(data, 200, ResponseMessages.OperationSucceeded));
        }

        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);
            if (data != null)
            {
                return Ok(new Response<TEntity>(data,200,ResponseMessages.OperationSucceeded));
            }
            return NotFound(new Response<TEntity>(ResponseMessages.NotFound,404));
        }

        [HttpGet("GetWithPagination")]
        public virtual async Task<IActionResult> GetWithPagination(int pageIndex, int pageSize)
        {
            var result = await _service.GetWithPagination(pageIndex, pageSize);
            return Ok(new Response<object>
                (new { Data = result.Data, TotalCount = result.RecordsTotal }
                ,200,ResponseMessages.OperationSucceeded));
        }
        [HttpGet("GetByParams")]
        public virtual IActionResult GetByParams([FromQuery] Dictionary<string, string> filters)
        {
            var convertedFilters = filters.ToDictionary(f => f.Key, f => (object)f.Value!);
            var result = _service.GetByDynamicParams(convertedFilters).ToList();
            return Ok(new Response<List<TEntity>>(result,200,ResponseMessages.OperationSucceeded));
        }
        [HttpPost("Insert")]
        public virtual async Task<IActionResult> Insert([FromBody] TEntity entity)
        {
            try
            {
                await _service.Insert(entity);
                await _service.SaveChangesAsync();
                return Ok(new Response<TEntity>(entity, 201, ResponseMessages.SavedSuccess));
            }
            catch (Exception)
            {
                return StatusCode(500, new Response<TEntity>(ResponseMessages.GenericError, 500));
            }
        }
        [HttpPut("Update")]
        public virtual async Task<IActionResult>Update([FromBody]TEntity entity)
        {
            try
            {
                _service.Update(entity);
                await _service.SaveChangesAsync();
                return Ok(new Response<TEntity>(entity, 200, ResponseMessages.UpdatedSuccess));
            }
            catch (Exception)
            {
                return StatusCode(500, new Response<TEntity>(ResponseMessages.GenericError, 500));
            }
        }
        [HttpDelete("SoftDelete")]
        public virtual async Task <IActionResult> SoftDelete(int id)
        {
            var entity=await _service.GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = 1;
                 _service.Update(entity);
                await _service.SaveChangesAsync();
                return Ok(new Response<string>($"{typeof(TEntity).Name} soft deleted.", 200, ResponseMessages.DeletedSuccess));
            }
            return NotFound(new Response<string>(ResponseMessages.NotFound, 404));
        }
        [HttpDelete("HardDelete")]
        public virtual async Task<IActionResult> HardDelete(int id)
        {
            var entity = await _service.GetById(id);
            if (entity != null)
            {
                await _service.Delete(entity);
                await _service.SaveChangesAsync();
                return Ok(new Response<string>($"{typeof(TEntity).Name} hard deleted.", 200, ResponseMessages.DeletedSuccess));
            }
            return NotFound(new Response<string>(ResponseMessages.NotFound, 404));

        }
    }
}
