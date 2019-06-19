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
    [UsageInterface("Интерфейс для работы с частичной оплатой")]
    public interface IIndentPaymentService
    {
        [UsageMethod("Метод получения списка оплат")]
        List<IndentPaymentViewModel> GetList();
        [UsageMethod("Метод получения оплаты по id")]
        IndentPaymentViewModel GetElement(int id);
        [UsageMethod("Метод добавления оплаты")]
        void AddElement(IndentPaymentViewModel model);
        [UsageMethod("Метод изменения оплат по заказу")]
        void UpdElement(IndentPaymentViewModel model);
        [UsageMethod("Метод удаления оплаты")]
        void DelElement(int id);
    }
}
