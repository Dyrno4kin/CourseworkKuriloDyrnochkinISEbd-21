using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreModel
{
    public class OrderPayment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Required]
        public DateTime DatePayment { get; set; }

        [Required]
        public decimal SumPayment { get; set; }

        public virtual Order Order { get; set; }
    }
}
