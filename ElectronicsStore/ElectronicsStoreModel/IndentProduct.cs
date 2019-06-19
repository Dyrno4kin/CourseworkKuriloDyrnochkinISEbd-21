using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ElectronicsStoreModel
{   [DataContract]
    public class IndentProduct
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public int IndentId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Count { get; set; }

        public virtual Indent Indent { get; set; }

        public virtual Product Product { get; set; }
    }
}
