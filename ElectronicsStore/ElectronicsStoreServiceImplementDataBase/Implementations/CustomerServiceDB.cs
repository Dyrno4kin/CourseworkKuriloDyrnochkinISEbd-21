using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class CustomerServiceDB : ICustomerService
    {
        private ElectronicsStoreDbContext context;
        public CustomerServiceDB(ElectronicsStoreDbContext context)
        {
            this.context = context;
        }
        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = context.Customers.Select(rec => new CustomerViewModel
            {
                Id = rec.Id,
                CustomerFIO = rec.CustomerFIO,
                Email = rec.Email
            })
            .ToList();
            return result;
        }
        public CustomerViewModel GetElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CustomerViewModel
                {
                    Id = element.Id,
                    CustomerFIO = element.CustomerFIO,
                    Email = element.Email
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Customers.Add(new Customer
            {
                CustomerFIO = model.CustomerFIO,
                Email = model.Email
            });
            context.SaveChanges();
        }
        public void UpdElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Customers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CustomerFIO = model.CustomerFIO;
            element.Email = model.Email;
            context.SaveChanges();
        }
        public void Block(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.CustomerFIO == model.CustomerFIO && rec.Id != model.Id);
            if (element != null)
            {
                element.CustomerStatus=true;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
