using ElectronicsStoreServiceDAL.BindingModel;
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
    /// Логика взаимодействия для FormCreateOrder.xaml
    /// </summary>
    public partial class FormCreateIndent : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IMainService service;

        private int? id;

        private List<IndentProductViewModel> indentproducts;

        public FormCreateIndent(IMainService service)
        {
            InitializeComponent();
            Loaded += FormCreateIndent_Load;
            this.service = service;
        }

        private void FormCreateIndent_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    IndentViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxPrice.Text = view.Sum.ToString();
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormIndentProduct>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.IndentId = id.Value;
                    indentproducts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                var form = Container.Resolve<FormIndentProduct>();
                form.Model = indentproducts[dataGridViewProduct.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    indentproducts[dataGridViewProduct.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        indentproducts.RemoveAt(dataGridViewProduct.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (indentproducts == null || indentproducts.Count == 0)
            {
                MessageBox.Show("Выберите товары", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<IndentProductBindingModel> indentproductBM = new List<IndentProductBindingModel>();
                for (int i = 0; i < indentproducts.Count; ++i)
                {
                    indentproductBM.Add(new IndentProductBindingModel
                    {
                        Id = indentproducts[i].Id,
                        IndentId = indentproducts[i].IndentId,
                        ProductId = indentproducts[i].ProductId,
                        Count = indentproducts[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new IndentBindingModel
                    {
                        Id = id.Value,
                        Sum = Convert.ToInt32(textBoxPrice.Text),
                        IndentProducts = indentproductBM
                    });
                }
                else
                {
                    service.AddElement(new IndentBindingModel
                    {
                        Sum = Convert.ToInt32(textBoxPrice.Text),
                        IndentProducts = indentproductBM
                    });
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
