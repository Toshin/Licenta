using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Licenta_v01.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Page";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(ContactViewModel form)
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = ReCaptcha.Validate(EncodedResponse) == "True";

            if (IsCaptchaValid)
            {
                await SendInternshipApplicationEmail(form);
                return View("Index");
            }

            return View("Error", 
                new ErrorViewModel { ErrorMessage = "Captcha is incorrect! Please try again."});
        }

        public ActionResult Error(ErrorViewModel error)
        {
            ViewBag.Message = "Error page";
            return View(error);
        }

        public async Task SendInternshipApplicationEmail(ContactViewModel form)
        {
            var body = "<p>Dear Administrator, </p>" +
                       "<p> A new message has been submitted from the Contact page. <br/><br/>" +
                       "==User details== <br/>"+
                       "Name: {0}<br/>"+
                       "Surname: {1}<br/>" +
                       "Phone Number: {2}<br/>" +
                       "Email Address: {3}<br/>" +
                       "Message: {4}</p>";
            var emailMessage = new MailMessage();
            emailMessage.To.Add(new MailAddress("imapnah@gmail.com"));
            emailMessage.From = new MailAddress("imapnah@gmail.com");
            emailMessage.Subject = "New Feedback/Question";
            emailMessage.Body = string.Format(body, form.Name, form.Surname, form.PhoneNumber, form.EmailAddress, form.Message);
            emailMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(emailMessage);
            }
        }
    }
}