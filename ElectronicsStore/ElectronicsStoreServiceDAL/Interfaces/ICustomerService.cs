using ElectronicsStoreServiceDAL.ViewModel;
using ElectronicsStoreServiceDAL.BindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicsStoreServiceDAL.Attributies;

namespace ElectronicsStoreServiceDAL.Interfaces
{
    [UsageInterface("Интерфейс для работы с клиентами")]
    public interface ICustomerService
    {
        [UsageMethod("Метод получения списка клиентов")]
        List<CustomerViewModel> GetList();
        [UsageMethod("Метод получения клиента по id")]
        CustomerViewModel GetElement(int id);
        [UsageMethod("Метод получения клиента по логину")]
        CustomerViewModel GetElement(string login);
        [UsageMethod("Метод добавления клиента")]
        void AddElement(CustomerBindingModel model);
        [UsageMethod("Метод изменения данных по клиенту")]
        void UpdElement(CustomerBindingModel model);
        [UsageMethod("Метод блокировки клиента")]
        void Block(int id);
        [UsageMethod("Метод просчёта бонусов")]
        void setBonus(int id);
    }
}
