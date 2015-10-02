using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

//namespace EShop.WebControls
namespace FreeSale.Controls
{
    public class TitledBoxDisigner : CompositeControlDesigner
    {
        private TitledBox _shSimplePanel;

        private int _currentRegion = -1;

        private int _nbRegions = 0;



        public override void Initialize(IComponent component)
        {

            _shSimplePanel = (TitledBox)component;

            base.Initialize(component);

            SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true);

            SetViewFlags(ViewFlags.TemplateEditing, true);

        }



        public override TemplateGroupCollection TemplateGroups
        {

            get
            {

                TemplateGroupCollection collection = new TemplateGroupCollection();

                TemplateGroup group = new TemplateGroup("ContentTemplate");

                TemplateDefinition definition = new TemplateDefinition(

                    this, "ContentTemplate", _shSimplePanel, "ContentTemplate", false);

                group.AddTemplateDefinition(definition);

                collection.Add(group);

                return collection;

            }

        }



        protected override void CreateChildControls()
        {

            base.CreateChildControls();

            if (_shSimplePanel.regions != null)
            {

                _nbRegions = _shSimplePanel.regions.Count;

                for (int i = 0; i < _nbRegions; i++)
                {

                    _shSimplePanel.regions[i].SetAttribute(

                        DesignerRegion.DesignerRegionAttributeName, i.ToString());

                }

            }

        }



        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {

            this.CreateChildControls();



            for (int i = 0; i < _nbRegions; i++)
            {

                DesignerRegion r;

                if (_currentRegion == i)

                    r = new EditableDesignerRegion(this, i.ToString());

                else

                    r = new DesignerRegion(this, i.ToString());

                regions.Add(r);

            }



            if ((_currentRegion >= 0) && (_currentRegion < _nbRegions))

                regions[_currentRegion].Highlight = true;

            return base.GetDesignTimeHtml(regions);

        }



        protected override void OnClick(DesignerRegionMouseEventArgs e)
        {

            base.OnClick(e);

            _currentRegion = -1;

            if (e.Region != null)
            {

                for (int i = 0; i < _nbRegions; i++)
                {

                    if (e.Region.Name == i.ToString())
                    {

                        _currentRegion = i;

                        break;

                    }

                }

                UpdateDesignTimeHtml();

            }

        }



        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {

            IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));

            if (host != null)
            {

                ITemplate contentTemplate;

                if (_currentRegion == 0)
                {

                    contentTemplate = _shSimplePanel.ContentTemplate;

                    return ControlPersister.PersistTemplate(contentTemplate, host);

                }

            }

            return String.Empty;

        }



        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {

            if (content == null)

                return;

            IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));

            if (host != null)
            {

                ITemplate template = ControlParser.ParseTemplate(host, content);

                if (template != null)
                {

                    if (_currentRegion == 0)
                    {

                        _shSimplePanel.ContentTemplate = template;

                    }

                }

            }

        }
    }
}
