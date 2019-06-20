using ElectronicsStoreServiceDAL.Interfaces;
using ElectronicsStoreServiceImplementDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Unity;

namespace ElectronicsStoreAdminView
{
    public partial class FormSchedule : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IDataServiceDB service;
        private readonly IProductService prodService;

        public FormSchedule(IDataServiceDB service, IProductService prodService)
        {
            InitializeComponent();
            this.service = service;
            this.prodService = prodService;
        }

        private void FormSchedule_Load(object sender, EventArgs e)
        {
            var products = prodService.GetList();
            var indents = service.DataIndent();

            chartProduct.ChartAreas[0].AxisX.MajorGrid.Interval = 1;

            foreach (var product in products)
            {
                chartProduct.Series.Add(product.ProductName);
                for (int day=1; day<= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); day++)
                {
                    chartProduct.Series[product.ProductName].Points.AddXY(day, 0);

                    var prodGroups = indents.Where(r => DateTime.ParseExact(r.DateCreate.Replace(" ", ""), "ddMMMMyyyy", null).Day == day)
                        .Select(r => r.IndentProducts.Where(rec => rec.ProductId == product.Id));
                    if (prodGroups.Count() != 0)
                    {
                        foreach (var prodGroup in prodGroups)
                        {
                            var prod = prodGroup.GroupBy(rec => rec.ProductId)
                                .Select(rec => new { Id = rec.Key, Total = rec.Sum(x => x.Count) });
                            if (prod.Count() != 0)
                            {
                                   chartProduct.Series[product.ProductName].Points.Last().SetValueY(
                                       chartProduct.Series[product.ProductName].Points.Last().YValues.Last()+prod.First().Total);
                                continue;
                            }
                        }
                    }
                }
            }
        }
    }
}
