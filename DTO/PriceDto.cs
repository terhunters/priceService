namespace PriceService.DTO
{
    public class PriceDto
    {
        public int Id { get; set; }
        public int PriceValue { get; set; }
        public int PlatformId { get; set; }
        
        public string PlatformName { get; set; }
    }
}