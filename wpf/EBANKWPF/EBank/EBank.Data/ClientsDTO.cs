using System;
using System.Collections.Generic;

namespace EBank.Data
{
    public class ClientsDTO
    {
        public string FullName { get; set; }
        public String PinCode { get; set; }
        public int IsEmployer { get; set; }
        public Boolean SessionSafety { get; set; }
        public string PrimaryAccountNumber { get; set; }
        public List<BankAccountsDTO> BankAccounts { get; set; }
        public ClientsDTO()
        {
            BankAccounts = new List<BankAccountsDTO>();
        }
    }
}
