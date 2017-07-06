using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Services.User;
using ViewModels;
using System.Threading.Tasks;

namespace Licenta_v01.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager _userManager;

        public UserController()
        {
            _userManager = new UserManager();
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            return View(_userManager.GetUser(id));
        }

        public ActionResult Edit(string id)
        {
            return View("Edit", _userManager.GetUser(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                await _userManager.UpdateUser(user);
                return View("Details", _userManager.GetUser(user.Id));
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Index()
        {
            List<UserViewModel> model = _userManager.GetAllUsers();
            return View(model);
        }
    }
}