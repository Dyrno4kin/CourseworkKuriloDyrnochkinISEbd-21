using ElectronicsStoreServiceDAL;
using ElectronicsStoreServiceDAL.BindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.BindingModel
{
    [DataContract]
    public class ProductBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<IndentProductBindingModel> IndentProducts { get; set; }
    }
}
