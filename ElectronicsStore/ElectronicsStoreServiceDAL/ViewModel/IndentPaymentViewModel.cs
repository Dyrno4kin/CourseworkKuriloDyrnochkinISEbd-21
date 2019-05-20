using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    public class IndentPaymentViewModel
    {
        public int Id { get; set; }
        public int IndentId { get; set; }
        public DateTime DatePayment { get; set; }
        public decimal SumPayment { get; set; }
    }
}