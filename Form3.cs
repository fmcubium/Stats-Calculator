using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwoVar;

namespace StatsCalculator {
    public partial class Form3 : Form {
        private LinReg line;
        public Form3(LinReg l) {
            line = l;
            InitializeComponent();
            GeneratePlot();
        }

        private void GeneratePlot() {
            TwoVarDataSet plot = line.ResidualPlot();

            int len = plot.XSet.Data.Length;
            double[] doubles1 = new double[len];
            double[] doubles2 = new double[len];
            for (int i = 0; i < len; i++)
                doubles1[i] = plot.XSet.Data[i];
            for (int i = 0; i < len; i++)
                doubles2[i] = plot.YSet.Data[i];

            //Create scatterplot
            formsPlot1.Plot.AddScatter(doubles1, doubles2, lineWidth: 0);
            formsPlot1.Plot.AddLine(0, 0, (doubles1.Min() - 2, doubles1.Max() + 2), lineWidth: 2);

            //Customize plot style
            formsPlot1.Plot.XLabel("X Set");
            formsPlot1.Plot.YLabel("Residuals");

            formsPlot1.Refresh();

            label1.Text = "Standard Deviation of Residuals: " + LinReg.CalcResidStDev(plot);
        }

        private void label1_Click(object sender, EventArgs e) {

        }
    }
}
