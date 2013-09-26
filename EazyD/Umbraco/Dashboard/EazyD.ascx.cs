using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EazyD.Interfaces;
using EazyD.Models;
using HtmlAgilityPack;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;

namespace EazyD.Umbraco.Dashboard
{
    public partial class EazyD : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var dir = new DirectoryInfo(Server.MapPath("~/Views"));

            foreach (var file in dir.GetFiles("*.cshtml"))
            {
                var replace = new System.Collections.Generic.Dictionary<string, string>();

                var changes = false;
                var doc = new HtmlDocument();
                doc.Load(file.FullName);

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']"))
                {
                    if (!node.InnerText.Contains("@") && !node.InnerText.Contains("{") && !node.InnerText.Contains("}")
                        && !node.InnerText.Contains("<") && !node.InnerText.Contains(">") &&
                        !node.InnerText.Contains("(")
                        && !node.InnerText.Contains("("))
                    {

                        var staticVal = node.InnerText.Trim();

                        // create dic item
                        var m = new DictionaryItemViewModel { Value = staticVal };

                        if (Settings.GetSetting("autoGenerateKey") == true.ToString())
                        {
                            var prov = (IAutoGenKey)Activator.CreateInstance(Type.GetType(Settings.GetSetting("autoGenerateKeyProvider")));
                            var key = prov.Generate(staticVal);

                            var newKey = key;
                            var c = 1;
                            while (Dictionary.DictionaryItem.hasKey(newKey))
                            {
                                newKey = key + c;
                                c++;
                            }
                            m.Key = newKey;
                        }


                        if (!Dictionary.DictionaryItem.hasKey(file.Name))
                            Dictionary.DictionaryItem.addKey(file.Name, "");

                        Dictionary.DictionaryItem.addKey(m.Key, m.Value, file.Name);
                        var d = new Dictionary.DictionaryItem(m.Key);

                        foreach (var lang in Language.GetAllAsList())
                        {
                            d.setValue(lang.id, m.Value);
                            d.Save();
                        }


                        replace.Add(m.Key, staticVal);

                        if (!changes)
                            changes = true;
                    }


                }

                //if (changes)
                //{
                //    var contents = File.ReadAllText(file.FullName);
                //    var newcontents = string.Empty;
                //    foreach (var item in replace)
                //        newcontents = contents.Replace(item.Value, "@Umbraco.GetDictionaryValue(\"" + item.Key + "\")");

                //    File.WriteAllText(file.FullName, newcontents);
                //}

                //doc.Save(file.FullName);
            }

            Button1.Text = "Done";
        }
    }
}