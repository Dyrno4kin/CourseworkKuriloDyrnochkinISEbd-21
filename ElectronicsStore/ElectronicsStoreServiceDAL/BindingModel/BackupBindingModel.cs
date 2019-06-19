using System.Runtime.Serialization;

namespace ElectronicsStoreServiceDAL.BindingModel
{
    [DataContract]
    public class BackupBindingModel
    {
        [DataMember]
        public string FileName { get; set; }
    }
}
