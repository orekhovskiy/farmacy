using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmacyWebApi.Models
{
    public class MedicineList
    {
        public int PagesAmount;
        public int CurrentPage;

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
