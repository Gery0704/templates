using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animals.Models
{
    public class AnimalPicModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AnimalType { get; set; }
        public IFormFile Picture { get; set; }
        public string PicturePath { get; set; }
    }
}
