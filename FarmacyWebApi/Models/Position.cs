using System;
using System.Collections.Generic;

namespace FarmacyWebApi.Models
{
    public partial class Position
    {
        public Position()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
