using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreModel
{
    public class IndentPayment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IndentId { get; set; }

        [Required]
        public DateTime DatePayment { get; set; }

        [Required]
        public decimal SumPayment { get; set; }

        public virtual Indent Indent { get; set; }
    }
}
