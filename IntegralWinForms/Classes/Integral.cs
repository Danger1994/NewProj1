using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntegralWinForms.Classes
{
    public class Integral
    {
        private Func<double, double> _F;
        private double a;
        private double b;
        private long n;

        public double A
        {
            get
            {
                return a;
            }
        }

        public double B
        {
            get
            {
                return b;
            }
        }

        public long N
        {
            get
            {
                return n;
            }
        }

        public void SetData(double a, double b, long N, Func<double, double> f)
        {
            this.a = a;
            this.b = b;
            this.n = N;
            _F = f;

        }
        public Integral(double a, double b, long N, Func<double, double> _F)
        {
            SetData(a, b, N, _F);
        }

        public double Rectangle(out double time)
        {
            Stopwatch sw1;
            sw1 = new Stopwatch();
            sw1.Start();
            time = 0;
            double res = CalcRectangle(a, b, n, _F);
            sw1.Stop();
            time = sw1.ElapsedMilliseconds;
            return res;
        }

        public double RectangleParallel(out double time)
        {
            Stopwatch sw2;
            sw2 = new Stopwatch();
            sw2.Start();

            time = 0;
            double res = ParallelCalcRectangle(a, b, n, _F);
            sw2.Stop();
            time = sw2.ElapsedMilliseconds;
            return res;
            
        }

        private double CalcRectangle(double a, double b, long N, Func<double, double> _F)
        {
            double h = (b - a) / n;
            double S = 0;
            for (int i = 0; i < n; i++)
            {
                S += _F(a + i * h);
            }
            S *= h;
            return S;
        }


        private double ParallelCalcRectangle(double a, double b, long N, Func<double, double> _F)
        {
            double h = (b - a) / n;
            double S = 0;
            object mon = new object();
            Parallel.For(0, N, () => 0.0, (i, state, local) =>
            {

                local += _F(a + i * h);
                return local;
            }, local => { lock (mon) S += local; });


            S *= h;
            return S;
        }


        private double Calculate(double a, double b, long n)
        {
            long i;
            double result = 0;
            double h = (b - a) / n;

            for (i = 0; i < n; i++)
            {
                result += _F(a + h * i);
            }

            result *= h;

            return result;
        }

        public double CalculateParallel(int numThreads)
        {
            double h = (b - a) / n;
            double rez = 0.0;

            int ost = Convert.ToInt32(n) % numThreads;
            int[] numOfIterationsPerThread = new int[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                numOfIterationsPerThread[i] = Convert.ToInt32(N / numThreads);
                if (i < ost)
                    numOfIterationsPerThread[i]++;
            }



            Task<double>[] tasks = new Task<double>[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                double start = this.a + rez;
                double end = start + numOfIterationsPerThread[i] * h;
                tasks[i] = Task.Factory.StartNew<double>(() => Calculate(a, b, n));
                rez += numOfIterationsPerThread[i] * h;
            }
            Task.WaitAll(tasks);
            rez = 0.0;
            for (int i = 0; i < numThreads; i++)
            {
                rez += tasks[i].Result;
            }

            return rez;

        }


        public double RectangleParallel2(out double time)
        {
            Stopwatch sw2;
            sw2 = new Stopwatch();
            sw2.Start();

            time = 0;
            double res = CalculateParallel(8);
            sw2.Stop();
            time = sw2.ElapsedMilliseconds;
            return res;

        }

     }
    }

