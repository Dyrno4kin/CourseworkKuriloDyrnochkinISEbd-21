using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ElectronicsStoreModel
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string CustomerFIO { get; set; }

        [DataMember]
        [Required]
        public string Login { get; set; }

        [DataMember]
        [Required]
        public string Password { get; set; }

        [DataMember]
        [Required]
        public string Email { get; set; }

        [DataMember]
        [Required]
        public DateTime DateRegistration { get; set; }

        [DataMember]
        [Required]
        public int Bonus { get; set; }

        [DataMember]
        [Required]
        public bool CustomerStatus { get; set; }

        [ForeignKey("CustomerId")]
        public virtual List<Indent> Indents { get; set; }
    }
}
