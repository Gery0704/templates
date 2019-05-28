using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBank.Szervic.Models
{
        public class BankAccounts
        {
            public int ID { get; set; }
            [ForeignKey("Clients")]
            public int ClientID { get; set; }
            public string AccountNumber { get; set; }
            public int Balance { get; set; }
            public DateTime Created { get; set; }
            public Boolean isLocked { get; set; }

            public ICollection<Transactions> Transactions { get; set; }
        }
}
