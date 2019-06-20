using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceImplementDataBase;
using ElectronicsStoreServiceImplementDataBase.Implementations;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace ElectronicsStoreAdminView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormAuthorization>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();

            currentContainer.RegisterType<DbContext, ElectronicsStoreDbContext>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIndentPaymentService, IndentPaymentDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IProductService, ProductServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReptService, ReptServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBackupService, BackupServiceDB>(new 
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDataServiceDB, DataServiceDB>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
