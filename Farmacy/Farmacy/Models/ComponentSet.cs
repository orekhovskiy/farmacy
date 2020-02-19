﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.Models
{
    public class ComponentSet
    {
        public IEnumerable<string> AvailableComponents { get; set; }

        public IEnumerable<string> CurrentComponents { get; set; }
    }
}