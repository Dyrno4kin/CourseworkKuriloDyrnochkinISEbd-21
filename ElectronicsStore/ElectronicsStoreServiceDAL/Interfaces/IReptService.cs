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
        [UsageMethod("Метод получения списка заказов")]
        List<IndentViewModel> GetCustomerIndents(ReptBindingModel model);
        [UsageMethod("Метод получения списка информации по отчётам")]
        List<IndentPaymentViewModel> GetIndentInfo(ReptBindingModel model);
        [UsageMethod("Метод получения отчёта по id")]
        void SaveCustomerIndents(ReptBindingModel model);
        [UsageMethod("Метод отправления писем")]
        void SendEmail(ReptBindingModel model);
    }
}
