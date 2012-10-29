using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace listrandomizer.Controllers
{
    public class RandomizeController : Controller
    {
        //
        // GET: /Randomize/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(int numLists, string list)
        {
            return Json(new Randomize().Post(numLists, list), JsonRequestBehavior.AllowGet);
        }

    }
}
