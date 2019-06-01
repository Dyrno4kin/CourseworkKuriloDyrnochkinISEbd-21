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

        public void CreateIndent(IndentBindingModel model)
        {
            var indent = new Indent
            {
                CustomerId = model.CustomerId,
                DateCreate = DateTime.Now,
                Sum = model.Sum,
                Customer = context.Customers.FirstOrDefault(rec => rec.Id == model.CustomerId)
            };
            context.Indents.Add(indent);
            context.SaveChanges();
            var products = model.IndentProducts
                    .GroupBy(rec => rec.Id)
                   .Select(rec => new
                   {
                       IndentId = rec.Key,
                       Count = rec.Sum(r => r.Count)
                   });

            foreach (var product in products)
            {
                var IndentProd = new IndentProduct
                {
                    ProductId = product.IndentId,
                    
                    IndentId = indent.Id,
                    Count = product.Count
                };
                context.IndentProducts.Add(IndentProd);
                context.SaveChanges();
            }
            context.SaveChanges();
            var customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
           // SendEmail(customer.Email, "Оповещение по заказам", $"Заказ №{indent.Id} от {indent.DateCreate.ToShortDateString()} создан успешно");
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                string login = ConfigurationManager.AppSettings["MailLogin"];
                objMailMessage.From = new
               MailAddress(login);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new
               NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
               ConfigurationManager.AppSettings["MailPassword"]);
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
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
                    SumPayment = recPC.SumPayment
                })
                .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public int GetBalance(int id)
        {
            int balance=0;
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
