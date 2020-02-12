using System;
using System.Collections.Generic;

namespace FarmacyWebApi.Models
{
    public partial class Form
    {
        public Form()
        {
            Medicine = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}
