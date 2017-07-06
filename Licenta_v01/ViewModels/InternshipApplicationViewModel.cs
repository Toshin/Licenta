using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class InternshipApplicationViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<SelectListItem> Languages { set; get; }
        public int[] SelectedLanguages { set; get; }
        public string SelectedL { get; set; }
        public IEnumerable<SelectListItem> Technologies { set; get; }
        public int[] SelectedTechnologies { set; get; }
        public string SelectedT { get; set; }

        [DataType(DataType.MultilineText)]
        public string PersonalProjects { get; set; }

        public string University { get; set; }
        public string Faculty { get; set; }
        public int? StudyYear { get; set; }

        [UIHint("YesNo")]
        public bool WantsToBeHired { get; set; }

        [DataType(DataType.MultilineText)]
        public string RecommendForInternship { get; set; }

        public IEnumerable<SelectListItem> Statuses { set; get; }
        public int SelectedStatusId { set; get; }
        public string Status { get; set; }
        public string Date { get; set; }
    }
}
