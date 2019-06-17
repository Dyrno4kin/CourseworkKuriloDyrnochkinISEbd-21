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
    [UsageInterface("Интерфейс для работы с заказами")]
    public interface IMainService
    {
        [UsageMethod("Метод получения списка заказов")]
        List<IndentViewModel> GetList();
        [UsageMethod("Метод получения списка заказов клиента")]
        List<IndentViewModel> GetListCustomer(int CustomerId);
        [UsageMethod("Метод получения продукта по id")]
        IndentViewModel GetElement(int id);
        [UsageMethod("Метод добавления заказа")]
        void CreateIndent(IndentBindingModel model);
        int GetBalance(int id);
    }
}
