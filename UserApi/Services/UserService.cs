using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Commons;
using UserApi.Models;
using UserApi.Models.Context;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserApiContext db;

        public UserService(UserApiContext context)
        {
            db = context;
        }

        public void AddUser(string login, string password, string firstname, string lastname, int position)
        {
            var u = new User
            {
                Login = login,
                Password = Hasher.GetHash(password),
                Firstname = firstname,
                Lastname = lastname,
                Position = position
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

        public User GetUser(string login, string password) => GetAllUsers().Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).First();

        public string GetUserPosition(int id) => db.Position.Find(db.User.Find(id).Position).Name;

        public int GetUserPositionId(int id) => db.User.Find(id).Position;
    }
}
