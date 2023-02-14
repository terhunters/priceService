using System.ComponentModel.DataAnnotations;

namespace PriceService.Model
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Version { get; set; }
    }
}