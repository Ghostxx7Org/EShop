using System;
using System.Data.SqlClient;
using EShop.Entity;

namespace EShopAdoDataProvider
{
    static class DataReaderEx
    {
        public static Category ToCategory(this SqlDataReader reader)
        {
            return new Category()
            {
                Id = reader["ID"] != DBNull.Value ? (int)reader["ID"] : 0,
                ImageId = reader["ImageID"] != DBNull.Value ? (int)reader["ImageID"] : 0,
                ParentId = reader["ParentId"] != DBNull.Value ? (int)reader["ParentId"] : 0,
                Name = reader["Name"].ToString()
            };
        }

        public static Model ToModel(this SqlDataReader reader)
        {
            return new Model()
            {
                Id = reader["ID"] != DBNull.Value ? (int)reader["ID"] : 0,
                CategoryId = reader["CategoryID"] != DBNull.Value ? (int)reader["CategoryID"] : 0,
                ImageId = reader["ImageId"] != DBNull.Value ? (int)reader["ImageId"] : (int?)null,
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                Price = reader["Price"] != DBNull.Value ? (decimal)reader["Price"] : 0,
                AvailabilityId = (short)(reader["Availability"] != DBNull.Value ? (short)reader["Availability"] : 0),
                DeliveryId = (short)(reader["Delivery"] != DBNull.Value ? (short)reader["Delivery"] : 0),
                Warranty = (short)(reader["Warranty"] != DBNull.Value ? (short)reader["Warranty"] : 0)
            };
        }
    }
}
