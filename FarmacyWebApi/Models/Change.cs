using System;
using System.Collections.Generic;

namespace FarmacyWebApi.Models
{
    public partial class Change
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MedicineId { get; set; }
        public int Operation { get; set; }
        public DateTime ChangeDate { get; set; }

        public virtual Medicine Medicine { get; set; }
        public virtual User User { get; set; }
    }
}
