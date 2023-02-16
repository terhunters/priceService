using System;
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
                var platform = PlatformExist(platformId);
                if (platform is null)
                {
                    Console.WriteLine($"Platform with id = {platformId} doesn't exist");
                    return false;
                }
                
                Console.WriteLine("Start query to create new Price");
                var id = db.Query<int>("INSERT INTO Prices (PlatformId, PriceValue) Values(@platformId, @priceValue);",
                    new { platformId = platformId, priceValue = price.PriceValue }).FirstOrDefault();

                if (id != null)
                {
                    Console.WriteLine("Price created");
                    price.Id = id;
                    price.PlatformId = platformId;
                    price.PlatformName = platform.Name;
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
                return db.Query<Platform>("SELECT * FROM Platforms WHERE ExternalId = @platformId",
                    new { platformId }).FirstOrDefault() != null;
            }
        }

        public Platform PlatformExist(int platformId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Platform>("SELECT * FROM Platforms WHERE Id = @platformId",
                    new { platformId }).FirstOrDefault();
            }
        }

        public bool CreatePlatform(Platform platform)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                if (ExternalIdExist(platform.Id))
                {
                    Console.WriteLine($"Platform with externalId = {platform.Id} doesn't exist");
                    return false;
                }
                
                Console.WriteLine("Start query to create new Platform");
                var id = db.Query<int>("INSERT INTO Platforms (ExternalId, Name) Values(@externalId, @Name)",
                    new { externalId = platform.Id, Name = platform.Name }).FirstOrDefault();
                if (id != null)
                {
                    Console.WriteLine("Platform created");
                    platform.Id = id;
                    return true;
                }

                return false;
            }
        }
    }
}