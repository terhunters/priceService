using System.ComponentModel.DataAnnotations;

namespace PriceService.Model
{
    public class Price
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int PlatformId { get; set; }
        [Required]
        public int PriceValue { get; set; }
    }
}