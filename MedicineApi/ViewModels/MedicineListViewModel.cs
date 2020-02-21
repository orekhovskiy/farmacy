using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineApi.ViewModels
{
    public class MedicineListViewModel
    {
        public int PagesAmount { get; set; }
        public int CurrentPage { get; set; }

        public virtual ICollection<MedicineViewModel> Medicines { get; set; }
    }
}
