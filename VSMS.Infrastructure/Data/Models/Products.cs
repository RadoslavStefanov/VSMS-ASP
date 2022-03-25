using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSMS.Infrastructure.Data.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int Kilograms { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Categories Category { get; set; }

        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required]
        [Range(minimum:0,maximum:double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public ICollection<SalesProducts> SalesProducts { get; set; }
        public Products()
        {
            SalesProducts = new List<SalesProducts>();
        }

    }
}
