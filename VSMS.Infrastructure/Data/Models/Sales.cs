using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Infrastructure.Data.Models
{
    public class Sales
    {
        [Key]
        [StringLength(64)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public Users User { get; set; }

        public ICollection<SalesProducts> SalesProducts {get;set;}
        public Sales()
        {
            SalesProducts = new List<SalesProducts>();
        }
    }
}
