using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.BindingModel
{
    [DataContract]
    public class IndentProductBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndentId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
