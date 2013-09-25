using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EazyD.Models;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.language;
using Umbraco.Web.Mvc;

namespace EazyD.Controllers
{
    public class EazyDController : UmbracoAuthorizedController
    {
        public ActionResult Dialog(string value)
        {
            var m = new DictionaryItemViewModel {Value = value};
            if (Session["parentId"] != null)
                m.Parent = Session["parentId"].ToString();

            return View(Config.DialogViewPath, m);
        }

        [HttpPost]
        public ActionResult CreateDictionaryItem(DictionaryItemViewModel model)
        {
            int id;
            if (model.Parent != string.Empty && int.TryParse(model.Parent, out id))
            {
                Session["parentId"] = model.Parent;

                var p = new Dictionary.DictionaryItem(id);

                Dictionary.DictionaryItem.addKey(model.Key, model.Value, p.key);
            }
            else
            {
                Session["parentId"] = null;
                Dictionary.DictionaryItem.addKey(model.Key, model.Value);
            }

            var d = new Dictionary.DictionaryItem(model.Key);

            foreach (var lang in Language.GetAllAsList())
            {
                d.setValue(lang.id,model.Value);
                d.Save();
            }

            TempData["success"] = true;

            return View(Config.DialogViewPath, model);

        }

        public JsonResult IsKeyAvailable(string key)
        {

            if (!Dictionary.DictionaryItem.hasKey(key))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json("Key is not available", JsonRequestBehavior.AllowGet);

        }

      
        public ActionResult GetDictionaryItemKey(int id)
        {
            var d = new Dictionary.DictionaryItem(id);

            return Json(new { d = d.key});
        }

    }
}
