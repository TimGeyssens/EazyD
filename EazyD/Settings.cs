using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml;
using EazyD.Services;
using umbraco.BusinessLogic;

namespace EazyD
{
    public class Settings
    {
        public static XmlDocument EazyDConfig
        {
            get
            {
                var us = (XmlDocument)HttpRuntime.Cache["EazyDSettingsFile"];
                if (us == null)
                    us = EnsureSettingsDocument();
                return us;
            }
        }

        private static XmlDocument EnsureSettingsDocument()
        {
            var settingsFile = HttpRuntime.Cache["EazyDSettingsFile"];
            var fullPath = HostingEnvironment.MapPath(Config.ConfigPath);

            // Check for language file in cache
            if (settingsFile == null)
            {
                var temp = new XmlDocument();
                var settingsReader = new XmlTextReader(fullPath);
                try
                {
                    temp.Load(settingsReader);
                    HttpRuntime.Cache.Insert("EazyDSettingsFile", temp, new CacheDependency(fullPath));
                }
                catch (XmlException e)
                {
                    throw new XmlException("Your EazyD.config file fails to pass as valid XML. Refer to the InnerException for more information", e);
                }
                catch (Exception e)
                {
                    Log.Add(LogTypes.Error, new User(0), -1, "Error reading bundles.config file: " + e.ToString());
                }
                settingsReader.Close();
                return temp;
            }
            else
                return (XmlDocument)settingsFile;
        }
        private static object configCacheSyncLock = new object();

        public static string GetSetting(string key)
        {
            return CacheService.GetCacheItem<string>("EazyDKey" + key, configCacheSyncLock, TimeSpan.FromHours(6),
                    delegate
                    {

                        XmlNode x = EazyDConfig.SelectSingleNode(string.Format("//setting [@key = '{0}']", key));
                        if (x != null)
                            return x.Attributes["value"].Value;
                        return string.Empty;
                    });
        }
    }
}