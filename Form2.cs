using OneVar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatsCalculator {
    public partial class Form2 : Form {
        private OneVarDataSet data;
        public Form2(OneVarDataSet d) {
            data = d;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label8_Click(object sender, EventArgs e) {

        }

        private void label1_Click_1(object sender, EventArgs e) {

        }

        private void tabPage2_Click(object sender, EventArgs e) {

        }

        private void button4_Click(object sender, EventArgs e) {
            try {
                float lower = float.Parse(textBox5.Text);
                float upper = float.Parse(textBox6.Text);
                float mean = float.Parse(textBox7.Text);
                float stdev = float.Parse(textBox8.Text);

                float n = NormalDist.NormalCdf(lower, upper, mean, stdev);
                label2.Text = "Area calculated: " + n; 
            } catch(Exception) {
                label2.Text = "Invalid information entered. (Don't enter spaces)";
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                float lower = float.Parse(textBox5.Text);
                float upper = float.Parse(textBox6.Text);

                float n = NormalDist.NormalCdf(lower, upper, data.Mean, data.StDev);
                label2.Text = "Area calculated: " + n;
            }
            catch (Exception) {
                label2.Text = "Invalid information entered. (Don't enter spaces)";
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                float area = float.Parse(textBox1.Text);
                float mean = float.Parse(textBox9.Text);
                float stdev = float.Parse(textBox10.Text);

                float z = NormalDist.InvNorm(area, mean, stdev);
                label13.Text = "Value calculated: " + z;
            }
            catch (Exception) {
                label13.Text = "Invalid information entered. (Don't enter spaces)";
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                float area = float.Parse(textBox1.Text);

                float z = NormalDist.InvNorm(area, data.Mean, data.StDev);
                label13.Text = "Value calculated: " + z;
            }
            catch (Exception) {
                label13.Text = "Invalid information entered. (Don't enter spaces)";
            }
        }
    }
}
