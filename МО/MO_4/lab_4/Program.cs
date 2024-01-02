// x*x + x*y + y^2 - 3*x - 6*y
// Ответ: (0,3)
using System;
using System.Linq.Expressions;
using PolStrLib;


namespace Lab4
{
    class Program
    {
        // Нахождение градиента
        static double[] Gradient(double[] _x, string _polstExpr)
        {

            return new double[] { PolStr.EvalPolStr(_polstExpr, _x, 1, 1), PolStr.EvalPolStr(_polstExpr, _x, 1, 2) };
        }

        // Метод дихотомии
        public static double Dichotomy(double _a, double _b, double _ex, string _expr)
        {
            uint maxIterations = 10000;

            double e = 0.2 * _ex;

            uint k = 0;

            do
            {
                double y = (_a + _b - e) / 2;
                double z = (_a + _b + e) / 2;

                double fy = PolStr.EvalPolStr(_expr, y, 0);
                double fz = PolStr.EvalPolStr(_expr, z, 0);

                if (fy <= fz)
                {
                    _b = z;
                }
                else
                {
                    _a = y;
                }

                k++;
            } while ((Math.Abs(_b - _a) > _ex) && (k < maxIterations));

            return (_a + _b) / 2;
        }

        static void Main(string[] args)
        {
            string expression = "(x1 * x1) + (x1 * x2) + (x2 * x2) - (3 * x1) - (6 * x2)";

            double[] x0 = { 3, 2 };
            double ex = 0.0001;


            double[] x = new double[2];
            x0.CopyTo(x, 0);
            double[] gradX = new double[2];
            do
            {
                x.CopyTo(x0, 0);

                string polstrExpr = PolStr.CreatePolStr(expression, 2);

                double[] fGrad = Gradient(x0, polstrExpr);
                double[] d = { -fGrad[0], -fGrad[1] };

                string alphaExpr = expression.Replace("x1", $"(({x0[0]}) + x * ({d[0]}))")
                                              .Replace("x2", $"(({x0[1]}) + x * ({d[1]}))")
                                              .Replace(',', '.')
                                              .Replace("x", "x1");

                // Ищем значение альфа с помощью метода дихотомии (если не получается можно изменить границы)
                double alpha = Dichotomy(-2, 2, ex, PolStr.CreatePolStr(alphaExpr, 1));

                x[0] = x0[0] + alpha * d[0];
                x[1] = x0[1] + alpha * d[1];

                gradX = Gradient(x, polstrExpr);
            } while ((Math.Abs(x[0] - x0[0]) > ex) || (Math.Abs(x[1] - x0[1]) > ex) || (Math.Abs(gradX[0]) > ex) || (Math.Abs(gradX[1]) > ex));


            x[0] = Math.Round(x[0], 3);
            x[1] = Math.Round(x[1], 3);
            if(x[0]<0)
            {
                x[0] = 0;
            }
            if (x[1] < 0)
            {
                x[1] = 0;
            }
            Console.WriteLine("Ответ: ({0},{1})",x[0],x[1]);
            Console.ReadKey();
        }        
    }
}