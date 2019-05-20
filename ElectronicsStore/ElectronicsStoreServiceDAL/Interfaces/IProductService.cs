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
    [UsageInterface("Интерфейс для работы с пиццами")]
    public interface IProductService
    {
        [UsageMethod("Метод получения списка продуктов")]
        List<ProductViewModel> GetList();
        [UsageMethod("Метод получения продукта по id")]
        ProductViewModel GetElement(int id);
        [UsageMethod("Метод добавления пицц")]
        void AddElement(ProductBindingModel model);
        [UsageMethod("Метод изменения данных по пицце")]
        void UpdElement(ProductBindingModel model);
        [UsageMethod("Метод удаления пицц")]
        void DelElement(int id);
    }
}
