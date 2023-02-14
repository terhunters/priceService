using System.Collections.Generic;
using System.Linq;
using PriceService.Model;

namespace PriceService.DataBase
{
    public class PriceRepository : IPricesRepository
    {
        private readonly AppDbContext _context;

        public PriceRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Price GetPricesByPlatformId(int platformId)
        {
            return _context.Prices.FirstOrDefault(x => x.PlatformId == platformId);
        }

        public IEnumerable<Price> GetAllPrices()
        {
            return _context.Prices.ToList();
        }

        public bool CreatePrice(int platformId, Price price)
        {
            if (_context.Platforms.FirstOrDefault(x => x.Id == platformId) == null) return false;
            
            price.PlatformId = platformId;
            _context.Add(price);

            return SaveChanges();
        }

        public void UpdatePrice(Price price)
        {
            var updatedPrice = _context.Prices.First(x => x.Id == price.Id);
            updatedPrice.PriceValue = price.PriceValue;
            SaveChanges();
        }

        public bool ExternalIdExist(int platformId)
        {
            return _context.Prices.FirstOrDefault(x => x.PlatformId == platformId) != null;
        }
    }
}