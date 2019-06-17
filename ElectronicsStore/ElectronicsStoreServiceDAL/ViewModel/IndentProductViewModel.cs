using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    [DataContract]
    public class IndentProductViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndentId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
