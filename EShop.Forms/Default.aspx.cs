using EShop.Entity;
using EShop.ServiceLayer;
using EShopAdoDataProvider;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EShop.Forms
{
    public partial class Default : System.Web.UI.Page
    {
        [Inject]
        public IDataService DataService { get; set; }
        //private readonly IDataService _dataService;

        private IEnumerable<Model> _modelList;

        private int _categoryId = 0;

        public Default()
        {
            //_dataService = new EShopDataService(new AdoModelsRepository(Global.ConnectionString), new AdoCategoryRepository(Global.ConnectionString));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["category"] != null)
            {
                var n = 0;
                _categoryId = int.TryParse(Request.QueryString["category"], out n) ? n : 0;
            }
            if (_categoryId > 0) 
            {
                //_modelList = _dataService.GetModels(_categoryId);
                _modelList = DataService.GetModels(_categoryId);
            }
        }
        protected void Page_PreRender (object sender, EventArgs e)
        {
            //categoryList.DataSource = _dataService.RootCategories;
            categoryList.DataSource = DataService.RootCategories;
            categoryList.DataBind();
            if (_categoryId <= 0) return;
            modelList.DataSource = _modelList;
            modelList.DataBind();
        }
    }
}