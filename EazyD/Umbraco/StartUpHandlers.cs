using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Core;
using umbraco.presentation.masterpages;
using umbraco.uicontrols;

namespace EazyD.Umbraco
{
    public class StartUpHandlers : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
           
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            umbracoPage.Load += umbracoPage_Load;
            umbracoDialog.Load += umbracoDialog_Load;

        }

        void umbracoDialog_Load(object sender, EventArgs e)
        {
            umbracoDialog dialogReference = (umbracoDialog)sender;

            string path = dialogReference.Page.Request.Path.ToLower();

            if (path.EndsWith("dialogs/treepicker.aspx"))
            {
                ContentPlaceHolder cph = (ContentPlaceHolder)dialogReference.FindControl("head");
                string script = @"
                <script>
                function openDictionaryItem(id) {
				    dialogHandler(id);
			    }                </script>
                ";

                cph.Controls.Add(new LiteralControl(script));
            }
        }

        private Control GetPanel1Control(umbracoPage up)
        {
            ContentPlaceHolder cph = (ContentPlaceHolder)up.FindControl("body");
            return cph.FindControl("Panel1");
        }

        private void umbracoPage_Load(object sender, EventArgs e)
        {
            umbracoPage pageReference = (umbracoPage)sender;

            string path = pageReference.Page.Request.Path.ToLower();


            if (path.EndsWith("settings/views/editview.aspx") == true || path.EndsWith("settings/edittemplate.aspx"))
            {
               
                Control c2 = GetPanel1Control(pageReference);

                if (c2 != null)
                {
                    UmbracoPanel panel = (UmbracoPanel)c2;

                    //Insert splitter in menu to make menu nicer on the eye
                    panel.Menu.InsertSplitter();

                    //Add new image button 
                    ImageButton bundleBtn = panel.Menu.NewImageButton();
                    bundleBtn.ToolTip = "Create dictionary item";
                    bundleBtn.ImageUrl = "../../../App_Plugins/EazyD/Icons/dictionary_item_icon.png";
                    bundleBtn.OnClientClick =   @"var selection = UmbEditor.IsSimpleEditor? jQuery('#body_editorSource').getSelection().text : UmbEditor._editor.getSelection();
                                                UmbClientMgr.openModalWindow('/App_Plugins/EazyD/Dialog?value='+selection, 'Create Dictionary Item', true, 400, 250);
                                                return false;";


                }
            }

           
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }
    }
}