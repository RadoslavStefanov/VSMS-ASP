using System.ComponentModel.DataAnnotations;

namespace VSMS.Infrastructure.Data.Models
{
    public class ResetRequests
    {
        [Key]
        [StringLength(64)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Date { get; set; }
        public string Username { get; set; }
    }
}
