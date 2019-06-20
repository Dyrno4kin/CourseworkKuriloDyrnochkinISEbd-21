using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
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

                return new ProductViewModel
                {
                    Id = element.Id,
                    ProductName = element.ProductName,
                    Price = element.Price,
                };
        }

        public IndentViewModel RecommendedIndent(int CustomerId)
        {
            return context.Indents.Where(rec => rec.CustomerId == CustomerId && rec.Status != IndentStatus.Оплачен).Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate)
           + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) +
           " " +
            SqlFunctions.DateName("yyyy",
           rec.DateCreate),
                Status = rec.Status,
                Sum = rec.Sum,
            })
            .OrderBy(rec => rec.DateCreate).ThenByDescending(rec => rec.Sum)
            .First();
        }

        public List<IndentViewModel> DataIndent()
        {
            int max = DateTime.Now.Month;
            int min = DateTime.Now.AddMonths(-1).Month;
            var indents = context.Indents
           .Include(rec => rec.IndentProducts)
           .Where(rec => rec.DateCreate.Month > min &&
           rec.DateCreate.Month <= max)
            .Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate)
           + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) +
           " " +
            SqlFunctions.DateName("yyyy",
           rec.DateCreate),
                
                IndentProducts = context.IndentProducts
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentProductViewModel
                {
                    IndentId = recPC.IndentId,
                    ProductId = recPC.ProductId,
                    Count = recPC.Count
                })
                .ToList()
                
            }).ToList();

            return indents;
        }
    }
}
