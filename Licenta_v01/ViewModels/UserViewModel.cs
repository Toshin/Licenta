using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddres { get; set; }
        public string Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public int SelectedRoleId { get; set; }
    }
}
