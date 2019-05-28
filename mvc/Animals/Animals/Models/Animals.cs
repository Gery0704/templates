using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animals.Models
{
    public class Animals
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Picture { get; set; }
        public Boolean free { get; set; }
    }
}
