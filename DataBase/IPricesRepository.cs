using System.Collections.Generic;
using PriceService.Model;

namespace PriceService.DataBase
{
    public interface IPricesRepository
    {
        Price GetPriceByExternalPlatformId(int platformId);
        
        Price GetPriceByInternalPlatformId(int platformId);

        IEnumerable<Price> GetAllPrices();

        bool CreatePrice(int platformId, Price price);
        
        void UpdatePrice(Price price);

        bool ExternalIdExist(int platformId);
        
        bool PlatformExist(int platformId);

        bool CreatePlatform(Platform platform);
    }
}