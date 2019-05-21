using System;
using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    [DataContract]
    public class IndentPaymentViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndentId { get; set; }
        [DataMember]
        public DateTime DatePayment { get; set; }
        [DataMember]
        public decimal SumPayment { get; set; }
    }
}