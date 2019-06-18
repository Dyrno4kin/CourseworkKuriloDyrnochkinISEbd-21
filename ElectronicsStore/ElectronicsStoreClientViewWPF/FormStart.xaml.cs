using System;
using System.Windows;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormStart.xaml
    /// </summary>
    public partial class FormStart : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public FormStart()
        {
            InitializeComponent();
        }
        private void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<FormRegistration>();
            form.ShowDialog();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<FormLogin>();
            this.Visibility = Visibility.Collapsed;
            form.ShowDialog();
            Close();
        }
    }
}
