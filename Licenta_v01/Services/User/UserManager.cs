using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using ViewModels;
using System.Web.Mvc;

namespace Services.User
{
    public class UserManager
    {
        private readonly InternSeekerEntities _dbContext;

        public UserManager()
        {
            _dbContext = new InternSeekerEntities();
        }

        public UserViewModel GetUser(string id)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == id);
            var role = user.AspNetRoles.First();

            var personViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                EmailAddres = user.Email,
                SelectedRoleId = int.Parse(role.Id),
                Roles = GetSelectListItems(),
                Role = role.Name
            };
            return personViewModel;
        }

        public async Task UpdateUser(UserViewModel userForm)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == userForm.Id);
            var role = _dbContext.AspNetRoles.FirstOrDefault(r => r.Id == userForm.SelectedRoleId.ToString());

            if (user != null)
            {
                user.Email = userForm.EmailAddres;
                user.UserName = userForm.UserName;
                user.AspNetRoles.Clear();
                user.AspNetRoles.Add(role);
            }

            await _dbContext.SaveChangesAsync();
        }

        public List<UserViewModel> GetAllUsers()
        {
            var users = _dbContext.AspNetUsers.ToList();
            var viewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                viewModels.Add(
                    new UserViewModel
                    {
                        Id=user.Id,
                        EmailAddres = user.Email,
                        UserName = user.UserName
                    });
            }
            return viewModels;
        }

        public string GetIdByEmail(string email)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Email == email);
            return user.Id;
        }

        private IEnumerable<SelectListItem> GetSelectListItems()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem{
                    Value = 1.ToString(),
                    Text = "Administrator"
                },
                new SelectListItem{
                    Value = 2.ToString(),
                    Text = "Manager"
                },
                new SelectListItem{
                    Value = 3.ToString(),
                    Text = "General User"
                }
            };
            return selectList;
        }
    }
}
