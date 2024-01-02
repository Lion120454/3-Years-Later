//x^2 + xy + y^2 -3x -6y
//x=(0;3)   x0=(3;2)
using PolStrLib;
using System;
using System.Runtime;
using System.Linq;


namespace MO_3
{
    class Program
    {
        static void Main(string[] args)
        {
			string expr = "(x1*x1)+(x1*x2)+(x2*x2)-(3*x1)-(6*x2)";//Входное уравнение
			string pol = PolStr.CreatePolStr(expr, 2);//Преобразовываем уравнение для работы библ. PolStr
			double[] x0 = { 3, 2 };//Начальный вектор
			//double[] answ = null;
			double a=0.5;//Коэффициент сжатия
			double ex=0.0001;//Точность

			//Симплексный метод
			Console.Write("Симплексный метод: ");
			int n = 2; // Размерность
			double len = 1;

			// Находим p и g
			double p = len * (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2));
			double g = p - len * Math.Sqrt(2) / 2;

			// Построение симплекса
			double[,] xS = { { x0[0], x0[1] }, { x0[0] + p, x0[1] + g }, { x0[0] + g, x0[1] + p } };

			// Начальное приближение (геометрический центр симплекса)
			double[] x = new double[2];

			for (byte i = 0; i < 2; i++)
			{
				double sum = 0;
				for (byte j = 0; j < 3; j++)
					sum += xS[j, i];

				x[i] = 1 / (n + 1) * sum;
			}

			double[] lastX = { x[0], x[1] };
			double lastVal = PolStr.EvalPolStr(pol, lastX, 0, 0);

			do
			{
				// Сохраняем прошлое приближение
				lastX[0] = x[0];
				lastX[1] = x[1];
				lastVal = PolStr.EvalPolStr(pol, lastX, 0, 0);

				// Поиск значений ЦФ для вершин симплекса
				double[] fS = new double[3];
				for (byte i = 0; i < 3; i++)
                {
					fS[i] = PolStr.EvalPolStr(pol, new double[] { xS[i, 0], xS[i, 1] }, 0, 0);
                }

				// Ищем максимальное значение ЦФ среди вершин симплекса
				int indexMax = Array.IndexOf(fS, fS.Max());

				// Ищем отражение вершины с максимальным значением ЦФ
				double[] mirror = new double[2];

				for (byte i = 0; i < 2; i++)
				{
					mirror[i] = 0;
					for (byte j = 0; j < 3; j++)
						mirror[i] += xS[j, i];

					mirror[i] -= xS[indexMax, i];
					mirror[i] = mirror[i] * (2 / n) - xS[indexMax, i];
				}

				// Если значение ЦФ отражения меньше значения ЦФ максимальной вершины, то меняем максимальную на отражение
				if (PolStr.EvalPolStr(pol, new double[] { mirror[0], mirror[1] }, 0, 0) <= fS[indexMax])
				{
					xS[indexMax, 0] = mirror[0];
					xS[indexMax, 1] = mirror[1];
				}
				else
				{
					// Иначе производим сжатие
					len *= a;

					int indOfMin = Array.IndexOf(fS, fS.Min());

					for (byte i = 0; i < 2; i++)
                    {
						for (byte j = 0; j < 3; j++)
						{
							if (j == indOfMin)
                            {
								continue;
                            }
							xS[j, i] = a * xS[j, i] + (1 - a) * xS[indOfMin, i];
						}
                    }
				}

				// Новое приближение точки оптиума
				for (byte i = 0; i < 2; i++)
				{
					double sum = 0;
					for (byte j = 0; j < 3; j++)
                    {
						sum += xS[j, i];
                    }
					x[i] = 1.0 / (n + 1) * sum;
				}


			} while ((Math.Abs(x[0] - lastX[0]) > ex) || (Math.Abs(x[1] - lastX[1]) > ex) || (Math.Abs(PolStr.EvalPolStr(pol, x, 0, 0) - lastVal) > ex));

			x[0] = Math.Round(x[0]);
			x[1] = Math.Round(x[1]);
            if (x[0] <= 0)
            {
				x[0] = 0;
            }
			if (x[1] <= 0)
			{
				x[1] = 0;
			}
			Console.WriteLine("({0} , {1})\n", x[0],x[1]);

			


			// Метод Хука-Дживса
			Console.Write("Метод Хука-Дживса: ");
			a=2;
			double step = 1;
			double check = PolStr.EvalPolStr(pol, x0, 0, 0) + (ex * 2);

			while ((step > ex) || (Math.Abs(PolStr.EvalPolStr(pol, x0, 0, 0) - check) > ex))
			{
				check = PolStr.EvalPolStr(pol, x0, 0, 0);

				double[] xBuf = { x0[0], x0[1] };

				double xValue = PolStr.EvalPolStr(pol, xBuf, 0, 0);

				bool notFound = true;

				// Исследующий поиск
				for (double i = (xBuf[0] - step); i <= (xBuf[0] + step); i += step)
                {
					for (double j = xBuf[1] - step; j <= (xBuf[1] + step); j += step)
					{
						double localSolution = PolStr.EvalPolStr(pol, new double[] { i, j }, 0, 0);
						if (!(xValue > localSolution))
							continue;

						xBuf[0] = i;
						xBuf[1] = j;
						xValue = localSolution;
						notFound = false;
					}
                }

				if (notFound)
				{
					step /= a;
					continue;
				}

				// Поиск по образцу
				double[] xSample = new double[2];	

				for (byte i = 0; i < 2; i++)
                {
					xSample[i] = xBuf[i] + (xBuf[i] - x0[i]);
                }
				if (PolStr.EvalPolStr(pol, xSample, 0, 0) < xValue)
				{ 
					for (byte i = 0; i < 2; i++)
                    {
						x0[i] = xSample[i];
                    }
				}
                else
                {
					for (byte i = 0; i < 2; i++)
                    {
						x0[i] = xBuf[i];
                    }
                }
			}


			x0[0] = Math.Round(x0[0]);
			x0[1] = Math.Round(x0[1]);
			if (x0[0] <= 0)
			{
				x0[0] = 0;
			}
			if (x0[1] <= 0)
			{
				x0[1] = 0;
			}
			Console.WriteLine("({0} , {1})", x0[0], x0[1]);
			Console.ReadKey();
		}
    }
}
