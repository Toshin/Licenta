using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Services.User;
using ViewModels;

namespace Licenta_v01.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager _userManager;

        public UserController()
        {
            _userManager = new UserManager();
        }

        // GET: Forms
        [Authorize]
        public ActionResult SubmitForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitForm(UserViewModel form)
        {
            form.Id = User.Identity.GetUserId();

            _userManager.Add(form);

            return RedirectToAction("Index", "Home");
        }
    }
}