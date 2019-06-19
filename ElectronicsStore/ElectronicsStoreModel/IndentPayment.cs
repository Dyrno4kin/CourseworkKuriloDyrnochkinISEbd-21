using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ElectronicsStoreModel
{   [DataContract]
    public class IndentPayment
    {
        [DataMember]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int IndentId { get; set; }

        [DataMember]
        [Required]
        public DateTime DatePayment { get; set; }

        [DataMember]
        [Required]
        public decimal SumPayment { get; set; }

        public virtual Indent Indent { get; set; }
    }
}
