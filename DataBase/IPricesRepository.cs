using System.Collections.Generic;
using PriceService.Model;

namespace PriceService.DataBase
{
    public interface IPricesRepository
    {
        bool SaveChanges();
        
        Price GetPricesByPlatformId(int platformId);

        IEnumerable<Price> GetAllPrices();

        bool CreatePrice(int platformId, Price price);
        
        void UpdatePrice(Price price);

        bool ExternalIdExist(int platformId);
    }
}