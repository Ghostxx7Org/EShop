using System;
using System.Collections.Generic;

namespace EShop.Entity
{
    public interface IModelRepository : IDisposable
    {
        /// <summary>
        /// Add model to repository
        /// </summary>
        /// <param name="item">New model</param>
        /// <returns></returns>
        bool AddModel(Model item);
        /// <summary>
        /// Update existing model
        /// </summary>
        /// <param name="item">Model with changes</param>
        /// <returns></returns>
        bool UpdateModel(Model item);
        /// <summary>
        /// Delete model from repository
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        bool DeleteModel(int modelId);
        /// <summary>
        /// Find model by Id
        /// </summary>
        /// <param name="modelId">Integer: model Id in DB</param>
        /// <returns></returns>
        Model GetModel(int modelId);
        /// <summary>
        /// Get all models of selected categoru
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IEnumerable<Model> GetModels(int categoryId);
        /// <summary>
        /// Get models by categoryId with paging
        /// </summary>
        /// <param name="categoryId">id category</param>
        /// <param name="pageNumber">page No</param>
        /// <param name="pageSize">models per page</param>
        /// <returns></returns>
        IEnumerable<Model> GetModels(int categoryId, int pageNumber, int pageSize);
    }

    public interface ICategoryRepository : IDisposable
    {
        bool AddCategory(Category item); //добавить новую категорию
        bool UpdateCategory(Category item); //обновить категорию в репозитории
        bool DeleteCategory(int itemId); //удалить категорию
        Category GetCategory(int categoryId); //найти категорию по id
        IEnumerable<Category> RootCategories { get; } //список корневых категорий
        IEnumerable<Category> PathToRoot(int categoryId); //путь от выбранной категории categoryId по дереву вверх к корневой
        IEnumerable<Category> GetSubCategories(int categoryId); //список ближайших подкатегорий
        IEnumerable<Category> GetAllSubCategories(int categoryId); //список всех подкатегорий выбранной категории,
                                                                   //независимо от уровня вложенности
        int CountModels(int categoryId); //кол-во моделей в выбранной категории, а так же во 
                                         //всех под категориях независимо от уровня вложенности
    }
}


