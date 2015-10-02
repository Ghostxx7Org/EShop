using EShop.Entity;
using EShopEFDataProvider.FluentDbContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopEFDataProvider
{
    public class EFModelsRepositoryAsync : IModelRepository
    {
        private string _connectStr;
        public EFModelsRepositoryAsync(string connectStr)
        {
            _connectStr = connectStr;
        }
        public bool AddModel(Model item)
        {
            //объект DbContext создается в каждом методе заново, что бы гарантировать
            //один экземпляр DbContext на поток
            using (var db = new EShopDbContext(_connectStr))
            {
                var category = db.Categories.FirstOrDefault(c => c.Id == item.CategoryId);
                if (category == null)
                {
                    throw new Exception(string.Format("New model {0} doesn't have valid category ID: {1}", item, item.CategoryId));
                }
                //добавляет ообъект типа EShop.Entity.Model в коллекцию DbSet<Model> Models класса EShopDbContext
                db.Models.Add(item);
                //метод SaveChanges проверяет статусы всех объектов коллекции DbSet<Model> Models и генерирует соответвукхцие
                //SQL команды для объектов, которые были добавлены (INSERT), изменены (UPDATE) или удалены (DELETE)
                db.SaveChanges();
                return true;
            }
        }
        public bool UpdateModel(Model item)
        {
            using (var db = new EShopDbContext(_connectStr))
            {
                var m = db.Models.FirstOrDefault(c => c.Id == item.Id);
                if (m == null)
                {
                    return false;
                }
                m.ImageId = item.ImageId;
                m.Price = item.Price;
                m.Title = item.Title;
                m.AvailabilityId = item.AvailabilityId;
                m.CategoryId = item.CategoryId;
                m.Availability = item.Availability;
                m.Category = item.Category;
                m.Warranty = item.Warranty;
                m.Description = item.Description;
                m.DeliveryId = item.DeliveryId;
                m.Delivery = item.Delivery;
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteModel(int modelId)
        {
            using (var db = new EShopDbContext(_connectStr))
            {
                var m = db.Models.FirstOrDefault(c => c.Id == modelId);
                if (m == null)
                {
                    return false;
                }
                db.Models.Remove(m);
                db.SaveChanges();
                return true;
            }
        }
        public Model GetModel(int modelId)
        {
            using (var db = new EShopDbContext(_connectStr))
            {
                //что бы сразу заполнить 'navigation property' Category в классе Model, включим объединение в
                //запросе к таблицам Models и Categories через вызов db.Models.Include("Category")|
                var m = db.Models.Include("Category").FirstOrDefault(c => c.Id == modelId);
                return m ?? null;
            }
        }
        public IEnumerable<Model> GetModels(int categoryId)
        {
            using (var db = new EShopDbContext(_connectStr))
            {
                var param = new SqlParameter()
                {
                    ParameterName = "@CategoryId",
                    SqlDbType = SqlDbType.Int,
                    Value = categoryId
                };
                return db.Database.SqlQuery<ModelComplex>("Model_GetModels @CategoryId", param).Select(m => new Model()
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
                }).ToArray();
            }
        }

        public IEnumerable<Model> GetModels(int categoryId, int pageNumber, int pageSize)
        {
            using (var db = new EShopDbContext(_connectStr))
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
                return db.Database.SqlQuery<ModelComplex>("Model_GetModelsByPage @CategoryID, @PageNo, @pageSize", paramCategory, paramPageNo, paramPageSize).Select(m => new Model()
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
                }).ToArray();
            }
        }

        public void Dispose()
        {
        }
    }
}