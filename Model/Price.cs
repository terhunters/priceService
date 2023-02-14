using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceService.Model
{
    public class Price
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PlatformId { get; set; }
        [ForeignKey("PlatformId")]
        public Platform Platform { get; set; }
        [Required]
        public int PriceValue { get; set; }
    }
}