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
