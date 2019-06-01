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
    [UsageInterface("Интерфейс для работы с отчётами")]
    public interface IReptService
    {
        [UsageMethod("Метод добавления отчёта excel")]
        void SaveProductPriceXls(ReptBindingModel model);
        [UsageMethod("Метод добавления отчёта word")]
        void SaveProductPriceW(ReptBindingModel model);
        [UsageMethod("Метод получения списка отчётов")]
        List<CustomerIndentViewModel> GetCustomerIndents(ReptBindingModel model);
        [UsageMethod("Метод получения отчёта по id")]
        void SaveCustomerIndents(ReptBindingModel model);
    }
}
