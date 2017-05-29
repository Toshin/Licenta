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

        public string GetUserRole(string id)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == id);
            var role = _dbContext.AspNetRoles.FirstOrDefault(r => r.Id == user.RoleID);
            return role.Name;
        }

        public UserViewModel GetUser(string id)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == id);
            var role = _dbContext.AspNetRoles.FirstOrDefault(r => r.Id == user.RoleID);

            var personViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                EmailAddres = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = role.Name,
                SelectedRoleId = int.Parse(role.Id),
                Roles = GetSelectListItems()
            };
            return personViewModel;
        }

        public async Task UpdateUser(UserViewModel userForm)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == userForm.Id);

            if (user != null)
            {
                user.Name = userForm.Name;
                user.Surname = userForm.Surname;
                user.PhoneNumber = userForm.PhoneNumber;
                user.Email = userForm.EmailAddres;
                user.RoleID = userForm.SelectedRoleId.ToString();
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
                        Name =user.Name,
                        Surname = user.Surname,
                        EmailAddres = user.Email,
                        PhoneNumber = user.PhoneNumber
                    });
            }
            return viewModels;
        }

        public void Add(UserViewModel userForm)
        {
            var user = _dbContext.AspNetUsers.FirstOrDefault(u => u.Id == userForm.Id);

            if (user != null)
            {
                user.Name = userForm.Name;
                user.Surname = userForm.Surname;
                user.PhoneNumber = userForm.PhoneNumber;
                user.Email = userForm.EmailAddres;
            }

            _dbContext.SaveChanges();
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
