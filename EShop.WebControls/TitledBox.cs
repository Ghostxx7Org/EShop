using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

//namespace EShop.WebControls
namespace FreeSale.Controls
{
    [ParseChildren(true)]

    [Designer(typeof(TitledBoxDisigner))]
    [ToolboxData("<{0}:TitledBox runat=server width=300px height=100px></{0}:TitledBox>")]
    public class TitledBox : CompositeControl
    {
        internal List<IAttributeAccessor> regions;
        private ITemplate _contentTemplate;
        private Style _StyleTitle;
        private Style _StyleContent;
        Table mainTable;
        TableRow titleRow, contentRow;
        TableCell titleCell, contentCell;

        [
        Category("Appearance"), Bindable(true),
        DefaultValue("+Title+"), Description("Title text of TitledBox")
        ]

        public string TitleText
        {
            get
            {
                object o = ViewState["TitleText"];
                return (o == null) ? "+Title+" : (string)o;
            }
            set
            {
                ViewState["TitleText"] = value;
            }
        }

        [
        Category("Layout"), DefaultValue(HorizontalAlign.NotSet),
        Description("Style of horizontal aligment of panel content")
        ]

        public HorizontalAlign HorizontalAlignContent
        {
            get
            {
                object o = ViewState["HorizontalAlignContent"];
                return (o == null) ? HorizontalAlign.NotSet : (HorizontalAlign)ViewState["HorizontalAlignContent"];
            }
            set
            {
                ViewState["HorizontalAlignContent"] = value;
            }
        }

        [
            Category("Layout"), DefaultValue(HorizontalAlign.NotSet),
        Description("Style of horizontal aligment of panel title")
        ]

        public HorizontalAlign HorizontalAlignTitle
        {
            get
            {
                object o = ViewState["HorizontalAlignTitle"];
                return (o == null) ? HorizontalAlign.NotSet : (HorizontalAlign)ViewState["HorizontalAlignTitle"];
            }
            set
            {
                ViewState["HorizontalAlignTitle"] = value;
            }
        }

        [
        Category("Layout"), DefaultValue(typeof(Unit), "100%"),
        Description("Width of TitledBox")
        ]

        public Unit WidthPanel
        {
            get
            {
                object o = ViewState["TitledBoxWidth"];
                return (o == null) ? Unit.Parse("100%") : (Unit)ViewState["TitledBoxWidth"];
            }
            set
            {
                ViewState["TitledBoxWidth"] = value;
            }
        }

        [Browsable(false)]
        [MergableProperty(false)]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(TitledBox))]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ContentTemplate
        {
            get
            {
                return _contentTemplate;
            }
            set
            {
                _contentTemplate = value;
            }
        }

        [
        Category("Appearance"),DefaultValue(null),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("Style of TitledBox title")
        ]
        public virtual Style StyleTitle
        {
            get
            {
                if (_StyleTitle == null)
                {
                    _StyleTitle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_StyleTitle).TrackViewState();
                    }                    
                }
                return _StyleTitle;
            }
        }

        [
        Category("Appearance"),DefaultValue(null),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("Style of TitledBox content")
        ]

        public virtual Style StyleContent
        {
            get
            {
                if (_StyleContent == null)
                {
                    _StyleContent = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_StyleContent).TrackViewState();
                    }
                }
                return _StyleContent;
            }
        }




        protected override void CreateChildControls()
        {
            mainTable = new Table()
            {
                CellPadding = 3,
                CellSpacing = 1,
                BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid,
                BorderColor = this.BorderColor,
                BorderWidth = this.BorderWidth,
                Width = Unit.Percentage(100)
            };
            mainTable.Attributes.Add("border", "1");
            titleRow = new TableRow();
            titleCell = new TableCell()
            {
                BackColor = StyleTitle.BackColor,
                HorizontalAlign = HorizontalAlignTitle,
                CssClass = StyleTitle.CssClass,
                VerticalAlign = VerticalAlign.Middle
            };
            titleRow.Cells.Add(contentCell);
            mainTable.Rows.Add(titleRow);
            mainTable.Rows.Add(contentRow);

            if (_contentTemplate != null)
            {
                _contentTemplate.InstantiateIn(contentCell);
            }
            regions = new List<IAttributeAccessor>();
            regions.Add(mainTable);
            Controls.Add(mainTable);
        }



        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                this.EnsureChildControls();
                if (mainTable == null)
                {
                    this.CreateChildControls();
                }
                titleCell.Text = TitleText;
                mainTable.Width = WidthPanel;
                this.Width = WidthPanel;

                if (StyleTitle != null)
                    titleCell.ApplyStyle(StyleTitle);
                if (StyleContent != null)
                    contentCell.ApplyStyle(StyleContent);
                if (this.CssClass != null)
                    mainTable.CssClass = CssClass;

                mainTable.RenderControl(writer);
            }
            catch (Exception ex)
            {
                writer.Write("TitledBox render error: " + ex.Message);
            }
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                base.LoadViewState(null);
                return;
            }
            else
            {
                object[] myState = (object[])savedState;
                if (myState.Length != 3)
                {
                    throw new ArgumentException("Invalid view state");
                }
                base.LoadViewState(myState[0]);
                ((IStateManager)StyleTitle).LoadViewState(myState[1]);
                ((IStateManager)StyleContent).LoadViewState(myState[2]);
            }
        }

        protected override object SaveViewState()
        {
            object[] myState = new object[3];
            myState[0] = base.SaveViewState();
            if (_StyleTitle != null)
                myState[1] = ((IStateManager)_StyleTitle).SaveViewState();
            if (_StyleContent != null)
                myState[2] = ((IStateManager)_StyleContent).SaveViewState();
            return myState;
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_StyleTitle != null)
                ((IStateManager)_StyleTitle).TrackViewState();
            if (_StyleContent != null)
                ((IStateManager)_StyleContent).TrackViewState();
        }
    }
}
