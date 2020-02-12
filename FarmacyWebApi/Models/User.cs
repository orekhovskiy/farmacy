using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmacyWebApi.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int position { get; set; }
    }
}
