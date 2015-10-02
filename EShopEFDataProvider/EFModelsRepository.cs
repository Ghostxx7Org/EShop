using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EShop.Entity;
using EShopEFDataProvider.FluentDbContext;

namespace EShopEFDataProvider
{
    public class EFModelsRepository : IModelRepository
    {
        //запрещаем создавать объект класса по умолчанию - без параметров
        private EFModelsRepository() { }

        private readonly string _connectionString;
        //EShopDbContext - наш fluent контекст для подключения к БД, который мы разработали 
        //в проекте EF.Training
        private readonly EShopDbContext _dbContext;
        public EFModelsRepository(string connectionString)
        {
            _connectionString = connectionString;
            _dbContext = new EShopDbContext(_connectionString);
        }

        public bool AddModel(Model item)
        {
            try
            {
                var category = _dbContext.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                if (category == null)
                {
                    throw new Exception(string.Format("New model {0} doesn't have valid category ID: {1}", item, item.CategoryId));
                }
                //добавляет ообъект типа EShop.Entity.Model в коллекцию DbSet<Model> Models класса EShopDbContext
                _dbContext.Models.Add(item);
                //метод SaveChanges проверяет статусы всех объектов коллекции DbSet<Model> Models и генерирует соответвующие
                //SQL команды для объектов, которые были добавлены (INSERT), изменены (UPDATE) или удалены (DELETE) 
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AddModel method failed", ex);
            }
        }

        public bool UpdateModel(Model item)
        {
            //сделать самостоятельно
            this.DeleteModel(item.Id);
            this.AddModel(item);
            return true;
        }

        public bool DeleteModel(int modelId)
        {
            try
            {
                var model = _dbContext.Models.FirstOrDefault(m => m.Id == modelId);
                if (model != null)
                {
                    _dbContext.Models.Remove(model);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteModel method failed", ex);
            }
        }

        public Model GetModel(int modelId)
        {
            try
            {
                //для информации о категории подключим navigation property Category
                return _dbContext.Models.Include("Category").FirstOrDefault(m => m.Id == modelId);
            }
            catch (Exception ex)
            {
                throw new Exception("GetModel method failed", ex);
            }
        }

        public IEnumerable<Model> GetModels(int categoryId)
        {
            try
            {
                var param = new SqlParameter()
                {
                    ParameterName = "@CategoryID",
                    SqlDbType = SqlDbType.Int,
                    Value = categoryId
                };
                return _dbContext.Database.SqlQuery<ModelComplex>("Model_GetModels @CategoryID", param).Select(m => new Model()
                {
                    Id = m.Id,
                    ImageId = m.ImageId,
                    Title = m.Title,
                    DeliveryId = m.Delivery,
                    AvailabilityId = m.Availability,
                    Description = m.Description,
                    Price = m.Price,
                    CategoryId = m.CategoryId,
                    Warranty = m.Warranty
                });
            }
            catch (Exception ex)
            {
                throw new Exception("GetModels method failed", ex);
            }
        }

        public IEnumerable<Model> GetModels(int categoryId, int pageNumber, int pageSize)
        {
            try
            {
                var paramCategory = new SqlParameter()
                {
                    ParameterName = "@CategoryID",
                    SqlDbType = SqlDbType.Int,
                    Value = categoryId
                };
                var paramPageNo = new SqlParameter()
                {
                    ParameterName = "@PageNo",
                    SqlDbType = SqlDbType.Int,
                    Value = pageNumber
                };
                var paramPageSize = new SqlParameter()
                {
                    ParameterName = "@PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = pageSize
                };
                return _dbContext.Database.SqlQuery<ModelComplex>("Model_GetModelsByPage @CategoryID, @PageNo, @PageSize", paramCategory, paramPageNo, paramPageSize).Select(m => new Model()
                {
                    Id = m.Id,
                    ImageId = m.ImageId,
                    Title = m.Title,
                    DeliveryId = m.Delivery,
                    AvailabilityId = m.Availability,
                    Description = m.Description,
                    Price = m.Price,
                    CategoryId = m.CategoryId,
                    Warranty = m.Warranty
                });
            }
            catch (Exception ex)
            {
                throw new Exception("GetModels method failed", ex);
            }
        }

        public void Dispose()
        {
            //осводождаем ресурсы
            _dbContext.Dispose();
        }
    }
}
