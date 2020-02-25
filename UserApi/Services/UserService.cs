using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Commons;
using UserApi.Models;
using UserApi.Models.Context;
using UserApi.ViewModels;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserApiContext db;

        public UserService(UserApiContext context)
        {
            db = context;
        }

        public void AddUser(UserViewModel user)
        {
            var u = new User
            {
                Login = user.Login,
                Password = Hasher.GetHash(user.Password),
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Position = user.PositionId
            };
            db.User.Add(u);
            db.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db.User.Join(
                db.Position,
                u => u.Position,
                p => p.Id,
                (u, p) => new User
                {
                    Id = u.Id,
                    Login = u.Login,
                    Password = u.Password,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    Position = p.Id,
                    PositionNavigation = p
                }
            );
        }

        public User GetUser(string login, string password) =>GetAllUsers().Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).FirstOrDefault();

        public string GetUserPosition(int id) => db.Position.Find(db.User.Find(id).Position).Name;

        public int GetUserPositionId(int id) => db.User.Find(id).Position;
    }
}
