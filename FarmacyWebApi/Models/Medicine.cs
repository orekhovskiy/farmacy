using System;
using System.Collections.Generic;

namespace FarmacyWebApi.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            Change = new HashSet<Change>();
            MedicineComposition = new HashSet<MedicineComposition>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProducerId { get; set; }
        public int? CategoryId { get; set; }
        public int? FormId { get; set; }
        public int Count { get; set; }

        public virtual Category Category { get; set; }
        public virtual Form Form { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual ICollection<Change> Change { get; set; }
        public virtual ICollection<MedicineComposition> MedicineComposition { get; set; }
    }
}
