
namespace Odai.Shared.Models
{
    public class CommentModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }


}
