namespace ElectronicsStoreAdminView
{
    partial class FormSchedule
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chartProduct = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // chartProduct
            // 
            chartArea1.Name = "ChartArea1";
            this.chartProduct.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartProduct.Legends.Add(legend1);
            this.chartProduct.Location = new System.Drawing.Point(25, 18);
            this.chartProduct.Name = "chartProduct";
            this.chartProduct.Size = new System.Drawing.Size(1151, 503);
            this.chartProduct.TabIndex = 0;
            // 
            // FormSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 533);
            this.Controls.Add(this.chartProduct);
            this.Name = "FormSchedule";
            this.Text = "FormSchedule";
            this.Load += new System.EventHandler(this.FormSchedule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartProduct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartProduct;
    }
}