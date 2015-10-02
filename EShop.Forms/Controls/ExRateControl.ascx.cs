using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EShop.Forms.Controls
{
    
    class ExRateItem
    {
        public string Exchange { get; set; }
        public decimal Rate { get; set; }
    }
    
    public partial class ExRateControl : System.Web.UI.UserControl
    {

        private readonly IList<Exception> _exeption = new List<Exception>();
        private ExRateItem[] _selectedRates;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var exRates = new ExRates.ExRatesSoapClient();
                var rates = exRates.ExRatesDaily(DateTime.Now);
                _selectedRates= rates.Tables[0].Select("Cur_abbreviation = 'RUB' OR Cur_abbreviation = 'USD' OR Cur_abbreviation = 'EUR'").Select(row => new ExRateItem()
                    {
                        Exchange = row["Cur_abbreviation"].ToString(), Rate = (decimal)row["Cur_OfficialRate"]}).ToArray();
            }
            catch (Exception ex)
            {
                _exeption.Add(ex);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (_exeption.Count ==0)
            {
                multiControlView.SetActiveView(viewExRates);
                listOfRates.DataSource = _selectedRates;
                listOfRates.DataBind();
                lblUpdateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            }
            else
            {
                multiControlView.SetActiveView(viewErrors);
                var sb = new StringBuilder();
                foreach (var err in _exeption)
                {
                    sb.Append(err.Message).Append("<br/>");
                }
                lblControlError.Text = sb.ToString();
            }
        }
    }
}