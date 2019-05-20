using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.Attributies
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UsageMethodAttribute : Attribute
    {
        public UsageMethodAttribute(string descript)
        {
            Description = string.Format("Описание метода: ", descript);
        }
        public string Description { get; private set; }
    }
}
