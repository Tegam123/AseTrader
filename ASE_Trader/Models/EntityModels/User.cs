using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ASE_Trader.Models.Roles;

namespace ASE_Trader.Models.EntityModels
{
    public class User
    {
        [Key] public long UserId { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Password")]
        public string PwHash { get; set; }

        [Display(Name = "Account type")]
        public Role AccountType { get; set; }




    }
}