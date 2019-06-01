using ElectronicsStoreModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    [DataContract]
    public class IndentViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public IndentStatus Status { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DateImplement { get; set; }
        [DataMember]
        public virtual List<IndentProductViewModel> IndentProducts { get; set; }
        [DataMember]
        public virtual List<IndentPaymentViewModel> IndentPayments { get; set; }
    }
}

