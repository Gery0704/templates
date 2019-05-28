using System;
using System.Collections.Generic;
using System.Text;

namespace EBank.Data
{
    public class TransactionDTO
    {
        public int ID { get; set; }
        public int AccountID { get; set; }
        public String TransactionType { get; set; }
        public string AccountNumberFrom { get; set; }
        public string AccountNumberTo { get; set; }
        public string ReceiverName { get; set; }
        public int TransactionValue { get; set; }
        public DateTime Created { get; set; }
    }
}
