using System;
using System.Collections.Generic;

namespace UserApi.Models
{
    public partial class MedicineComposition
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int ComponentId { get; set; }

        public virtual Component Component { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
