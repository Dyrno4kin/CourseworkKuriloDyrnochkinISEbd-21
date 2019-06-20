using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class DataServiceDB : IDataServiceDB
    {
        private ElectronicsStoreDbContext context;
        public DataServiceDB(ElectronicsStoreDbContext context)
        {
            this.context = context;
        }

        public ProductViewModel PopularProduct()
        {
            var popularProduct = context.IndentProducts.GroupBy(rec => rec.ProductId)
                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) })
                .OrderByDescending(rec => rec.Total)
                .First();
            Product element = context.Products.FirstOrDefault(rec => rec.Id == popularProduct.Id);
            if (element != null)
            {
                return new ProductViewModel
                {
                    Id = element.Id,
                    ProductName = element.ProductName,
                    Price = element.Price,
                };
            }
            throw new Exception("Элемент не найден");
        }
    }
}
