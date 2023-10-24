

using OneVar;
using ScottPlot.Drawing.Colormaps;
using ScottPlot;
using System.Xml.Linq;
using ScottPlot.MinMaxSearchStrategies;
using TwoVar;
using System.Security;
using System;

namespace StatsCalculator {
    public partial class Form1 : Form {
        private List<float> D;
        private List<float> D1;
        private List<float> D2;
        private List<int> R1;
        private List<float> R2;
        private OneVarDataSet data { get; set; }
        private TwoVarDataSet data2 { get; set; }
        private RandVar.RandVar random { get; set; }
        private LinReg linReg { get; set; }
        private Form2 form2;
        private Form3 form3;
        public Form1() {
            InitializeComponent();
            
            D = new();
            
            D1 = new();
            D2 = new();
            
            R1 = new();
            R2 = new();
        }

        private void CreateHistogram() {
            //Manually convert data.Data to a double array
            double[] doubles = new double[data.Data.Length];
            for (int i = 0; i < data.Data.Length; i++) 
                doubles[i] = data.Data[i];
            
            // create a histogram
            (double[] counts, double[] binEdges) = ScottPlot.Statistics.
                Common.Histogram(doubles, min: (doubles.Min()-1), max: (doubles.Max()+2), binSize: 1);
            double[] leftEdges = binEdges.Take(binEdges.Length - 1).ToArray();

            // display the histogram counts as a bar plot
            var bar = formsPlot2.Plot.AddBar(values: counts, positions: leftEdges);
            bar.BarWidth = 1;

            // customize the plot style
            formsPlot2.Plot.YAxis.Label("Count");
            formsPlot2.Plot.XAxis.Label("Number");
            formsPlot2.Plot.SetAxisLimits(yMin: 0);

            formsPlot2.Refresh();
        }

        private void CreateProbHistogram() {
            //Manually convert random.Data to double-type arrays
            int len = random.Set.Data.Length;
            double[] doubles1 = new double[len];
            double[] doubles2 = new double[len];
            for(int i = 0; i < len; i++) {
                doubles1[i] = random.Set.Data[i];
                doubles2[i] = random.Prob.Data[i];
            }

            //Create the histogram
            (double[] counts, double[] binEdges) = ScottPlot.Statistics.
                Common.Histogram(doubles1, min: (doubles1.Min() - 1), max: (doubles1.Max() + 2), binSize: 1);
            double[] leftEdges = binEdges.Take(binEdges.Length - 1).ToArray();

            //Display the histogram counts as a bar plot
            var bar = formsPlot3.Plot.AddBar(values: doubles2, positions: leftEdges);
            bar.BarWidth = 1;

            //Display Normal Approximation - Create function 


            //Customize plot style
            formsPlot3.Plot.YAxis.Label("Probability");
            formsPlot3.Plot.XAxis.Label("Number");
            formsPlot3.Plot.SetAxisLimits(yMin: 0);
        }

        private void CreateScatterPlot() {
            //Manually convert both datasets to double-type arrays
            int len = data2.XSet.Data.Length;
            double[] doubles1 = new double[len];
            double[] doubles2 = new double[len];
            for (int i = 0; i < len; i++) { 
                doubles1[i] = data2.XSet.Data[i];
                doubles2[i] = data2.YSet.Data[i];
            }
                

            //Create scatterplot
            formsPlot1.Plot.AddScatter(doubles1, doubles2, lineWidth: 0);
            formsPlot1.Plot.AddLine(linReg.B, linReg.A, (doubles1.Min() - 2, doubles1.Max() + 2), lineWidth: 2);

            //Customize plot style
            formsPlot1.Plot.XLabel("X Set");
            formsPlot1.Plot.YLabel("Y Set");

            formsPlot1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e) {
            /*try {
                OneVarDataSet data = new OneVarDataSet(getData());
                string Str = "";
                foreach (float i in data.Data) {
                    Str += i + " ";
                }

                label1.Text = Str;
            }
            catch (Exception ex) {
                label1.Text = "Invalid data set.";
                Console.WriteLine(ex.StackTrace);
            }*/
            if (D.Count > 0) {
                data = new OneVarDataSet(D.ToArray());
                string Str = "";
                foreach (float i in data.Data) {
                    Str += i + " ";
                }

                label1.Text = Str;
                label4.Text = "Mean: " + data.Mean;
                label5.Text = "Median: " + data.Median;
                label6.Text = "Standard Deviation: " + data.StDev;
                label7.Text = "n: " + data.Data.Length;
                CreateHistogram();
                button4.Enabled = true;
            }
            else {
                label1.Text = "Empty Set";
                label4.Text = "Mean: ";
                label5.Text = "Median: ";
                label6.Text = "Standard Deviation: ";
                label7.Text = "n: 0";
            }
        }

