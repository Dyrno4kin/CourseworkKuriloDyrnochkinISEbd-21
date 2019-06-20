using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Windows;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormCustomerIndents.xaml
    /// </summary>
    public partial class FormCustomerIndents : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int customerId { get; set; }
        public String customerEmail { get; set; }

        private readonly IReptService service;

        public FormCustomerIndents(IReptService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "ElectronicsStoreClientViewWPF.ReportIndent.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePickerFrom.SelectedDate.ToString() +
                                            " по " + dateTimePickerTo.SelectedDate.ToString());
                reportViewer.LocalReport.SetParameters(parameter);


                var rept = new ReptBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                };
                var dataSource = service.GetCustomerIndentsForClients(rept, customerId);
                reportViewer.LocalReport.DataSources.Clear();
                ReportDataSource source = new ReportDataSource("DataSet", dataSource);
                reportViewer.LocalReport.DataSources.Add(source);
                var dataSourcePayment = service.GetIndentInfoForClients(rept, customerId);
                source = new ReportDataSource("DataSetPayment", dataSourcePayment);
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var model = (new ReptBindingModel
                    {
                        Email = customerEmail,
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate,
                        DateTo = dateTimePickerTo.SelectedDate
                    });
                    service.SaveCustomerIndentsForClients(model, customerId);
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSendClick(object sender, RoutedEventArgs e)
        {
            var model = (new ReptBindingModel
            {
                Email = customerEmail,
                FileName = @"D:\test.pdf",
                DateFrom = dateTimePickerFrom.SelectedDate,
                DateTo = dateTimePickerTo.SelectedDate
            });
            service.SaveCustomerIndentsForClients(model, customerId);
        }
    }
}
