using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess;
using ViewModels;

namespace Services.InternshipApplication
{
    public class InternshipApplicationManager
    {
        private readonly InternSeekerEntities _dbContext;

        public InternshipApplicationManager()
        {
            _dbContext = new InternSeekerEntities();
        }

        public async Task Add(InternshipApplicationViewModel appForm)
        {
            var app = _dbContext.InternshipApplications.FirstOrDefault(a => a.UserId == appForm.UserId);
            var languages = appForm.SelectedLanguages;
            var technologies = appForm.SelectedTechnologies;

            if (app == null)
            {
                var newApp = new DataAccess.InternshipApplication
                {
                    UserId = appForm.UserId,
                    Name = appForm.Name,
                    Surname = appForm.Surname,
                    PhoneNumber = appForm.PhoneNumber,
                    PersonalProjects = appForm.PersonalProjects,
                    University = appForm.University,
                    Faculty = appForm.Faculty,
                    StudyYear = appForm.StudyYear,
                    WantsToBeHired = appForm.WantsToBeHired,
                    RecommendForInternship = appForm.RecommendForInternship,
                    Status = "Pending",
                    ScheduleDate = "Not scheduled"
                };

                foreach (var langId in languages)
                {
                    var language = _dbContext.Languages.FirstOrDefault(l => l.Id == langId);
                    newApp.Languages.Add(language);
                }

                foreach (var techId in technologies)
                {
                    var technology = _dbContext.Technologies.FirstOrDefault(l => l.Id == techId);
                    newApp.Technologies.Add(technology);
                }

                _dbContext.InternshipApplications.Add(newApp);
                await _dbContext.SaveChangesAsync();
            }
            else throw new Exception();
        }

        public InternshipApplicationViewModel GetApplication(string id)
        {
            var app = _dbContext.InternshipApplications.FirstOrDefault(u => u.UserId == id);
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == id);
            var status = GetStatuses().FirstOrDefault(s => s.Text == app.Status);

            var selectedLangs = new List<string>();
            foreach (var lang in app.Languages)
            {
                selectedLangs.Add(lang.Name);
            }

            var selectedTechs = new List<string>();
            foreach (var tech in app.Technologies)
            {
                selectedTechs.Add(tech.Name);
            }

            var appViewModel = new InternshipApplicationViewModel
            {
                UserId = app.UserId,
                UserEmail = user.Email,
                Name = app.Name,
                Surname = app.Surname,
                PhoneNumber = app.PhoneNumber,
                PersonalProjects = app.PersonalProjects,
                University = app.University,
                Faculty = app.Faculty,
                StudyYear = app.StudyYear,
                WantsToBeHired = app.WantsToBeHired,
                RecommendForInternship = app.RecommendForInternship,
                SelectedL = string.Join(", ", selectedLangs.ToArray()),
                SelectedT = string.Join(", ", selectedTechs.ToArray()),
                Statuses = GetStatuses(),
                SelectedStatusId = int.Parse(status.Value),
                Status = status.Text,
                Date = app.ScheduleDate
            };
            return appViewModel;
        }

