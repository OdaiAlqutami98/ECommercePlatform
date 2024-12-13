using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Shared
{
    public class CommentModel
    {
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
    
    
}
