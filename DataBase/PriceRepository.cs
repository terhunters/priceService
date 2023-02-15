using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using PriceService.DTO;
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
        

        public Price GetPriceByExternalPlatformId(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Price>("SELECT Prices.Id, Prices.PriceValue, Platforms.ExternalId as PlatformId, Platforms.Name as PlatformName " +
                                       "FROM Prices LEFT JOIN Platforms ON Platforms.ExternalId = Platforms.Id " +
                                       "WHERE PlatformId = @platformId", new { platformId }).FirstOrDefault();
            }
        }

        public Price GetPriceByInternalPlatformId(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Price>("SELECT Prices.Id, Prices.PriceValue, Prices.PlatformId, Platforms.Name as PlatformName " +
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
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (!PlatformExist(platformId))
                {
                    Console.WriteLine($"Platform with id = {platformId} doesn't exist");
                    return false;
                }
                
                Console.WriteLine("Start query to create new Price");
                var id = db.Query<int>("INSERT INTO Prices (PlatformId, PriceValue) Values(@platformId, @priceValue); SELECT CAST(SCOPE_IDENTITY() as int)",
                    new { platformId = platformId, priceValue = price.PriceValue }).FirstOrDefault();

                if (id != null)
                {
                    price.Id = id;
                    price.PlatformId = platformId;
                    return true;
                }

                return false;
            }
        }

        public void UpdatePrice(Price price)
        {
            throw new System.NotImplementedException();
        }

        public bool ExternalIdExist(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<int>("SELECT * FROM Platforms WHERE ExternalId = @platformId",
                    new { platformId }).FirstOrDefault() != null;
            }
        }

        public bool PlatformExist(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = db.Query<int>("SELECT * FROM Platforms WHERE Id = @platformId",
                    new { platformId }).FirstOrDefault() != null;
                Console.WriteLine($"Platform Exist result: {result}");
                return result;
            }
        }

        public bool CreatePlatform(Platform platform)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (ExternalIdExist(platform.Id)) return false;
                
                var id = db.Query<int>("INSERT INTO Platforms (ExternalId, Name) Values(@externalId, @Name)",
                    new { externalId = platform.Id, Name = platform.Name }).FirstOrDefault();
                if (id != null)
                {
                    platform.Id = id;
                    return true;
                }

                return false;
            }
        }
    }
}