using System;
using System.Collections.Generic;

namespace UserApi.Models
{
    public partial class Category
    {
        public Category()
        {
            Medicine = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}
