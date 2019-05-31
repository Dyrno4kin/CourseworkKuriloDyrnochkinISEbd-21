using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ElectronicsStoreAdminView
{
    public partial class FormIndentProduct : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly IMainService service;

        private List<IndentProductViewModel> indentProduct;
        public int Id { set { id = value; } }
        private int? id;

        public FormIndentProduct(IMainService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormIndentProducts_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    IndentViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        indentProduct = view.IndentProducts;
                        if (indentProduct != null)
                        {
                            dataGridView.DataSource = indentProduct;
                            dataGridView.Columns[0].Visible = false;
                            dataGridView.Columns[1].AutoSizeMode =
                            DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
    }
}