        public List<InternshipApplicationViewModel> GetAllApplications()
        {
            var apps = _dbContext.InternshipApplications.ToList();
            var viewModels = new List<InternshipApplicationViewModel>();
            foreach (var app in apps)
            {
                string date = app.ScheduleDate;
                var status = GetStatuses().FirstOrDefault(s => s.Text == app.Status);

                var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == app.UserId);
                viewModels.Add(
                    new InternshipApplicationViewModel
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        Name = app.Name,
                        Surname = app.Surname,
                        PhoneNumber = app.PhoneNumber,
                        PersonalProjects = app.PersonalProjects,
                        University = app.University,
                        Faculty = app.Faculty,
                        StudyYear = app.StudyYear,
                        WantsToBeHired = app.WantsToBeHired,
                        RecommendForInternship = app.RecommendForInternship,
                        Status = status.Text,
                        Date = date
                    });
            }
            return viewModels;
        }

        public async Task UpdateApplication(InternshipApplicationViewModel appForm)
        {
            var app = _dbContext.InternshipApplications.FirstOrDefault(a => a.UserId == appForm.UserId);
            var status = GetStatuses().FirstOrDefault(s => s.Value == appForm.SelectedStatusId.ToString());

            if (app != null)
            {
                app.Status = status.Text;
                app.ScheduleDate = appForm.Date;
            }

            await _dbContext.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetLanguages()
        {
            var langs = _dbContext.Languages.ToList();
            var selectList = new List<SelectListItem>();
            foreach (var lang in langs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = lang.Id.ToString(),
                    Text = lang.Name
                });
            }
            return selectList;
        }

        public IEnumerable<SelectListItem> GetTechnologies()
        {
            var techs = _dbContext.Technologies.ToList();
            var selectList = new List<SelectListItem>();
            foreach (var tech in techs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = tech.Id.ToString(),
                    Text = tech.Name
                });
            }
            return selectList;
        }

        private IEnumerable<SelectListItem> GetStatuses()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem{
                    Value = 1.ToString(),
                    Text = "Accepted"
                },
                new SelectListItem{
                    Value = 2.ToString(),
                    Text = "Rejected"
                },
                new SelectListItem{
                    Value = 3.ToString(),
                    Text = "Pending"
                }
            };
            return selectList;
        }

        public async Task SendStatusEmail(InternshipApplicationViewModel form)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == form.UserId);
            var status = GetStatuses().FirstOrDefault(s => s.Value == form.SelectedStatusId.ToString());

            var body = "<p>Dear {0}, </p><p>{1}</p>";
            var emailMessage = new MailMessage();
            emailMessage.To.Add(new MailAddress("imapnah@gmail.com"));
            emailMessage.From = new MailAddress("imapnah@gmail.com");
            emailMessage.Subject = "Internship Application Update";
            string message = "Your Internship Application is still on pending.<br/><br/>" +
                           "Best regards, <br/>" +
                           "InternRecruiter."; ;
            if (status.Text == "Accepted" && form.Date != "Not scheduled")
            {
                message = "Congratulations!<br/><br/>" +
                              "The Human Resources team has Accepted your Internship Application.<br/>" +
                              "You are scheduled for a Technical Test at our office on " + form.Date + "<br/><br/>" +
                              "If the date doesn't suit you, please use our Contact page and contact us to change the date for the test. <br/><br/>" +
                              "Best regards, <br/>" +
                              "InternRecruiter.";
            }
            else if (status.Text == "Accepted" && form.Date == "Not scheduled")
            {
                message = "Congratulations!<br/><br/>" +
                              "The Human Resources team has Accepted your Internship Application.<br/>" +
                              "You are not scheduled for a Technical Test yet but you will be notified with another email when the Human Resources" +
                              " team schedules you. <br/><br/>" +
                              "Best regards, <br/>" +
                              "InternRecruiter.";
            }
            else if (status.Text == "Rejected")
            {
                message = "We are sorry to tell you but your Internship Application has been Rejected.<br/>" +
                          "We wish you good luck and we hope to see you again! <br/><br/>" +
                          "Best regards, <br/>" +
                          "InternRecruiter.";
            }

            emailMessage.Body = string.Format(body, user.UserName, message);
            emailMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                //var credential = new NetworkCredential
                //{
                //    UserName = "internrecruiter@mail.com",  // replace with valid value
                //    Password = "Int3rnR#cruiter"  // replace with valid value
                //};
                //smtp.Credentials = credential;
                //smtp.Host = "smtp.mail.com";
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                await smtp.SendMailAsync(emailMessage);
            }
        }

        public async Task SendInternshipApplicationEmail(InternshipApplicationViewModel form)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == form.UserId);

            var body = "<p>Dear {0}, </p><p>{1}</p>";
            var emailMessage = new MailMessage();
            emailMessage.To.Add(new MailAddress(user.Email)); 
            emailMessage.From = new MailAddress("internrecruiter@mail.com");
            emailMessage.Subject = "New Internship Application";
            var message = "Thank you for submitting an Internship Application. <br/><br/> " +
                          "The Human Resources team will take a look at your application and will give you an answer it as soon as possible, " +
                          "after the application has been Accepted or Rejected, you will be notified through an email.<br/><br/>" +
                          "Best regards, <br/>" +
                          "InternRecruiter.";
            emailMessage.Body = string.Format(body, user.UserName, message);
            emailMessage.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(emailMessage);
            }
        }
    }
}
