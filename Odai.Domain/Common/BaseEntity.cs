using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odai.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatonDate { get; set; }= DateTime.Now;
        public Guid? LastUpdateBy { get; set; }
        public DateTime? LastUpdateDate { get; set; } = DateTime.Now;
        public int? IsDeleted { get; set; }
    }
}
