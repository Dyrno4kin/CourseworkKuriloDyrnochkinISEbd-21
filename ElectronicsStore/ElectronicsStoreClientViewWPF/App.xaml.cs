using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceImplementDataBase;
using ElectronicsStoreServiceImplementDataBase.Implementations;
using System;
using System.Data.Entity;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace ElectronicsStoreClientViewWPF
{
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            App app = new App();
            var container = BuildUnityContainer();
            app.Run(container.Resolve<FormStart>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, ElectronicsStoreDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IProductService, ProductServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IIndentPaymentService, IndentPaymentDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReptService, ReptServiceDB>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
