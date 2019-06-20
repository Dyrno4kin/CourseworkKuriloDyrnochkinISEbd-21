using ElectronicsStoreServiceDAL.Attributies;
using ElectronicsStoreServiceDAL.BindingModel;

namespace ElectronicsStoreServiceDAL.Interfaces
{
    [UsageInterface("Интерфейс для бэкапа базы данных")]
    public interface IBackupService
    {
        [UsageMethod("Метод создания бэкапа БД в формате JSON")]
        void BackupToJson(BackupBindingModel model);

        [UsageMethod("Метод создания бэкапа БД в формате XML")]
        void BackupToXML();
    }
}
