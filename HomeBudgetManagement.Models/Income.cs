using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Models
{
    public class Income
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public byte[] File { get; set; }
        public string FileExtension { get; set; }

    }
}