        /*private float[] getData() {
            string txt = richTextBox1.Text;
            List<float> data = new(); 
            while(FindFloatIndex(txt) != int.MaxValue) {
                data.Add(float.Parse(txt, System.Globalization.NumberStyles.AllowDecimalPoint));
                int ind = FindEndOfFloat(txt, FindFloatIndex(txt));
                txt = txt.Substring(ind);
                // 1. split off every float
                // 2. identify which strings have floats
                // 3. Parse each one

                // Better option: enter each data one by one
            }

            return data.ToArray();
        }

        private int FindEndOfFloat(string txt, int start) {
            //declare an index variable and set it to start
            int ind = start;
            //While next character is a digit or decimal point, add one to counter
            while (txt[ind] == '0' || txt[ind] == '1' || txt[ind] == '2' || txt[ind] == '3' || txt[ind] == '4' || txt[ind] == '5' 
                || txt[ind] == '6' || txt[ind] == '7' || txt[ind] == '8' || txt[ind] == '9' || txt[ind] == '.') {
                ind++;
                if (ind >= txt.Length) { break; }
            }
            //stop and return
            return ind;
        }

        private int FindFloatIndex(String str) {
            int[] indeces = new int[10];
            for (int i = 0; i < indeces.Length; i++) {
                indeces[i] = str.IndexOf(i + "");
                if (indeces[i] == -1) { indeces[i] = int.MaxValue; }
            }
            return indeces.Min();
        }*/

        private void Form1_Load(object sender, EventArgs e) {
            
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                string txt = textBox1.Text;
                D.Add(float.Parse(txt, System.Globalization.NumberStyles.Float));
                label3.Text = "Last Data Entry: " + txt;
            } 
            catch(Exception) {
                label3.Text = "Last Data Entry: Invalid";
            }
            textBox1.Text = "";
        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void button3_Click(object sender, EventArgs e) {
            D.Clear();

            label1.Text = "";
            label4.Text = "Mean: ";
            label5.Text = "Median: ";
            label6.Text = "Standard Deviation: ";
            label7.Text = "n: ";

            button4.Enabled = false;

            formsPlot2.Reset();
        }

        private void button4_Click(object sender, EventArgs e) {
            form2 = new Form2(data);
            form2.Show();
        }

        private void button5_Click(object sender, EventArgs e) {

        }


        private void label10_Click(object sender, EventArgs e) {

        }

        private void label18_Click(object sender, EventArgs e) {

        }

