using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Odai.Domain;
using Odai.Logic.Common;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController(RatingManager _ratingManager, IIdentityService _identityService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult>GetAll()
        {
            var rating = await _ratingManager.GetAll().Include(r=>r.Product).ToListAsync();
            if (rating is not null)
            {
                return Ok(rating);
            }
           return BadRequest(false);
        }
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult>GetById(int id)
        {
            var rating=await _ratingManager.GetById(id);
            if (rating is not null)
            {
                return Ok(rating);
            }
            return BadRequest(false);
        }
        [HttpPost]
        [Route("AddEdit")]
        public async Task<IActionResult> AddEdit(RatingModel model)
        {
            var userId =  await _identityService.GetUserAsync(model.UserId);
            if (userId is null)
            {
                return Unauthorized("User not found.");
            }
            if (!Guid.TryParse(userId.Id.ToString(), out Guid user))
            {
                return BadRequest("Invalid user ID format.");
            }
            if (model.Id is null)
            {
                Rating rating =new Rating();
                rating.Value = model.Value;
                rating.Comment = model.Comment;
                rating.UserId = user;
                rating.ProductId = model.ProductId;
                rating.CreatonDate=DateTime.Now;
                rating.CreatedBy = user;
                await _ratingManager.Add(rating);
                await _ratingManager.SaveChangesAsync();
                return Ok(new Response<bool>());
            }
           else
            {
                var rating = await _ratingManager.Get().FirstOrDefaultAsync(r=>r.Id==model.Id);
                if (rating is not null)
                {
                    rating.Value = model.Value; 
                    rating.Comment = model.Comment;
                    rating.UserId = user;
                    rating.ProductId = model.ProductId;
                    rating.LastUpdateDate = DateTime.Now;
                    rating.LastUpdateBy = user;
                    _ratingManager.Update(rating);
                    await _ratingManager.SaveChangesAsync();
                    return Ok(new Response<bool>());
                }
            }
           return BadRequest(false);
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
                return Ok(new Response<bool>());
            }
            return BadRequest(false);
        }

    }
}
