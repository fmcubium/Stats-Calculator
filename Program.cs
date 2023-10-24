using OneVar;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using TwoVar;

namespace StatsCalculator {
    internal static class Program {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}


namespace OneVar {
    public class OneVarDataSet {
        public float[] Data { get; }
        public float Mean { get; }
        public float Median { get; }
        public float StDev { get; }
        public OneVarDataSet(params float[] data) {
            Data = data;
            Median = CalcMedian();
            Mean = CalcMean();
            StDev = CalcStDev();
        }

        private float CalcMean() {
            float sum = 0;
            foreach (float i in Data) {
                sum += i;
            }
            return sum / Data.Length;
        }

        private float CalcMedian() {
            float med = 0;
            float[] temp = (float[])Data.Clone();
            Array.Sort(temp);
            if (temp.Length % 2 == 0) {
                med = (temp[temp.Length / 2] + temp[temp.Length / 2 - 1]) / 2;
            }
            else {
                med = temp[temp.Length / 2];
            }
            return med;
        }

        private float CalcStDev() {
            float squareSums = 0;
            foreach (float i in Data) {
                squareSums += (float)Math.Pow(i - Mean, 2);
            }
            return (float)Math.Pow(squareSums/(Data.Length - 1), 0.5);
        }
    }

    public static class NormalDist {
       /* static Dictionary<float, float> TableA { get; set; }
        static bool TableInit { get; set; }*/
        

        //private static void InitTable() {
        //    if(!TableInit) {
        //        TableA = new Dictionary<float, float>();

        //        //TODO: for the z-scores have a while loop with a number counting up by 0.01 from -3.49 to 3.49, following table a 
        //        float[] temp = { 0.0002f, 0.0003f, 0.0003f, 0.0003f, 0.0003f, };
                
        //        TableInit = true;
        //    }
        //}

        public static float NormalCdf(float upBound, float lowBound, float mean, float stdev) {
            float n = 100000;
            float width = (upBound - lowBound) / n;
            float sum = 0;
            for(int i = 1; i <= n; i++) {
                sum += (float)(1 / (stdev * Math.Sqrt(2 * Math.PI)) *
                    Math.Pow(Math.E, -0.5 * Math.Pow(((width*i + lowBound) - mean) / stdev, 2))*width);
            }
            return -sum;
        }

