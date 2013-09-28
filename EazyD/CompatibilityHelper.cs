using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using umbraco;

namespace EazyD
{
    public class CompatibilityHelper
    {
        public static bool UseLegacySchema
        {
            get
            {

                return UmbracoSettings.UseLegacyXmlSchema;
               
            }
        }

        public static bool IsVersion7OrNewer
        {
            get
            {
                var retval = true;
                try
                {
                    // Attempt to access a static AField field defined in the App class. 
                    // However, because the App class does not define this field, 
                    // a MissingFieldException is thrown. 
                   
                    typeof(umbraco.uicontrols.CodeArea).InvokeMember("Menu",
                             BindingFlags.GetField, null, new umbraco.uicontrols.CodeArea(), null);
                }
                catch (MissingFieldException e)
                {
                    retval = false;
                }

                return retval;
            }
        }
        public static bool IsVersion4Dot5OrNewer
        {
            get
            {
                var retval = true;

                try
                {
                    bool t = UseLegacySchema;
                }
                catch (MissingMethodException)
                {
                    retval = false;
                }

                return retval;


            }
        }
    }
}