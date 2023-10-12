//x^4-10x^3+36x^2+5x
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace MO_2
{
    class Program
    {
        static double F(double arg)
        {
            return (Math.Pow(arg, 4)) - (10 * Math.Pow(arg, 3)) + (36 * Math.Pow(arg, 2)) + 5 * arg;
        }
        static double F1(double arg)
        {
            return (4*Math.Pow(arg, 3)) - (30 * Math.Pow(arg, 2)) + (72 * arg) + 5;
        }
        static double F2(double arg)
        {
            return (12*Math.Pow(arg, 2)) - (60 * arg) + 72;
        }
        static void Main(string[] args)
        {
            double a=3,b=5, e = 0.0001;
            //Метод Ньтона
            double x0 = b,x1=x0;
            while (Math.Abs(F1(x0)) > e)
            {
                x1 = x0 - (F1(x0) / F2(x0));
                x0 = x1;
            }
            Console.Write("Метод Ньтона: ");
            Console.WriteLine("x= {0}",x0);

            //Метод средней точки
            double z = 0;
            while(Math.Abs(F(z))>e)
            {
                z = (a + b) / 2;
                Console.WriteLine("[a,b]= {0},{1}",a,b);
                Console.WriteLine("Z= {0}",z);
                Console.WriteLine(F(z));
                if (F(z) < 0)
                {
                    a = z;
                }
                else
                {
                    b = z;
                }
                x0 = z;
                //Console.ReadKey();
            }
            Console.Write("Метод средней точки: ");
            Console.WriteLine("x= {0}",x0);
            Console.ReadKey();
        }
    }
}
