using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.Attributies
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class UsageInterfaceAttribute : Attribute
    {
        public UsageInterfaceAttribute(string descript)
        {
            Description = string.Format("Описание интерфейса: ", descript);
        }
        public string Description { get; private set; }
    }
}
