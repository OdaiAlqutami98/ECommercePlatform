
namespace Odai.Shared.Models
{
    public class CommentModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public string Content { get; set; }
    }


}
