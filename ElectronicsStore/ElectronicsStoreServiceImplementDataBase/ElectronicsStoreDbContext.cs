using ElectronicsStoreModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicsStoreServiceImplementDataBase
{
    public class ElectronicsStoreDbContext : DbContext
    {
        public ElectronicsStoreDbContext() : base("ElectronicsStoreDatabase")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied =
           System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Indent> Indents { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<IndentProduct> IndentProducts { get; set; }
        public virtual DbSet<IndentPayment> IndentPayments { get; set; }
    }
}
