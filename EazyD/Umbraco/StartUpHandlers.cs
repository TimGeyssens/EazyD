using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using EazyD.Interfaces;
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
            var dialogReference = (umbracoDialog)sender;

            var path = dialogReference.Page.Request.Path.ToLower();

            if (path.EndsWith("dialogs/treepicker.aspx"))
            {
                var cph = (ContentPlaceHolder)dialogReference.FindControl("head");
                var script = @"
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
            var cph = (ContentPlaceHolder)up.FindControl("body");

            return CompatibilityHelper.IsVersion7OrNewer ? cph.FindControl("body_Panel1_container") : cph.FindControl("Panel1");
        }

        private void umbracoPage_Load(object sender, EventArgs e)
        {
            var pageReference = (umbracoPage)sender;

            var path = pageReference.Page.Request.Path.ToLower();


            if (path.EndsWith("settings/views/editview.aspx") == true || path.EndsWith("settings/edittemplate.aspx"))
            {
               
                var c2 = GetPanel1Control(pageReference);

                if (c2 != null)
                {
                    if (!CompatibilityHelper.IsVersion7OrNewer) // U6
                    {

                        var panel = (UmbracoPanel) c2;

                        //Insert splitter in menu to make menu nicer on the eye
                        panel.Menu.InsertSplitter();

                        //Add new image button 
                        var eazyDBtn = panel.Menu.NewImageButton();
                        eazyDBtn.ToolTip = "Create dictionary item";
                        eazyDBtn.ImageUrl = "../../../App_Plugins/EazyD/Icons/dictionary_item_icon.png";
                        eazyDBtn.OnClientClick =
                            @"var selection = UmbEditor.IsSimpleEditor? jQuery('#body_editorSource').getSelection().text : UmbEditor._editor.getSelection();
                                                UmbClientMgr.openModalWindow('/App_Plugins/EazyD/Dialog?value='+selection, 'Create Dictionary Item', true, 400, 250);
                                                return false;";
                    }
                    else // U7
                    {

                        var prov = (IAddMenuButton)Activator.CreateInstance(Type.GetType("EazyD.Providers.AddV7MenuButton, EazyD"));
                        prov.Add(c2);
                    }


                }
            }

           
        }


      

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }
    }
}