﻿using System;
using System.Collections.Generic;

namespace MedicineApi.Models
{
    public partial class Producer
    {
        public Producer()
        {
            Medicine = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Medicine> Medicine { get; set; }
    }
}
