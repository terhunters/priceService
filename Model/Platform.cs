namespace PriceService.Model
{
    public interface IPlatform
    {
        int Id { get; set; }
        int ExternalId { get; set; }
        string Name { get; set; }
    }

    public class Platform : IPlatform
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }
    }
}