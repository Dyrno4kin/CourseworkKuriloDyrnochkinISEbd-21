using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicsStoreServiceDAL.BindingModel
{
    [DataContract]
    public class IndentPaymentBindingModel
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
