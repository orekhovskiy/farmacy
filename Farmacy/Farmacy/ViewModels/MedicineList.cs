using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.ViewModels
{
    public class MedicineList
    {
        public int PagesAmount { get; set; }
        public int CurrentPage { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
