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
    [UsageInterface("Интерфейс для работы с продуктами")]
    public interface IProductService
    {
        [UsageMethod("Метод получения списка продуктов")]
        List<ProductViewModel> GetList();
        [UsageMethod("Метод получения продукта по id")]
        ProductViewModel GetElement(int id);
        [UsageMethod("Метод добавления продукта")]
        void AddElement(ProductBindingModel model);
        [UsageMethod("Метод изменения данных по продукту")]
        void UpdElement(ProductBindingModel model);
        [UsageMethod("Метод удаления продукта")]
        void DelElement(int id);
    }
}
