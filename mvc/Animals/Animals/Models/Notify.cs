using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Animals.Models
{
    public class Notify
    {
        public int ID { get; set; }
        public int PeopleID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public string AnimalType { get; set; }
    }
}
