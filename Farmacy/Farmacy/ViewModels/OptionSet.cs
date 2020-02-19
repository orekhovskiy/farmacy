using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.ViewModels
{
    public class OptionSet
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public ICollection<string> Options { get; set; }
    }
}
