using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using EazyD.Services;
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

        private static object configCacheSyncLock = new object();

        public static bool IsVersion7OrNewer
        {
            get
            {
                 return CacheService.GetCacheItem<bool>("EazyDIsVersion7OrNewer", configCacheSyncLock, TimeSpan.FromHours(6),
                     delegate
                     {
                         var retval = true;
                         try
                         {
                             typeof (umbraco.uicontrols.CodeArea).InvokeMember("Menu",
                                 BindingFlags.GetField, null, new umbraco.uicontrols.CodeArea(), null);
                         }
                         catch (MissingFieldException e)
                         {
                             retval = false;
                         }

                         return retval;
                     });
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