        public static float InvNorm(float area, float mean, float stdev) {
            double a1 = -3.969683028665376e+01;
            double a2 = 2.209460984245205e+02;
            double a3 = -2.759285104469687e+02;
            double a4 = 1.383577518672690e+02;
            double a5 = -3.066479806614716e+01;
            double a6 = 2.506628277459239e+00;

            double b1 = -5.447609879822406e+01;
            double b2 = 1.615858368580409e+02;
            double b3 = -1.556989798598866e+02;
            double b4 = 6.680131188771972e+01;
            double b5 = -1.328068155288572e+01;

            double c1 = -7.784894002430293e-03;
            double c2 = -3.223964580411365e-01;
            double c3 = -2.400758277161838e+00;
            double c4 = -2.549732539343734e+00;
            double c5 = 4.374664141464968e+00;
            double c6 = 2.938163982698783e+00;

            double d1 = 7.784695709041462e-03;
            double d2 = 3.224671290700398e-01;
            double d3 = 2.445134137142996e+00;
            double d4 = 3.754408661907416e+00;

            double pLow = 0.02425;
            double pHigh = 1 - pLow;

            double p = (double)area;
            double q, r, x;
            if(0 < p && p < pLow) {
                q = Math.Sqrt(p - 2 * Math.Log(p));
                x = (((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                        ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
            }
            else if(pLow <= p && p <= pHigh) {
                q = p - 0.5;
                r = q * q;
                x = (((((a1 * r + a2) * r + a3) * r + a4) * r + a5) * r + a6) * q /
                        (((((b1 * r + b2) * r + b3) * r + b4) * r + b5) * r + 1);
            }
            else if(pHigh < p && p < 1) {
                q = Math.Sqrt(-2 * Math.Log(1 - p));
                x = -(((((c1 * q + c2) * q + c3) * q + c4) * q + c5) * q + c6) /
                        ((((d1 * q + d2) * q + d3) * q + d4) * q + 1);
            }
            else if(p == 0) {
                return float.NegativeInfinity;
            }
            else if(p == 1) {
                return float.PositiveInfinity;
            }
            else {
                return float.NaN;
            }
            return (float)x * stdev + mean;

        }
    }

    public static class TDist {

        public static float TCdf(float upBound, float lowBound, int df) {
            float n = 100000;
            float width = (upBound - lowBound) / n;
            float sum = 0;
            for(int i = 1; i <= n; i++) {
                sum += (float)Math.Pow(1 + (width*i + lowBound)*(width*i + lowBound)/df, -0.5 * (df + 1)); 
            }
            
            float tConst = 0;
            if (df % 2 == 0 && df > 1) tConst = TConstEven(df);
            else if (df % 2 == 1 && df > 1) tConst = TConstOdd(df);
            else return float.NaN;

            return tConst * sum;
        }

        private static float TConstEven(int df) {
            float val1 = 1 / (2 * (float)Math.Sqrt(df));
            
            float prod = 1;
            for (int i = 1; i <= df - 2; i++) prod *= (2*i + 1.0f)/(2*i);
            return val1 * prod;
        }
        private static float TConstOdd(int df) {
            float val1 = 1 / (float)(Math.PI * Math.Sqrt(df));

            float prod = 1;
            for (int i = 1; i <= df - 2; i++) prod *= (2 * i)/(2 * i + 1.0f);
            return val1 * prod;
        }

    }

    
}

namespace TwoVar {
    public class TwoVarDataSet {
        public OneVarDataSet XSet { get; set; }
        public OneVarDataSet YSet { get; set; }

        public TwoVarDataSet(OneVarDataSet x, OneVarDataSet y) {
            XSet = x;
            YSet = y;
        }

        public TwoVarDataSet(float[] x, float[] y) {
            XSet = new OneVarDataSet(x);
            YSet = new OneVarDataSet(y);
        }

        //public float LinReg(float x) {
        //    return A + B * x;
        //}

        //private float CalcR() {
        //    float SumProducts = 0;
        //    for (int i = 0; i < XSet.Data.Length; i++) {
        //        float x = (XSet.Data[i] - XSet.Mean) / XSet.StDev;
        //        float y = (YSet.Data[i] - YSet.Mean) / YSet.StDev;
        //        SumProducts += x * y;
        //    }
        //    return SumProducts/(XSet.Data.Length - 1);
        //}
        
    }

    public class LinReg {
        public TwoVarDataSet Set { get; set; }
        public float R { get; set; }
        public float A { get; set; }
        public float B { get; set; }

        public static float CalcResidStDev(TwoVarDataSet resid) {
            float residSums = 0;
            foreach(float i in resid.YSet.Data)
                residSums += i * i;
            return (float)Math.Pow(residSums / (resid.YSet.Data.Length - 2), 0.5);
        }

        public LinReg(TwoVarDataSet set) {
            Set = set;

            R = CalcR();
            B = R * (Set.YSet.StDev / Set.XSet.StDev);
            A = Set.YSet.Mean - B * Set.XSet.Mean;
        }
        public float YHat(float x) {
            return A + B * x;
        }

        public TwoVarDataSet ResidualPlot() {
            OneVarDataSet x = Set.XSet;
            float[] res = new float[x.Data.Length];
            for (int i = 0; i < res.Length; i++) {
                res[i] = Set.YSet.Data[i] - YHat(x.Data[i]);
            }

            TwoVarDataSet plot = new TwoVarDataSet(x, new OneVarDataSet(res));
            return plot;
        }

        private float CalcR() {
            float SumProducts = 0;
            for (int i = 0; i < Set.XSet.Data.Length; i++) {
                float x = (Set.XSet.Data[i] - Set.XSet.Mean) / Set.XSet.StDev;
                float y = (Set.YSet.Data[i] - Set.YSet.Mean) / Set.YSet.StDev;
                SumProducts += x * y;
            }
            return SumProducts / (Set.XSet.Data.Length - 1);
        }
    }

    
}

namespace RandVar {
    public class RandVar {
        public OneVarDataSet Set { get; set; }
        public OneVarDataSet Prob { get; set; }
        public float Mean { get; }
        public float StDev { get; }

        public RandVar(OneVarDataSet set, OneVarDataSet prob) {
            Set = set;
            Prob = prob;
            Mean = CalcMean();
            StDev = CalcStDev();
        }

        public RandVar(int[] set, float[] prob) {
            float[] temp = new float[set.Length];
            for (int i = 0; i < set.Length; i++) temp[i] = set[i];

            Set = new OneVarDataSet(temp);
            Prob = new OneVarDataSet(prob);
        }

        private float CalcMean() {
            float mean = 0;
            for (int i = 0; i < Set.Data.Length; i++) {
                mean += Set.Data[i] * Prob.Data[i];
            }
            return mean;
        }

        private float CalcStDev() {
            float sum = 0;
            for (int i = 0; i < Set.Data.Length; i++) {
                sum += Prob.Data[i] * ((Set.Data[i] - Mean) * (Set.Data[i] - Mean));
            }
            return (float)Math.Sqrt(sum);
        }
    }

    public static class BinomDist {

        private static int factorial(int n) {
            int prod = 1;
            for (int i = 1; i <= n; i++) {
                prod *= i;
            }
            return prod;
        }

        public static float BinomPdf(int n, float p, int k) {
            int coef = factorial(n) / (factorial(k) * factorial(n - k));
            return coef * (float)Math.Pow(p, k) * (float)Math.Pow(1 - p, n - k);
        }

        public static float BinomCdf(int n, float p, int kLow, int kHigh) {
            float prob = 0;
            for (int i = kLow; i <= kHigh; i++) {
                prob += BinomPdf(n, p, i);
            }
            return prob;
        }
    }

    public static class GeomDist {

        public static float GeomPdf(float p, int k) { return p * (float)Math.Pow(1 - p, k - 1); }
        public static float GeomCdf(float p, int kLow, int kHigh) {
            float prob = 0;
            for (int i = kLow; i <= kHigh; i++) {
                prob += GeomPdf(p, i);
            }
            return prob;
        }
    }
}