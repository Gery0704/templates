using System;
using System.Collections.Generic;
using System.Text;

namespace EBank.Data
{
    public class BankAccountsDTO
    {
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string AccountNumber { get; set; }
        public int Balance { get; set; }
        public DateTime Created { get; set; }
        public Boolean isLocked { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is BankAccountsDTO dto) && ID == dto.ID;
        }
        
    }
}
