using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeBudgetManagement.Models
{
    //[Table("Item")] //commented out so all edm should only be configure inside dbcontext so it is easy to maintain and for separation of concern
    public class Item
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public byte[] File { get; set; }
        public string FileExtension { get; set; }
        public int ItemTypeId { get; set; }
    }
}
