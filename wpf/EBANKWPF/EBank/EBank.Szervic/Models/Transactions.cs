using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBank.Szervic.Models
{
    public class Transactions
    {
        public int ID { get; set; }
        [ForeignKey("BankAccounts")]
        public int AccountID { get; set; }
        public String TransactionType { get; set; }
        public string AccountNumberFrom { get; set; }
        public string AccountNumberTo { get; set; }
        public string ReceiverName { get; set; }
        public int TransactionValue { get; set; }
        public DateTime Created { get; set; }
    }
}
