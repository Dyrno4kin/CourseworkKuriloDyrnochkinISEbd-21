using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class IndentPaymentDB : IIndentPaymentService
    {
        private ElectronicsStoreDbContext context;
        public IndentPaymentDB(ElectronicsStoreDbContext context)
        {
            this.context = context;
        }
        public List<IndentPaymentViewModel> GetList()
        {
            List<IndentPaymentViewModel> result = context.IndentPayments.Select(rec => new IndentPaymentViewModel
            {
                Id = rec.Id,
                IndentId = rec.IndentId,
                DatePayment = rec.DatePayment,
                SumPayment = rec.SumPayment
            })
            .ToList();
            return result;
        }
        public IndentPaymentViewModel GetElement(int id)
        {
            IndentPayment element = context.IndentPayments.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new IndentPaymentViewModel
                {
                    Id = element.Id,
                    IndentId = element.IndentId,
                    DatePayment = element.DatePayment,
                    SumPayment = element.SumPayment
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(IndentPaymentBindingModel model)
        {
            IndentPayment element = context.IndentPayments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть оплата с таким id");
            }
            context.IndentPayments.Add(new IndentPayment
            {
                Id = element.Id,
                IndentId = element.IndentId,
                DatePayment = DateTime.Now,
                SumPayment = element.SumPayment
            });
            context.SaveChanges();
        }
        public void UpdElement(IndentPaymentBindingModel model)
        {
            IndentPayment element = context.IndentPayments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть оплата с таким id");
            }
            element = context.IndentPayments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.SumPayment = model.SumPayment;
            context.SaveChanges();
        }
        public void DelElement(int id)
        {
            IndentPayment element = context.IndentPayments.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.IndentPayments.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
