using IntegralWinForms.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegralWinForms
{
    public partial class Form1 : Form
    {

        private double SumIntegral(double x)
        {
            return 7 * x - Math.Log(7 * x) + 8;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rtbLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void btCalc_Click(object sender, EventArgs e)
        {
            double time;
            double time2;
            double time3;
            double a=Convert.ToDouble(tbA.Text);
            double b = Convert.ToDouble(tbB.Text);
            long N = Convert.ToInt64(nudN.Text);

            Integral int1 = new Integral(a, b, N, SumIntegral);
            double result=int1.Rectangle(out time);
            double result2 = int1.RectangleParallel(out time2);
            double result3 = int1.RectangleParallel2(out time3);

            rtbLog.AppendText("Otvet_posled:" + result.ToString()+Environment.NewLine);
            rtbLog.AppendText("Time_posled:" + time.ToString() + Environment.NewLine);
            rtbLog.AppendText("Otvet_parallel:" + result2.ToString() + Environment.NewLine);
            rtbLog.AppendText("Time_parallel:" + time2.ToString() + Environment.NewLine);

            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();



            chart2.Series[0].Points.AddY(time);
            chart2.Series[1].Points.AddY(time2);
            chart2.Series[2].Points.AddY(time3);
            double[] res = new double [10000];


            chart3.Series[0].Points.Clear();
            for (int i=1; i<=10; i++)
            {
                res[i]=int1.RectangleParallel2(out time);
                chart3.Series[0].Points.AddXY(i, time);
            }

            DrawGraphs();

        }

        private void btDraw_Click(object sender, EventArgs e)
        {
            DrawGraphs();
        }


        public void DrawGraphs()
        {
         int startN = 1000000;
         int maxN = 10000000;
         double a = 1, b = 10000;
         Stopwatch sw = new Stopwatch();
         Stopwatch sw2 = new Stopwatch();
         for (int i = startN; i <= maxN; i+= 1000000)
            {
                sw.Reset();
                sw.Start();

                double time;
                Integral int2 = new Integral(a, b, i, SumIntegral);
                double result = int2.Rectangle(out time);
               
                sw.Stop();


                double t = sw.ElapsedMilliseconds;
                
                chart1.Series[0].Points.AddXY(i, t);
                
            }


            for (int i = startN; i <= maxN; i += 1000000)
            {
                sw2.Reset();
                sw2.Start();

                double time2;
                Integral int2 = new Integral(a, b, i, SumIntegral);
                double result2 = int2.RectangleParallel(out time2);
      
                sw2.Stop();


                double t = sw2.ElapsedMilliseconds;

                
                chart1.Series[1].Points.AddXY(i, t);
            }




        }

        private void button1_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
