using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.ViewModel
{
    public class IndentProductViewModel
    {
        public int Id { get; set; }
        public int IndentId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
