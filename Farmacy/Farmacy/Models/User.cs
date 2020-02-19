using System;
using System.Collections.Generic;

namespace Farmacy.Models
{
    public partial class User
    {
        public User()
        {
            Purchase = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Position { get; set; }

        public virtual Position PositionNavigation { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }
    }
}
