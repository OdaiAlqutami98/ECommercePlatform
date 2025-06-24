using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Domain.Entities;
using Odai.Logic.Manager;
using Odai.Shared.Auth;
using Odai.Shared.Models;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingManager _ratingManager;
        public RatingController(RatingManager ratingManager)
        {
            _ratingManager = ratingManager;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<TableResponse<Rating>> GetAll(int pageIndex,int pageSize)
        {
            var query  = _ratingManager.GetAll().Include(r => r.Product);
            var length = query.Count();
            var rating = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TableResponse<Rating>(rating, length);
           
        }
        //[HttpGet]
        //[Route("GetById")]
        //public async Task<IActionResult>GetById(int id)
        //{
        //    var rating=await _ratingManager.GetById(id);
        //    if (rating is not null)
        //    {
        //        return Ok(rating);
        //    }
        //    return NotFound();
        //}
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit(RatingModel model)
        {
            if (model.Id is null)
            {
                Rating rating =new Rating();
                rating.Value = model.Value;
                rating.ClientId = model.ClientId;
                rating.ProductId = model.ProductId;
                rating.CreatonDate=DateTime.Now;
                await _ratingManager.Insert(rating);
                await _ratingManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Rating Added successfully." });
            }
           else
            {
                var rating = await _ratingManager.Get().FirstOrDefaultAsync(r=>r.Id==model.Id);
                if (rating is not null)
                {
                    rating.Value = model.Value; 
                    rating.ClientId = model.ClientId;
                    rating.ProductId = model.ProductId;
                    rating.LastUpdateDate = DateTime.Now;
                    _ratingManager.Update(rating);
                    await _ratingManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Rating Updated successfully." });
                }
            }
           return BadRequest();
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult>Delete(int id)
        {
            var rating = await _ratingManager.Get().FirstOrDefaultAsync(r => r.Id == id);
            if (rating is not null)
            {
                await _ratingManager.Delete(rating);
                await _ratingManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Rating deleted successfully." });
            }
            return BadRequest();
        }

    }
}
