using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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
        public string Password { get; set; }
        public string Email { get; set; }
        public int Bonus { get; set; }
        public bool CustomerStatus { get; set; }
    }
}
