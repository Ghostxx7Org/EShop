using System.Data;
using System.Data.SqlClient;
using EShop.Entity;
using System;
using System.Collections.Generic;

namespace EShopAdoDataProvider
{
    public class AdoModelsRepository : IModelRepository
    {
        private readonly string _connectionString;
        public AdoModelsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// добавляет новую модель товара в БД
        /// </summary>
        /// <param name="item">объект EShop.Entity.Model который будет добавлен в БД</param>
        /// <returns></returns>
        public bool AddModel(Model item)
        {
            try
            {
                using (var connect = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[Model_Add]", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar, 512)).Value = item.Title;
                        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 2048)).Value = item.Description;
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int)).Value = item.CategoryId;
                        cmd.Parameters.Add(new SqlParameter("@ImageID", SqlDbType.Int)).Value = item.ImageId;
                        cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Money)).Value = item.Price;
                        cmd.Parameters.Add(new SqlParameter("@Warranty", SqlDbType.SmallInt)).Value = item.Warranty;
                        cmd.Parameters.Add(new SqlParameter("@Availability", SqlDbType.SmallInt)).Value = item.AvailabilityId == 0 ? (object)null : item.Availability;
                        cmd.Parameters.Add(new SqlParameter("@Delivery", SqlDbType.SmallInt)).Value = item.DeliveryId == 0 ? (object)null : item.Delivery;
                        cmd.Parameters.Add(new SqlParameter("@return_value", SqlDbType.Int)).Direction = ParameterDirection.ReturnValue;
                        connect.Open();
                        var result = cmd.ExecuteNonQuery();
                        var value = cmd.Parameters["@return_value"].Value;
                        if (value != null)
                            item.Id = (int)value;
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddModel method failed.", ex);
            }
        }

        /// <summary>
        /// Обновляет в БД запись выбранной модели
        /// </summary>
        /// <param name="item">код товара (модели)</param>
        /// <returns></returns>
        public bool UpdateModel(Model item)
        {
            //этот метод реализуете сами
            return true;
        }

        /// <summary>
        /// Удаляет выбранную модель из БД
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool DeleteModel(int modelId)
        {
            try
            {
                using (var connect = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[Model_DeleteModel]", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ModelID", SqlDbType.Int)).Value = modelId;
                        //output параметр
                        cmd.Parameters.Add(new SqlParameter("@rowsNumber", SqlDbType.Int)).Direction = ParameterDirection.Output;
                        connect.Open();
                        var result = cmd.ExecuteNonQuery();
                        var value = cmd.Parameters["@rowsNumber"].Value;
                        if (value != null) return (int)value == modelId;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteModel method failed.", ex);
            }
        }

        /// <summary>
        /// Полная информация о модели
        /// </summary>
        /// <param name="modelId">код модели (товара)</param>
        /// <returns></returns>
        public Model GetModel(int modelId)
        {
            try
            {
                using (var connect = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[Model_GetModel]", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ModelID", SqlDbType.Int)).Value = modelId;
                        connect.Open();
                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            return reader.Read() ? reader.ToModel() : null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetModel method failed.", ex);
            }
        }

        /// <summary>
        /// Возвращает список всех моделей для выбранной категории с учетом всех уровней 
        /// вложенности подкатегорий в категорию categoryId.
        /// Напр: для категории HDD вернет модели из категории External HDD, 2.5', 3.5' и т.д. со всех под категорий
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<Model> GetModels(int categoryId)
        {
            try
            {
                using (var connect = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[Model_GetModels]", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int)).Value = categoryId;
                        connect.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            var list = new List<Model>();
                            while (reader.Read())
                            {
                                list.Add(reader.ToModel());
                            }
                            return list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetModels method failed.", ex);
            }
        }

        /// <summary>
        /// Вывод списка модели для выбранной категории постранично
        /// </summary>
        /// <param name="categoryId">код категории</param>
        /// <param name="pageNumber">номер страницы</param>
        /// <param name="pageSize">кол-во записей на странице</param>
        /// <returns></returns>
        public IEnumerable<Model> GetModels(int categoryId, int pageNumber, int pageSize)
        {
            try
            {
                using (var connect = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[Model_GetModelsByPage]", connect))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CategoryID", SqlDbType.Int)).Value = categoryId;
                        cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int)).Value = pageNumber;
                        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int)).Value = pageSize;
                        connect.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            var list = new List<Model>();
                            while (reader.Read())
                            {
                                list.Add(reader.ToModel());
                            }
                            return list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetModels method failed.", ex);
            }
        }

        public void Dispose()
        {
            //здесь освобождаются неуправляемые ресурсы, если они есть в объекте:
            // - открытые файлы
            // - соедения с БД
            // - сокеты и т.д.
        }
    }
}
