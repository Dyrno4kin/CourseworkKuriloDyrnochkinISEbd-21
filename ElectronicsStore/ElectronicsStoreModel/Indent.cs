using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsStoreModel
{
    public class Indent
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public decimal Sum { get; set; }

        public IndentStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public virtual Customer Customer { get; set; }

        [ForeignKey("IndentId")]
        public virtual List<IndentProduct> IndentProducts { get; set; }

        [ForeignKey("IndentId")]
        public virtual List<IndentPayment> IndentPayments { get; set; }
    }
}
