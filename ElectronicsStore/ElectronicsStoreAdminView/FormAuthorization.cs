using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ElectronicsStoreAdminView
{
    public partial class FormAuthorization : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public FormAuthorization()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxLogin.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPassword.Text))
            {
                MessageBox.Show("Заполните пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if ((textBoxPassword.Text == ConfigurationManager.AppSettings["AdminPassword"])
                &&(textBoxLogin.Text == ConfigurationManager.AppSettings["AdminLogin"]))
            {

                this.Visible = false;
                var form2 = Container.Resolve<FormMain>();
                form2.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
