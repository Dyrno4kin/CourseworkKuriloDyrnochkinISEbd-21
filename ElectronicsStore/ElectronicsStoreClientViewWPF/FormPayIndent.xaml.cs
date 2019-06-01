using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
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
using System.Windows.Shapes;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormPayIndent.xaml
    /// </summary>
    public partial class FormPayIndent : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly IMainService service;

        public int Id { set { id = value; } }
        public bool full { get; set; }
        private int? id;
        IndentViewModel indent;

        public IndentPaymentViewModel Model { set { model = value; } get { return model; } }
        private IndentPaymentViewModel model;

        public FormPayIndent(IMainService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormIndentProduct_Load(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    indent = service.GetElement(id.Value);
                    if ((indent != null)&&(full==true))
                    {
                        textBoxPrice.Text = service.GetBalance(id.Value).ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                   MessageBoxImage.Error);
                }
            }
        }
        

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (service.GetBalance(id.Value) < Convert.ToInt32(textBoxPrice.Text))
            {
                MessageBox.Show("Сумма превышает необходимую", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if(model == null)
                {
                    model = new IndentPaymentViewModel
                    {
                        Id = id.Value,
                        SumPayment = Convert.ToInt32(textBoxPrice.Text),
                        DatePayment = DateTime.Now
                    };
                }
                else
                {
                    model.SumPayment = Convert.ToInt32(textBoxPrice.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
