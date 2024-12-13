using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Odai.Domain;
using Odai.Logic.Common.Interface;
using Odai.Logic.Manager;
using Odai.Shared;
using Odai.Shared.Auth;
using Odai.Shared.Table;

namespace Odai.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentManager _commentManager;
        public CommentController(CommentManager commentManager)
        {
            _commentManager = commentManager;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<TableResponse<Comment>> GetAll(int pageIndex,int pageSize)
        {

            var query  = _commentManager.GetAll().Include(c=>c.Product);
            var length = query.Count();
            var comment= await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new TableResponse<Comment>
            {
                RecordsTotal = length,
                Data = comment
            };

        }
        [HttpGet("GetById")]
        public async Task<IActionResult>GetById(int Id)
        {
           var comment=await _commentManager.GetById(Id);
            if (comment is not null)
            {
                return Ok(comment);
            }
            return BadRequest();
        }
        [HttpPost("AddEdit")]
        public async Task<IActionResult> AddEdit(CommentModel model)
        {
            if (model.Id == null)
            {
                Comment comment =new Comment();
                comment.ProductId = model.ProductId;
                comment.Content = model.Content;
                comment.UserId= model.UserId;
                comment.CreatedBy = model.UserId;
                comment.CreatonDate = DateTime.Now;
                await _commentManager.Add(comment);
                await  _commentManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Comment Added successfully." });

            }
            else
            {
                var comment = await _commentManager.Get(c=>c.Id==model.Id).FirstOrDefaultAsync();
                if( comment is not null )
                {
                   comment.Content = model.Content;
                    comment.UserId= model.UserId;
                    comment.ProductId= model.ProductId;
                    comment.LastUpdateBy=model.UserId;
                    comment.LastUpdateDate= DateTime.Now;
                    _commentManager.Update(comment);
                     await _commentManager.SaveChangesAsync();
                    return Ok(new Response<bool> { Succeeded = true, Message = "Comment Updated successfully." });
                }
                else
                {
                    return NoContent();
                }
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult>Delete(int Id)
        {
            var comment = await _commentManager.Get(c => c.Id == Id).FirstOrDefaultAsync();
            if( comment is not null )
            {
                await _commentManager.Delete(comment);
                await _commentManager.SaveChangesAsync();
                return Ok(new Response<bool> { Succeeded = true, Message = "Comment deleted successfully."});
            }
            return NotFound(new Response<bool>{ Succeeded = true, Message = "Not Found Comment." });
        }
    }
}
