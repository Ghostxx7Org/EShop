using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using EShop.Entity;
using EShopEFDataProvider.FluentDbContext;

namespace EShopEFDataProvider
{
    public class EFCategoryRepository : ICategoryRepository
    {

        //запрещаем создавать объект класса по умолчанию - без параметров
        private EFCategoryRepository() { }

        private readonly string _connectionString;
        //EShopDbContext - наш fluent контекст для подключения к БД, который мы разработали 
        //в проекте EF.Training
        private readonly EShopDbContext _dbContext;
        public EFCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
            _dbContext = new EShopDbContext(_connectionString);
        }

        public bool AddCategory(Category item)
        {
            try
            {
                if (item.ParentId.HasValue)
                {
                    var parent = _dbContext.Categories.FirstOrDefault(c => c.Id == item.ParentId.Value);
                    if (parent == null)
                    {
                        throw new Exception(string.Format("New category {0} doesn't has valid parent category", item));
                    }
                }
                _dbContext.Categories.Add(item);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateCategory method failed.", ex);
            }
        }

        public bool UpdateCategory(Category item)
        {
            //сделать самостоятельно
            return true;
        }

        public bool DeleteCategory(int itemId)
        {
            try
            {
                var item = _dbContext.Categories.FirstOrDefault(c => c.Id == itemId);
                if (item != null)
                {
                    _dbContext.Categories.Remove(item);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteCategory method failed.", ex);
            }
        }

        public Category GetCategory(int categoryId)
        {
            try
            {
                return _dbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception("GetCategory method failed.", ex);
            }
        }

        public IEnumerable<Category> RootCategories
        {
            get
            {
                return (from c in _dbContext.Categories
                        where c.ParentId == null || (c.ParentId.HasValue && c.ParentId.Value == 0)
                        select c).ToList();
            }
        }

        public IEnumerable<Category> PathToRoot(int categoryId)
        {
            try
            {
                var param = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return _dbContext.Database.SqlQuery<Category>("Category_PathToRoot @ID", param).ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("PathToRoot method failed.", ex);
            }
        }

        public IEnumerable<Category> GetSubCategories(int categoryId)
        {
            try
            {
                return
                    _dbContext.Categories.Where(c => c.ParentId == categoryId)
                              .Select(cat => cat)
                              .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSubCategories method failed.", ex);
            }
        }

        public IEnumerable<Category> GetAllSubCategories(int categoryId)
        {
            try
            {
                var paramCategoryId = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return _dbContext.Database.SqlQuery<Category>("Category_AllSubCategories @ID", paramCategoryId).ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("PathToRoot method failed.", ex);
            }
        }

        public int CountModels(int categoryId)
        {
            try
            {
                var paramCategoryId = new SqlParameter("@categoryId", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return _dbContext.Database.SqlQuery<int>("SELECT [dbo].[GetModelsCount] (@categoryId)", paramCategoryId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("CountModels method failed.", ex);
            }
        }

        public void Dispose()
        {
            //освобождаем ресурсы
            _dbContext.Dispose();
        }
    }
}
