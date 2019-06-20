using System.Data.SqlClient;
using System.Windows;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormLogin.xaml
    /// </summary>
    public partial class FormLogin : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public FormLogin()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLogin.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPass.Text))
            {
                MessageBox.Show("Заполните пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ElectronicsStoreDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from Customers where Login = '" + textBoxLogin.Text + "' and Password = '" + textBoxPass.Text + "'", conn);
            SqlDataReader dt;
            dt = cmd.ExecuteReader();
            int count = 0;

            while (dt.Read())
            {
                count += 1;
            }
            if (count == 1)
            {
                Close();
                var form = Container.Resolve<FormMain>();
                form.login = textBoxLogin.Text;
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        
    }
}
