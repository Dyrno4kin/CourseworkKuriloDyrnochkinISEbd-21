using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    public class IndentViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFIO { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public string Status { get; set; }
        public string DateCreate { get; set; }
        public string DateImplement { get; set; }

        public virtual List<IndentProductViewModel> IndentProducts { get; set; }
        public virtual List<IndentPaymentViewModel> IndentPayments { get; set; }
    }
}

