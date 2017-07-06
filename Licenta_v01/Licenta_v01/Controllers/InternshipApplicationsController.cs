using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Services.InternshipApplication;
using ViewModels;

namespace Licenta_v01.Controllers
{
    public class InternshipApplicationsController : Controller
    {
        private readonly InternshipApplicationManager _appManager;

        public InternshipApplicationsController()
        {
            _appManager = new InternshipApplicationManager();
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Index()
        {
            List<InternshipApplicationViewModel> model = _appManager.GetAllApplications();
            return View(model);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Details(string id)
        {
            return View(_appManager.GetApplication(id));
        }

        public ActionResult Edit(string id)
        {
            return View("Edit", _appManager.GetApplication(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(InternshipApplicationViewModel app)
        {
            await _appManager.UpdateApplication(app);

            await _appManager.SendStatusEmail(app);

            return View("Details", _appManager.GetApplication(app.UserId));
        }

        [Authorize(Roles = "General User")]
        public ActionResult AppDetails(string id)
        {
            try
            {
                var app = _appManager.GetApplication(id);
                return View(app);
            }
            catch
            {
                return RedirectToAction("Error", "Home",
                    new ErrorViewModel { ErrorMessage = "The logged in user does not have an Internship Application!" });
            }

        }

        // GET: InternshipApplications
        [Authorize(Roles = "General User")]
        public ActionResult SubmitApplication()
        {
            var vm = new InternshipApplicationViewModel
            {
                Languages = _appManager.GetLanguages(),
                Technologies = _appManager.GetTechnologies()
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "General User")]
        public async Task<ActionResult> SubmitApplication(InternshipApplicationViewModel form)
        {
            try
            {
                form.UserId = User.Identity.GetUserId();
                await _appManager.Add(form);
                await _appManager.SendInternshipApplicationEmail(form);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home",
                    new ErrorViewModel { ErrorMessage = "You have already applied for the Internship!" });
            }

        }
    }
}