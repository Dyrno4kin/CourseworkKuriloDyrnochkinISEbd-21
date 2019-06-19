using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPaymentsIndent.xaml
    /// </summary>
    public partial class FormPaymentsIndent : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        public int customerId { get; set; }

        private readonly IMainService service;
        private readonly IProductService serviceProd;
        private readonly ICustomerService serviceCustomer;

        private int? id;

        private List<IndentPaymentViewModel> indentpayments;

        public FormPaymentsIndent(IMainService service, IProductService serviceProd, ICustomerService serviceCustomer)
        {
            InitializeComponent();
            Loaded += FormPaymentsIndent_Load;
            this.service = service;
            this.serviceProd = serviceProd;
            this.serviceCustomer = serviceCustomer;
        }

        private void FormPaymentsIndent_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    IndentViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        indentpayments = view.IndentPayments;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
               indentpayments = new List<IndentPaymentViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (indentpayments != null)
                {
                    dataGridViewPayments.ItemsSource = null;
                    dataGridViewPayments.ItemsSource = indentpayments;
                    dataGridViewPayments.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPayments.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewPayments.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
