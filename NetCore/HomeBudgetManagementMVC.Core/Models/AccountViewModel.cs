using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.MVC.Core.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public double BalanceToAdd { get; set; }
    }
}
