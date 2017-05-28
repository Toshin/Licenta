using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using ViewModels;

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
            var personViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                EmailAddres = user.Email,
                PhoneNumber = user.PhoneNumber
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
    }
}
