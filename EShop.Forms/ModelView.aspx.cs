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
    public partial class ModelView : System.Web.UI.Page
    {

        [Inject]
        public IDataService DataService { get; set; }

        protected Model SelectedModel;
        //private readonly IDataService _dataService;

        public ModelView()
        {
            //_dataService = new EShopDataService(new AdoModelsRepository(Global.ConnectionString), new AdoCategoryRepository(Global.ConnectionString));            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = 0;

            if(int.TryParse(Request.QueryString["model"], out id))
            {
                //SelectedModel = _dataService.GetModel(id);
                SelectedModel = DataService.GetModel(id);
                modelImage.ImageUrl = string.Format("~/model.img?id={0}&mode=full", SelectedModel.ImageId);
            }
        }
    }
}