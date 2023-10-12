//x^4-10x^3+36x^2+5x
//3,00004051727295
using System;

namespace MO_1._1
{
    class Program
    {
        static double F(double arg)
        {
            return (Math.Pow(arg, 4)) - (10 * Math.Pow(arg, 3)) + (36 * Math.Pow(arg, 2)) + 5 * arg;
        }
        static void Main(string[] args)
        {
            double a = 3, b = 5, e = 0.0001;
            //Метод Дихотомии
            double l= 0.2 * e, y,z;
            double x=0;
            for (int i = 0; Math.Abs(b-a)>e; i++)
            {
                y = (a + b - l) / 2;
                z = (a + b + l) / 2;
                if (F(y) <= F(z))
                {
                    b = z;
                }
                if(F(y) > F(z))
                {
                    a = y;
                }
                x = (a + b) / 2;
            }
            Console.WriteLine("Метод Дихотомии:");
            Console.WriteLine("x= {0}", x);
            Console.WriteLine("f(x)= {0}",F(x));
            //Метод Равномерного поиска
            double x1=0,n = 100,min = 300;
            for (int i = 1; i < n; i++)
            {
                x1 = a + i*( (b-a)/(n+1) );
                if (F(x1) < min)
                {
                    x = x1;
                }
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine("Метод Равномерного поиска:");
            Console.WriteLine("x= {0}", x);
            Console.WriteLine("f(x)= {0}", F(x));
            Console.ReadKey();
        }
    }
}
