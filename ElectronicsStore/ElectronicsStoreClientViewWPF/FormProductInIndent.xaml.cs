using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    public partial class FormProductInIndent : Window
    {

        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        public int customerId { get; set; }

        private readonly IMainService service;
        private readonly IProductService serviceProd;
        private readonly ICustomerService serviceCustomer;

        private int? id;

        private List<IndentProductViewModel> indentproducts;

        public FormProductInIndent(IMainService service, IProductService serviceProd, ICustomerService serviceCustomer)
        {
            InitializeComponent();
            Loaded += FormProductInIndent_Load;
            this.service = service;
            this.serviceProd = serviceProd;
            this.serviceCustomer = serviceCustomer;
        }

        private void FormProductInIndent_Load(object sender, RoutedEventArgs e)
        {
           if (id.HasValue)
            {
                try
                {
                    IndentViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        indentproducts = view.IndentProducts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
           }
           else
             indentproducts = new List<IndentProductViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (indentproducts != null)
                {
                    dataGridViewProduct.ItemsSource = null;
                    dataGridViewProduct.ItemsSource = indentproducts;
                    dataGridViewProduct.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
