using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ElectronicsStoreModel
{  [DataContract] 
    public class Indent
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int CustomerId { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public IndentStatus Status { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        public virtual Customer Customer { get; set; }

        [ForeignKey("IndentId")]
        public virtual List<IndentProduct> IndentProducts { get; set; }
        public virtual List<IndentPayment> IndentPayments { get; set; }
    }
}
