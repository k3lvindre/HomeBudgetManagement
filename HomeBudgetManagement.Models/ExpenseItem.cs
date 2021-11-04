using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Models
{
    public class ExpenseItem
    {
        public int ExpenseItemId { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public byte[] File { get; set; }
        public string FileExtension { get; set; }
        public int ExpenseItemTypeId { get; set; }
    }
}
