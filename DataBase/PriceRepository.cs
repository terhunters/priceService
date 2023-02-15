using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using PriceService.Model;

namespace PriceService.DataBase
{
    public class PriceRepository : IPricesRepository
    {
        private readonly string _connectionString;

        public PriceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        

        public Price GetPriceByPlatformId(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Price>("SELECT Prices.Id, Prices.PriceValue, Platforms.ExternalId as PlatformId, Platforms.Name as PlatformName " +
                                       "FROM Prices LEFT JOIN Platforms ON Prices.PlatformId = Platforms.Id " +
                                       "WHERE PlatformId = @platformId", new { platformId }).FirstOrDefault();
            }
        }

        public IEnumerable<Price> GetAllPrices()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Price>("SELECT Prices.Id, Prices.PriceValue, Platforms.ExternalId as PlatformId, Platforms.Name as PlatformName " +
                                       "FROM Prices LEFT JOIN Platforms ON Prices.PlatformId = Platforms.Id").ToList();
            }
        }

        public bool CreatePrice(int platformId, Price price)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePrice(Price price)
        {
            throw new System.NotImplementedException();
        }

        public bool ExternalIdExist(int platformId)
        {
            throw new System.NotImplementedException();
        }

        public void CreatePlatform()
        {
            throw new System.NotImplementedException();
        }
    }
}