using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private CustomerViewModel customer;

        public FormMain(IMainService service, IReptService reportService, ICustomerService customerService)
        {
            InitializeComponent();
            this.service = service;
            this.reportService = reportService;
            this.customerService = customerService;
        }

        private void LoadData()
        {
            try
            {
                customer = customerService.GetElement(login);
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
                    dataGridViewMain.ItemsSource = service.GetListCustomer(customer.Id);
                    if (dataGridViewMain.Columns.Count > 0)
                    {
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
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
                            //IndentViewModel view = service.GetElement(id);
                            //List<IndentPaymentViewModel> indentpayments = view.IndentPayments;
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
                int id = ((IndentViewModel)dataGridViewMain.SelectedItem).Id;
                try
                {
                    var form = Container.Resolve<FormPayIndent>();
                            form.Model.IndentId = id;
                            form.full = false;
                    if (form.ShowDialog() == true)
                    {
                        if (form.Model != null)
                        {
                            IndentViewModel view = service.GetElement(id);
                            List<IndentPaymentViewModel> indentpayments = view.IndentPayments;
                            indentpayments.Add(form.Model);
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
            //var form = Container.Resolve<FormCustomerIndents>();
           // form.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
