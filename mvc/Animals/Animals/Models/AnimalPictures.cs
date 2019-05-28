using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animals.Models
{
    public class AnimalPictures
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AnimalType { get; set; }
        public byte[] Picture { get; set; }
        public string PicturePath { get; set; }
    }
}
