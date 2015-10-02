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
    public class EFCategoryRepositoryAsync : ICategoryRepository
    {
        private string _connectionString;
        public EFCategoryRepositoryAsync(string connectStr)
        {
            _connectionString = connectStr;
        }
        public bool AddCategory(Category item)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                db.Categories.Attach(item);
                db.SaveChanges();
                return true;
            }
        }
        public bool UpdateCategory(Category item)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var category = db.Categories.FirstOrDefault(c => c.Id == item.Id);
                if (category == null) return false;
                category.ImageId = item.ImageId;
                category.ParentId = item.ParentId;
                category.Name = item.Name;
                db.SaveChanges();
                return true;
            }
        }
        public bool DeleteCategory(int itemId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var item = db.Categories.FirstOrDefault(c => c.Id == itemId);
                if (item != null)
                {
                    db.Categories.Remove(item);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Category GetCategory(int categoryId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var item = db.Categories.FirstOrDefault(c => c.Id == categoryId);
                return item ?? null;
            }
        }
        public IEnumerable<Category> RootCategories
        {
            get
            {
                using (var db = new EShopDbContext(_connectionString))
                {
                    return db.Categories.Where(c => c.ParentId == null).Select(c => c).ToArray();
                }
            }
        }
        public IEnumerable<Category> PathToRoot(int categoryId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var param = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return db.Database.SqlQuery<Category>("Category_PathToRoot @ID", param).ToArray();
            }
        }
        public IEnumerable<Category> GetSubCategories(int categoryId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                return
                db.Categories.Where(c => c.ParentId == categoryId)
                .Select(cat => cat)
                .ToArray();
            }
        }

        public IEnumerable<Category> GetAllSubCategories(int categoryId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var paramCategoryId = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return db.Database.SqlQuery<Category>("Category_AllSubCategories @ID", paramCategoryId).ToArray();
            }
        }
        public int CountModels(int categoryId)
        {
            using (var db = new EShopDbContext(_connectionString))
            {
                var paramCategoryId = new SqlParameter("@categoryId", SqlDbType.Int)
                {
                    Value = categoryId
                };
                return db.Database.SqlQuery<int>("SELECT [dbo].[GetModelsCount](@categoryId)", paramCategoryId).FirstOrDefault();
            }
        }
        public void Dispose()
        {
        }
    }
}
