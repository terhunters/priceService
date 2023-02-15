namespace PriceService.Model
{
    public interface IPrice
    {
        int Id { get; set; }
        int PlatformId { get; set; }
        int PriceValue { get; set; }
    }

    public class Price : IPrice
    {
        public int Id { get; set; }
        public int PriceValue { get; set; }
        public int PlatformId { get; set; }
        
        public string PlatformName { get; set; }
    }
}