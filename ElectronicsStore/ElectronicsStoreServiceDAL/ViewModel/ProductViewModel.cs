using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    [DataContract]
    public class ProductViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
    }
}
