using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace ElectronicsStoreServiceImplementDataBase.Implementations
{
    public class BackupServiceDB : IBackupService
    {
        private ElectronicsStoreDbContext context;

        public BackupServiceDB(ElectronicsStoreDbContext context)
        {
            this.context = context;
        }

        public void BackupToJson(BackupBindingModel model)
        {
            DataContractJsonSerializer formatterCustomer = new DataContractJsonSerializer(typeof(List<Customer>));
            MemoryStream msCustomer = new MemoryStream();
            formatterCustomer.WriteObject(msCustomer, context.Customers.ToList());
            msCustomer.Position = 0;
            StreamReader srCustomer = new StreamReader(msCustomer);
            string customersJSON = srCustomer.ReadToEnd();
            srCustomer.Close();
            msCustomer.Close();

            DataContractJsonSerializer formatterIndent = new DataContractJsonSerializer(typeof(List<Indent>));
            MemoryStream msIndent = new MemoryStream();
            formatterIndent.WriteObject(msIndent, context.Indents.ToList());
            msIndent.Position = 0;
            StreamReader srIndent = new StreamReader(msIndent);
            string indentsJSON = srIndent.ReadToEnd();
            srIndent.Close();
            msIndent.Close();

            DataContractJsonSerializer formatterProduct = new DataContractJsonSerializer(typeof(List<Product>));
            MemoryStream msProduct = new MemoryStream();
            formatterProduct.WriteObject(msProduct, context.Products.ToList());
            msProduct.Position = 0;
            StreamReader srProduct = new StreamReader(msProduct);
            string productsJSON = srProduct.ReadToEnd();
            srProduct.Close();
            msProduct.Close();

            DataContractJsonSerializer formatterIndentProduct = new DataContractJsonSerializer(typeof(List<IndentProduct>));
            MemoryStream msIndentProduct = new MemoryStream();
            formatterIndentProduct.WriteObject(msIndentProduct, context.IndentProducts.ToList());
            msIndentProduct.Position = 0;
            StreamReader srIndentProduct = new StreamReader(msIndentProduct);
            string indentProductsJSON = srIndentProduct.ReadToEnd();
            srIndentProduct.Close();
            msIndentProduct.Close();

            DataContractJsonSerializer formatterIndentPayment = new DataContractJsonSerializer(typeof(List<IndentPayment>));
            MemoryStream msIndentPayment = new MemoryStream();
            formatterIndentPayment.WriteObject(msIndentPayment, context.IndentPayments.ToList());
            msIndentPayment.Position = 0;
            StreamReader srIndentPayment = new StreamReader(msIndentPayment);
            string indentPaymentsJSON = srIndentPayment.ReadToEnd();
            srIndentPayment.Close();
            msIndentPayment.Close();

            File.WriteAllText(model.FileName, "{\n" +
                "    \"Customers\": " + customersJSON + ",\n" +
                "    \"Indents\": " + indentsJSON + ",\n" +
                "    \"Products\": " + productsJSON + ",\n" +
                "    \"IndentProducts\": " + indentProductsJSON + ",\n" +
                "    \"IndentPayments\": " + indentPaymentsJSON + ",\n" +
                "}");
        }

        public void BackupToXML()
        {
            DataContractJsonSerializer formatterCustomer = new DataContractJsonSerializer(typeof(List<Customer>));
            XmlWriter writerCustomer = new XmlTextWriter(@"D:\customer.xml", null);
            formatterCustomer.WriteObject(writerCustomer, context.Customers.ToList());
            writerCustomer.Close();

            DataContractJsonSerializer formatterIndent = new DataContractJsonSerializer(typeof(List<Indent>));
            XmlWriter writerIndent = new XmlTextWriter(@"D:\indent.xml", null);
            formatterIndent.WriteObject(writerIndent, context.Indents.ToList());
            writerIndent.Close();
            
            DataContractJsonSerializer formatterProduct = new DataContractJsonSerializer(typeof(List<Product>));
            XmlWriter writerProduct = new XmlTextWriter(@"D:\product.xml", null);
            formatterProduct.WriteObject(writerProduct, context.Products.ToList());
            writerProduct.Close();

            DataContractJsonSerializer formatterIndentProduct = new DataContractJsonSerializer(typeof(List<IndentProduct>));
            XmlWriter writerIndentProduct = new XmlTextWriter(@"D:\indentProduct.xml", null);
            formatterIndentProduct.WriteObject(writerIndentProduct, context.IndentProducts.ToList());
            writerIndentProduct.Close();

            DataContractJsonSerializer formatterIndentPayment = new DataContractJsonSerializer(typeof(List<IndentPayment>));
            XmlWriter writerIndentPayment= new XmlTextWriter(@"D:\indentPayment.xml", null);
            formatterIndentPayment.WriteObject(writerIndentPayment, context.IndentPayments.ToList());
            writerIndentPayment.Close();           
        }
    }
}