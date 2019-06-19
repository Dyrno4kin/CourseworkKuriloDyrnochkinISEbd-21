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
                Email = rec.Email,
                Bonus = rec.Bonus,
                Password = rec.Password,
                Login = rec.Login,
                CustomerStatus = rec.CustomerStatus
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
                    Email = element.Email,
                    Bonus = element.Bonus,
                    Password = element.Password,
                    Login = element.Login,
                    CustomerStatus = element.CustomerStatus,

                    Indents = context.Indents
                .Where(recPC => recPC.CustomerId == element.Id)
                .Select(recPC => new IndentViewModel
                {
                    Id = recPC.Id,
                    DateCreate = recPC.DateCreate.ToString(),
                    CustomerId = recPC.CustomerId,
                    Status = recPC.Status,
                    Sum = recPC.Sum
                })
                .ToList(),


                };
            }
            throw new Exception("Элемент не найден");
        }
        public CustomerViewModel GetElement(string login)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Login == login);
            if (element != null)
            {
                return new CustomerViewModel
                {
                    Id = element.Id,
                    CustomerFIO = element.CustomerFIO,
                    Email = element.Email,
                    Bonus = element.Bonus,
                    Password = element.Password,
                    Login = element.Login,
                    CustomerStatus = element.CustomerStatus
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
            element = new Customer
            {
                DateRegistration = DateTime.Now,
                Password = model.Password,
                Login = model.Login,
                CustomerStatus = false,
                Bonus = 0,
                CustomerFIO = model.CustomerFIO,
                Email = model.Email
            };
            context.Customers.Add(element);
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
        public void Block(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                element.CustomerStatus = !element.CustomerStatus;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public void setBonus(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                var indents = context.Indents.Where(recPC => recPC.CustomerId == id).Select(rec => new IndentViewModel
                {
                    Sum = rec.Sum,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate)
                   + " " +
                    SqlFunctions.DateName("mm", rec.DateCreate) +
                   " " +
                    SqlFunctions.DateName("yyyy",
                   rec.DateCreate),

                    IndentPayments = context.IndentPayments
                    .Where(recPC => recPC.IndentId == rec.Id)
                    .Select(recPC => new IndentPaymentViewModel
                    {
                        DatePayment = recPC.DatePayment
                    })
                    .ToList()
                }).ToList();

                if (indents.Select(x => x.Sum).Sum() >= 25000)
                {
                    element.Bonus = 5;
                }
                if (indents.Select(x => x.Sum).Sum() >= 75000)
                {
                    element.Bonus = 10;
                }
                if (indents.Select(x => x.Sum).Sum() >= 150000)
                {
                    element.Bonus = 20;
                }
                foreach (var indent in indents)
                {
                    if (indent.IndentPayments.Count == 0)
                    {
                        if ((indent.Status != IndentStatus.Оплачен) &&
                            (DateTime.ParseExact(indent.DateCreate.Replace(" ", ""), "ddMMMMyyyy", null).AddDays(8) < DateTime.Now))
                        {
                            element.CustomerStatus = true;
                        }
                    }
                    else{
                        if ((indent.Status != IndentStatus.Оплачен) && (indent.IndentPayments.ToList().Last().DatePayment.Value.AddDays(8) < DateTime.Now))
                        {
                            element.CustomerStatus = true;
                        }
                    }
                }
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
