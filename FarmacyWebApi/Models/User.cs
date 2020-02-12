﻿using System;
using System.Collections.Generic;

namespace FarmacyWebApi.Models
{
    public partial class User
    {
        public User()
        {
            Change = new HashSet<Change>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Position { get; set; }

        public virtual Position PositionNavigation { get; set; }
        public virtual ICollection<Change> Change { get; set; }
    }
}