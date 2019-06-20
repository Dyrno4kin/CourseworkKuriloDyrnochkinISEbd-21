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
        [UsageMethod("Метод получения списка заказов для клиента")]
        List<IndentViewModel> GetCustomerIndentsForClients(ReptBindingModel model, int customerId);
        [UsageMethod("Метод получения списка информации по отчётам")]
        List<IndentPaymentViewModel> GetIndentInfo(ReptBindingModel model);
        [UsageMethod("Метод получения списка информации по отчётам для клиента")]
        List<IndentPaymentViewModel> GetIndentInfoForClients(ReptBindingModel model, int customerId);
        [UsageMethod("Метод получения отчёта по id")]
        void SaveCustomerIndents(ReptBindingModel model);
        [UsageMethod("Метод получения отчёта по заказам для клиента")]
        void SaveCustomerIndentsForClients(ReptBindingModel model, int customerId);
        [UsageMethod("Метод отправления писем")]
        void SendEmail(ReptBindingModel model);
    }
}