        private void button8_Click(object sender, EventArgs e) {
            try {
                string txt1 = textBox4.Text;
                D1.Add(float.Parse(txt1, System.Globalization.NumberStyles.Float));

                string txt2 = textBox5.Text;
                D2.Add(float.Parse(txt2, System.Globalization.NumberStyles.Float));

                label11.Text = "Last Data Entry: " + "(" + txt1 + "," + txt2 + ")";
            } 
            catch(Exception) {
                label11.Text = "Last Data Entry: Invalid";
            }
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button5_Click_1(object sender, EventArgs e) {
            try {
                string txt1 = textBox2.Text;
                int i = int.Parse(txt1, System.Globalization.NumberStyles.Integer);
                if (R1.Contains(i)) throw new Exception();
                R1.Add(i);

                string txt2 = textBox3.Text;
                float f = float.Parse(txt2, System.Globalization.NumberStyles.Float);
                if (!(0.0f < f && f < 1.0f)) throw new Exception();
                R2.Add(f);

                label9.Text = "Last Data Entry: " + "(" + txt1 + "," + txt2 + ")";
            }
            catch (Exception) {
                label9.Text = "Last Data Entry: Invalid";
            }
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button9_Click(object sender, EventArgs e) {
            if (D1.Count > 1 && D1.Count == D2.Count) {
                data2 = new TwoVarDataSet(D1.ToArray(), D2.ToArray());
                linReg = new LinReg(data2);
                string Str1 = "";
                string Str2 = "";
                foreach (float i in data2.XSet.Data) {
                    Str1 += i + " ";
                }
                foreach (float i in data2.YSet.Data) {
                    Str2 += i + " ";
                }

                label22.Text = "X: " + Str1;
                label12.Text = "n: " + data2.XSet.Data.Length;
                label13.Text = "Mean: " + data2.XSet.Mean;
                label14.Text = "Median: " + data2.XSet.Median;
                label15.Text = "Standard Deviation: " + data2.XSet.StDev;

                label23.Text = "Y: " + Str2;
                label16.Text = "n: " + data2.YSet.Data.Length;
                label17.Text = "Mean: " + data2.YSet.Mean;
                label18.Text = "Median: " + data2.YSet.Median;
                label19.Text = "Standard Deviation: " + data2.YSet.StDev;

                label28.Text = "Linear Regression: " + linReg.A + " + " + linReg.B + "x       r = " + linReg.R 
                    + "\t r² = " + (linReg.R * linReg.R);
                button11.Enabled = true;
                CreateScatterPlot();
            }
            else {
                label22.Text = "X: Empty Set";
                label12.Text = "n: 0";
                label13.Text = "Mean: ";
                label14.Text = "Median: ";
                label15.Text = "Standard Deviation: ";

                label23.Text = "Y: Empty Set";
                label16.Text = "n: 0";
                label17.Text = "Mean: ";
                label18.Text = "Median: ";
                label19.Text = "Standard Deviation: ";

                label28.Text = "Linear Regression: ";
                button11.Enabled = false;
            }
        }

        private void button10_Click(object sender, EventArgs e) {
            D1.Clear();
            D2.Clear();

            label22.Text = "X: ";
            label12.Text = "n: ";
            label13.Text = "Mean: ";
            label14.Text = "Median: ";
            label15.Text = "Standard Deviation: ";

            label23.Text = "Y: ";
            label16.Text = "n: ";
            label17.Text = "Mean: ";
            label18.Text = "Median: ";
            label19.Text = "Standard Deviation: ";

            label28.Text = "Linear Regression: ";
            button11.Enabled = false;

            //add reset of plot and disable any buttons you enable
            formsPlot1.Reset();
        }

        private void label28_Click(object sender, EventArgs e) {

        }

        private void button11_Click(object sender, EventArgs e) {
            form3 = new Form3(linReg);
            form3.Show();
        }

        private void label8_Click(object sender, EventArgs e) {

        }

        private void button6_Click(object sender, EventArgs e) {
            label29.Text = "";
            if((float)Math.Abs(R2.Sum() - 1) > 0.001) {
                label29.Text = "Your probabilities do not sum to 1, \n so your data has been automatically cleared.";

                R1.Clear();
                R2.Clear();

                label24.Text = "n: ";
                label25.Text = "Mean: ";
                label27.Text = "Standard Deviation: ";
                label26.Text = "Data: ";
                label30.Text = "Probabilities: ";
            }
            else if(R1.Count > 1 && R1.Count == R2.Count) {
                random = new RandVar.RandVar(R1.ToArray(), R2.ToArray());
                string Str1 = "";
                foreach(int i in random.Set.Data) {
                    Str1 += i + " ";
                }
                string Str2 = "";
                foreach(float i in random.Prob.Data) {
                    Str2 += i + " ";
                }

                label24.Text = "n: " + random.Set.Data.Length;
                label25.Text = "Mean: " + random.Mean;
                label27.Text = "Standard Deviation: " + random.StDev;
                label26.Text = "Data: " + Str1;
                label30.Text = "Probabilities: " + Str2;

                //the plot
            }
            else {
                label24.Text = "n: 0";
                label25.Text = "Mean: ";
                label27.Text = "Standard Deviation: ";
                label26.Text = "Data: Empty Set";
                label30.Text = "Probabilities: Empty Set";

                //add anything that becomes empty
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            R1.Clear();
            R2.Clear();

            label24.Text = "n: ";
            label25.Text = "Mean: ";
            label27.Text = "Standard Deviation: ";
            label26.Text = "Data: ";
            label30.Text = "Probabilities: ";

            label29.Text = "";

            // add resets of anything else added to the rand var panel

        }

        
    }
}