using ElectronicsStoreModel;
using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    public partial class FormMain : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public string login { get; set; }

        private readonly IMainService service;
        private readonly IReptService reportService;
        private readonly ICustomerService customerService;
        private readonly IIndentPaymentService paymentService;
        private readonly IBackupService backupService;
        
        private CustomerViewModel customer;

        public FormMain(IMainService service, IReptService reportService, ICustomerService customerService, 
            IIndentPaymentService paymentService, IBackupService backupService)
        {
            InitializeComponent();
            this.service = service;
            this.reportService = reportService;
            this.customerService = customerService;
            this.paymentService = paymentService;
            this.backupService = backupService;
        }

        private void LoadData()
        {
            try
            {
                customer = customerService.GetElement(login);
                customerService.setBonus(customer.Id);
                if (customer.CustomerStatus == true)
                {
                    buttonCreateOrder.Visibility = Visibility.Collapsed;
                    System.Windows.MessageBox.Show("Пользователь заблокирован", "Пользователь заблокирован", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    buttonCreateOrder.Visibility = Visibility.Visible;
                }
                if (customer != null)
                {
                    var indents = service.GetListCustomer(customer.Id);
                    dataGridViewMain.ItemsSource = indents;
                    if (dataGridViewMain.Columns.Count > 0)
                    {
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[6].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[7].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
                        }

                    foreach (var indent in indents)
                    {
                        if (service.GetBalance(indent.Id) == 0)
                            service.SetStatus(indent);
                    }

                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void товарыToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<FormProducts>();
            form.ShowDialog();
        }

        private void buttonCreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var form = Container.Resolve<FormCreateIndent>();
                form.customerId = customer.Id;
                form.ShowDialog();
                LoadData();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void buttonPayPolnost_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                if (((IndentViewModel)dataGridViewMain.SelectedItem).Status == IndentStatus.Оплачен)
                {
                    System.Windows.MessageBox.Show("Заказ уже полностью оплачен", "Заказ", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                int id = ((IndentViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    var form = Container.Resolve<FormPayIndent>();
                    form.Id = id;
                    form.full = true;
                    if (form.ShowDialog() == true)
                    {
                        if (form.Model != null)
                        {
                            paymentService.AddElement(form.Model);
                        }
                        LoadData();
                    }
                    
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayChastich_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                if (((IndentViewModel)dataGridViewMain.SelectedItem).Status == IndentStatus.Оплачен)
                {
                    System.Windows.MessageBox.Show("Заказ уже полностью оплачен", "Заказ", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                int id = ((IndentViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    var form = Container.Resolve<FormPayIndent>();
                    form.Id = id;
                    form.full = false;
                    if (form.ShowDialog() == true)
                    {
                        if (form.Model != null)
                        {
                            paymentService.AddElement(form.Model);
                        }
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private void списокТоваровWToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    reportService.SaveProductPriceW(new ReptBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void списокТоваровEToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    reportService.SaveProductPriceXls(new ReptBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void отчетToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
          var form = Container.Resolve<FormCustomerIndents>();
          form.customerId = customer.Id;
          form.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void buttonBonus_Click(object sender, RoutedEventArgs e)
        {
            customerService.setBonus(customer.Id);
            LoadData();
        }

        private void бэкапJsonToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "json|*.json"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    backupService.BackupToJson(new BackupBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void бэкапXMLToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                backupService.BackupToXML();
                MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonListProductInIndent_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = Container.Resolve<FormProductInIndent>();
                form.customerId = customer.Id;
                form.Id = ((IndentViewModel)dataGridViewMain.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonPaymentsIndent_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = Container.Resolve<FormPaymentsIndent>();
                form.customerId = customer.Id;
                form.Id = ((IndentViewModel)dataGridViewMain.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }
    }
}
