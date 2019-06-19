using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class MainServiceDB : IMainService
    {
        private ElectronicsStoreDbContext context;
        public MainServiceDB(ElectronicsStoreDbContext context)
        {
            this.context = context;
        }
        public List<IndentViewModel> GetList()
        {
            List<IndentViewModel> result = context.Indents.Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                CustomerId = rec.CustomerId,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate),
                Status = rec.Status,
                Sum = rec.Sum,
                CustomerFIO = rec.Customer.CustomerFIO,

                IndentProducts = context.IndentProducts
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentProductViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    ProductId = recPC.ProductId,
                    Count = recPC.Count
                })
                .ToList(),

                IndentPayments = context.IndentPayments
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentPaymentViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    DatePayment = recPC.DatePayment,
                    SumPayment = recPC.SumPayment,
                })
                .ToList()
            })
            .ToList();
            return result;
        }

        public void SetStatus(IndentViewModel model)
        {
            Indent element = context.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            element.Status = IndentStatus.Оплачен;
            context.SaveChanges();
        }

        public List<IndentViewModel> GetListCustomer(int CustomerId)
        {
            List<IndentViewModel> result = context.Indents.Where(rec => rec.CustomerId == CustomerId).Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate),
                Status = rec.Status,
                Sum = rec.Sum,

                IndentProducts = context.IndentProducts
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentProductViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    ProductId = recPC.ProductId,
                    Count = recPC.Count
                })
                .ToList(),

                IndentPayments = context.IndentPayments
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentPaymentViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    DatePayment = recPC.DatePayment,
                    SumPayment = recPC.SumPayment,
                })
                .ToList()
            })
            .ToList();
            return result;
        }

        public void CreateIndent(IndentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Indent indent = new Indent
                    {
                        CustomerId = model.CustomerId,
                        DateCreate = DateTime.Now,
                        Status = IndentStatus.Принят,
                        Sum = model.Sum
                };
                    context.Indents.Add(indent);
                    context.SaveChanges();
                    var products = model.IndentProducts
                            .GroupBy(rec => rec.ProductId)
                           .Select(rec => new
                           {
                               ProductId = rec.Key,
                               Count = rec.Sum(r => r.Count)
                               
                           });
                    foreach (var product in products)
                    {
                        var IndentProd = new IndentProduct
                        {
                            IndentId = indent.Id,
                            ProductId = product.ProductId,
                            Count = product.Count
                        };
                        context.IndentProducts.Add(IndentProd);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public IndentViewModel GetElement(int id)
        {
            Indent rec = context.Indents.FirstOrDefault(rec1 => rec1.Id == id);
            if (rec != null)
            {
                return new IndentViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    DateCreate = rec.DateCreate.ToShortDateString(),
                    Status = rec.Status,
                    Sum = rec.Sum,
                    CustomerFIO = context.Customers.FirstOrDefault(rec1 => rec1.Id == rec.CustomerId).CustomerFIO,

                    IndentProducts = context.IndentProducts
                    .Where(recPC => recPC.IndentId == rec.Id)
                    .Select(recPC => new IndentProductViewModel
                    {
                        Id = recPC.Id,
                        IndentId = recPC.IndentId,
                        ProductId = recPC.ProductId,
                        Count = recPC.Count
                    })
                    .ToList(),

                    IndentPayments = context.IndentPayments
                    .Where(recPC => recPC.IndentId == rec.Id)
                    .Select(recPC => new IndentPaymentViewModel
                    {
                        Id = recPC.Id,
                        IndentId = recPC.IndentId,
                        DatePayment = recPC.DatePayment,
                        SumPayment = recPC.SumPayment
                    })
                    .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public int GetBalance(int id)
        {
            int balance = 0;
            Indent rec = context.Indents.FirstOrDefault(rec1 => rec1.Id == id);
            if (rec != null)
            {
                var indent = new IndentViewModel
                {
                    Sum = rec.Sum,

                    IndentPayments = context.IndentPayments
                .Where(recPC => recPC.IndentId == rec.Id)
                .Select(recPC => new IndentPaymentViewModel
                {
                    Id = recPC.Id,
                    IndentId = recPC.IndentId,
                    DatePayment = recPC.DatePayment,
                    SumPayment = recPC.SumPayment
                })
                .ToList()
                };
                balance = Convert.ToInt32(indent.Sum);

                foreach (var indentPayment in indent.IndentPayments)
                {
                    balance -= Convert.ToInt32(indentPayment.SumPayment);
                }

                return balance;
            }
            throw new Exception("Элемент не найден");
        }
    }
}
