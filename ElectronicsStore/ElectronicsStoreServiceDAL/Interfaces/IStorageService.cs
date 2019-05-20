using ElectronicsStoreServiceDAL.Attributies;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.Interfaces
{
    [UsageInterface("Интерфейс для работы с складами")]
    public interface IStorageService
    {
        [UsageMethod("Метод получения списка складов")]
        List<StorageViewModel> GetList();
        [UsageMethod("Метод получения склада по id")]
        StorageViewModel GetElement(int id);
        [UsageMethod("Метод добавления склада")]
        void AddElement(StorageBindingModel model);
        [UsageMethod("Метод изменения данных по складу")]
        void UpdElement(StorageBindingModel model);
        [UsageMethod("Метод удаления склада")]
        void DelElement(int id);
    }
}
