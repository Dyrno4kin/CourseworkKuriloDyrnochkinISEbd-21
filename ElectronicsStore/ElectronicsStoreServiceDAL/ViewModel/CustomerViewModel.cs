using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    [DataContract]
    public class CustomerViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int Bonus { get; set; }
        [DataMember]
        public bool CustomerStatus { get; set; }
    }
}
