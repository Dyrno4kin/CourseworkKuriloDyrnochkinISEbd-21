using ElectronicsStoreServiceDAL.Attributies;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceDAL.Interfaces
{
    [UsageInterface("Интерфейс для работы с клиентами")]
    public interface IDataServiceDB
    {
        [UsageMethod("Метод gjлучения популярного продукта")]
        ProductViewModel PopularProduct();
        [UsageMethod("Метод gjлучения популярного продукта")]
        IndentViewModel RecommendedIndent(int id);
    }
}
