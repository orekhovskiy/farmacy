using System;
using System.Collections.Generic;

namespace Farmacy.ViewModels
{
    public partial class MedicineViewModel
    {
        public MedicineViewModel() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public string Category { get; set; }
        public string Form { get; set; }
        public int ShelfTime { get; set; }
        public int Count { get; set; }
        public ICollection<string> MedicineComposition { get; set; }
    }
}
