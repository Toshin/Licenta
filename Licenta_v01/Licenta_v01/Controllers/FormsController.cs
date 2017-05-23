using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licenta_v01.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult SubmitForm()
        {
            return View();
        }
    }
}