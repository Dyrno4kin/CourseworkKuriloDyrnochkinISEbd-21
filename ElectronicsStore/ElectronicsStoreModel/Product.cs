using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ElectronicsStoreModel
{   [DataContract]
    public class Product
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string ProductName { get; set; }

        [DataMember]
        [Required]
        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual List<IndentProduct> IndentProducts { get; set; }
    }
}
