using System;
using System.Collections.Generic;

namespace UserApi.Models
{
    public partial class Component
    {
        public Component()
        {
            MedicineComposition = new HashSet<MedicineComposition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MedicineComposition> MedicineComposition { get; set; }
    }
}
