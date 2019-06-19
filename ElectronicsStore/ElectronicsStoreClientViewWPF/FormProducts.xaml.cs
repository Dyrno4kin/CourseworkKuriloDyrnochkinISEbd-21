using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormProducts.xaml
    /// </summary>
    public partial class FormProducts : Window
    {

        private readonly IProductService service;

        public FormProducts(IProductService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormProducts_Load(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ProductViewModel> list = service.GetList();
                if ((list != null) &&(list.Count > 0))
                {
                    dataGridViewProducts.ItemsSource = list;
                    dataGridViewProducts.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewProducts.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
