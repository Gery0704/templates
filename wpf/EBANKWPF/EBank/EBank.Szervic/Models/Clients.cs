using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBank.Szervic.Models
{
    public class Clients : IdentityUser<int>
    {
        public string FullName { get; set; }
        public String PinCode { get; set; }
        public int IsEmployer { get; set; }
        public Boolean SessionSafety { get; set; }
        public string PrimaryAccountNumber { get; set; }
        public ICollection<BankAccounts> BankAccounts { get; set; }
    }
}
