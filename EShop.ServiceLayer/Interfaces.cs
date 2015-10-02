using System;
using System.Collections.Generic;
using EShop.Entity;

namespace EShop.ServiceLayer
{
    public interface IDataService : IDisposable
    {
        //прикладная бизнес-логика приложения ----------------------------------
        /// <summary>
        /// Возвращает список из number случайных моделей категории categoryId
        /// </summary>
        /// <param name="categoryId">id категории</param>
        /// <param name="number">кол-во случайных моделей</param>
        /// <returns></returns>
        IEnumerable<Model> GetRandomModels(int categoryId, int number);
        /// <summary>
        /// Возвращает список всех категорий дерева в виде линейного массива
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetCategoryList();
        //-----------------------------------------------------------------------

        //базовые методы управления категориями
        bool NewCategory(Category item);
        bool ChangeCategory(Category item);
        bool DeleteCategory(int itemId);
        Category GetCategory(int categoryId);
        IEnumerable<Category> RootCategories { get; }
        IEnumerable<Category> PathToRoot(int categoryId);
        IEnumerable<Category> GetSubCategories(int categoryId);
        IEnumerable<Category> GetAllSubCategories(int categoryId);
        int ModelsInCategory(int categoryId);
        //базовые методы управления моделями
        bool NewModel(Model item);
        bool ChangeModel(Model item);
        bool DeleteModel(int modelId);
        Model GetModel(int modelId);
        IEnumerable<Model> GetModels(int categoryId);
        IEnumerable<Model> GetModels(int categoryId, int from, int pageSize);
    }
}


