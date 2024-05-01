namespace Optimization.RGR;

public class RGR
{
    public const int Accuracy = 3;
    public const double E = 0.01;

    public static readonly double[,] H = { { 6, -1 }, { -1, 8 } };
    
    private static double F(double x1, double x2)
    {
        return Math.Round(3 * Math.Pow(x1 - 1, 2) - x1 * x2 + 4 * Math.Pow(x2, 2), Accuracy);
    }
    
    private static double[] Gradient(double x1, double x2)
    {
        return [Math.Round(6 * x1 - x2 - 6 , Accuracy), Math.Round(- x1 + 8 * x2, Accuracy)];
    }
    
    public static void Calculate()
    {
        Console.WriteLine("Розрахункова робота");
        Console.WriteLine("Мiнiмiзувати функцiю f(x) = 3(x1-1)^2 - x1x2 + 4x2^2 ");
        Console.WriteLine("Початкова точка: x0 = (-5.2, -5.2)^T");
        Console.WriteLine();
        
        //double[] x0 = [-14.8, -14.8];
        double[] x0 = [-5.2, -5.2];
        double[] s1 = [1, 0];
        double[] s2 = [0, 1];
        
        var deltaL = FindDeltaL(x0, s2);
        var interval = Sven(0, deltaL, x0, s2, out var lt);

        GoldCut(interval, E, x0, s2);

        var l1 = FindL(x0, s2);
        double[] x1 = [x0[0] + l1 * s2[0], x0[1] + l1 * s2[1]];
        x1 = [Math.Round(x1[0], Accuracy), Math.Round(x1[1], Accuracy)];
        Console.WriteLine($"x1 = ({Print(x0)})^T + {l1} * ({Print(s2)})^T = ({Print(x1)})^T");

        deltaL = FindDeltaL(x1, s1);
        interval = Sven(0, deltaL, x1, s1, out lt);

        GoldCut(interval, E, x1, s1);

        var l2 = FindL(x1, s1);
        double[] x2 = [x1[0] + l2 * s1[0], x1[1] + l2 * s1[1]];
        x2 = [Math.Round(x2[0], Accuracy), Math.Round(x2[1], Accuracy)];
        Console.WriteLine($"x2 = ({Print(x1)})^T + {l2} * ({Print(s1)})^T = ({Print(x2)})^T");
        Console.WriteLine($"f(x2) - f(x1) = {F(x2)} - {F(x1)} = {F(x2) - F(x1)} <= {E}   {Math.Round(Math.Abs(F(x2) - F(x1)),Accuracy) <= E}");

        deltaL = FindDeltaL(x2, s2);
        Sven(0, deltaL, x2, s2, out var l3, true);
        double[] x3 = [x2[0] + l3 * s2[0], x2[1] + l3 * s2[1]];
        x3 = [Math.Round(x3[0], Accuracy), Math.Round(x3[1], Accuracy)];
        Console.WriteLine($"\nx3 = ({Print(x2)})^T + {l3} * ({Print(s2)})^T = ({Print(x3)})^T");
        Console.WriteLine($"f(x3) - f(x2) = {F(x3)} - {F(x2)} = {Math.Round(F(x3) - F(x2),Accuracy)} <= {E}   {Math.Round(Math.Abs(F(x3) - F(x2)),Accuracy) <= E}");

        double[] direction = [Math.Round(x3[0] - x1[0], Accuracy), Math.Round(x3[1] - x1[1], Accuracy)];
        deltaL = FindDeltaL(x3, direction);
        ShortSven(0, deltaL, x3, s2, out var l4, true);
        double[] x4 = [x3[0] + l4 * direction[0], x3[1] + l4 * direction[1]];
        x4 = [Math.Round(x4[0], Accuracy), Math.Round(x4[1], Accuracy)];
        Console.WriteLine($"x4 = ({Print(x3)})^T + {l4} * ({Print(direction)})^T = ({Print(x4)})^T");
        Console.WriteLine($"f(x4) - f(x3) = {F(x4)} - {F(x3)} = {F(x4) - F(x3)} <= {E}   {F(x4) - F(x3) <= E}");
    }

    private static string Print(double[] x)
    {
        return $"{x[0]}, {x[1]}";
    }

    private static double FindL(double[] x0, double[] s2) 
    {
        var gradient = Gradient(x0[0], x0[1]);
        var top = gradient[0] * s2[0] + gradient[1] * s2[1];
        double[] bot = [s2[0] * H[0, 0] + s2[1] * H[1, 0], s2[0] * H[1, 0] + s2[1] * H[1, 1]];
        var bottom = bot[0] * s2[0] + bot[1] * s2[1];
        var result = Math.Round(-top/bottom, Accuracy);
        Console.WriteLine($"l = -{top}/{bottom} = {result}");

        return result;
    }

    private static double FindDeltaL(double[] x0, double[] s)
    {
        var top = Math.Sqrt(Math.Pow(x0[0], 2) + Math.Pow(x0[1], 2));
        var bot = Math.Sqrt(Math.Pow(s[0], 2) + Math.Pow(s[1], 2));
        var res = 0.1 * (top/bot);
        res = Math.Round(res, Accuracy);

        Console.WriteLine($"dl = 0.1 * (\u221a{x0[0]}^2 + {x0[1]}^2 / \u221a{s[0]}^2 + {s[1]}^2) = {res}\n");
        
        return res;
    }

    private static double[] Sven(double l0, double step, double[] xk, double[] s, out double ls, bool isLast = false)
    {
        Console.WriteLine("Алгоритм Свена");
        var fx0 = F(xk, s, l0);
        Console.WriteLine($"l0 = {l0}  f(l0) = {fx0}");
        
        var multiplayer = 2;
        var tempX1Plus = F(xk, s, l0 + step);
        Console.WriteLine($"l0 + d = {l0 + step}  f(l0 + d) = {tempX1Plus}");
        var tempX1Minus =  F(xk, s, l0 - step);
        Console.WriteLine($"l0 - d = {l0 - step}  f(l0 - d) = {tempX1Minus}");
        var x0Value = fx0;
        double firstStep;
        double secondStep;
        var x = new List<double> { l0 };
        var values = new List<double>{fx0};
        var minusPlusMultiplayer = 1;
        string multiplayerString;
                
        if (tempX1Minus < x0Value)
        {
            minusPlusMultiplayer = -1;
            secondStep = tempX1Minus;
            multiplayerString = "-";
        }
        else
        {
            secondStep = tempX1Plus;
            multiplayerString = "+";
        }

        firstStep = x0Value;
        x.Add(x[^1] + step * minusPlusMultiplayer);
        var fx = F(xk, s, x[^1]);
        values.Add(fx);
        Console.WriteLine($"l{x.Count-1} = l{x.Count-2} {multiplayerString} d = {x[^1]}  f(l{x.Count - 1}) = {fx}");
                        
        while (firstStep > secondStep)
        {
            x.Add(x[^1] + step * multiplayer * minusPlusMultiplayer);
            var newValue = Math.Round(F(xk, s, x[^1]), Accuracy);
            values.Add(newValue);
            Console.WriteLine($"l{x.Count-1} = l{x.Count-2} {multiplayerString} {multiplayer}d = {Math.Round(x[^1], 2)}  f(l{x.Count - 1}) = {newValue}");
            firstStep = secondStep;
            secondStep = newValue;
            multiplayer *= 2;
        }
                
        var middle = (x[^1] + x[^2]) / 2;
        var temp = x[^1];
        x[^1] = middle;
        x.Add(temp);
        
        var fmid = Math.Round(F(xk, s, middle), Accuracy);
        temp = values[^1];
        values[^1] = fmid;
        values.Add(temp);
        
        Console.WriteLine($"l{x.Count} = (l{x.Count-1} / l{x.Count-2}) / 2 = {Math.Round(middle, 2)}  f(l{x.Count}) = {fmid}");
        
        var min = values.Min();
        var left = values.IndexOf(min) - 1;
        if (left < 0) left = 0;
        var right = values.IndexOf(min) + 1;

        double[] interval;
        if (x[left] <= x[right])
        {
            interval = [x[left], x[right]];
        }
        else
        {
            interval = [x[right], x[left]];
        }

        interval[0] = Math.Round(interval[0], Accuracy);
        interval[1] = Math.Round(interval[1], Accuracy);
        Console.WriteLine($"Iнтервал невизначеностi: [{Print(interval)}]\n");
        
        FindXS(out ls, isLast, interval, s, xk);

        return interval;
    }

    private static void FindXS(out double ls, bool isLast, double[] interval, double[] s, double[] xk)
    {
        ls = 0;
        double xs = 0;
        if (isLast)
        {
            Console.WriteLine("Метод ДСК Пауелла");

            var x1 = interval[0];
            var x2 = (interval[0] + interval[1]) / 2;
            var x3 = interval[1];

            double fx1 = Math.Round(F(xk, s, x1, false), Accuracy);
            double fx2 = Math.Round(F(xk, s, x2, false), Accuracy);
            double fx3 = Math.Round(F(xk, s, x3, false), Accuracy);
            
            var delta = x3-x2;
            
            var top = delta * (fx1 - fx3);
            var bot = 2 * (fx1 - 2 * fx2 + fx3);
            var x = x2 + top / bot;
            xs = Math.Round(x,Accuracy);
            var fxs = F(xk, s, xs, false);
            Console.WriteLine($"x1 = {x1}  x2 = {x2}  x3 = {x3}");
            Console.WriteLine($"x* = {xs}  fx* = {fxs}");
            
            Console.WriteLine("Виконується перевiрка на закiнчення пошуку: ");
            Console.WriteLine($"|fx2 - fx*| <= e");
            Console.WriteLine($"|x2 - x*| <= e");

            var result = Math.Abs(fx2 - fxs) <= E && Math.Abs(x2 - xs) <= E;
            Console.WriteLine($"|{fx2} - {fxs}| <= {E}    {Math.Abs(fx2-fxs) <= E}");
            Console.WriteLine($"|{x2} - {xs}| <= {E}       {Math.Abs(x2-xs) <= E}");

            if (result)
            {
                Console.WriteLine($"x* = {xs}");
                ls = xs;
            }
            else
            {
                Console.WriteLine("Умови закiнчення пошуку не виконуються.");
                List<double> values = new List<double>() { fxs, fx1, fx2, fx3 };
                List<double> listx = new List<double>() { xs, x1, x2, x3 };
                var max = FindMax(values);
                var indexMax = values.IndexOf(max);
                values.Remove(max);
                listx.RemoveAt(indexMax);

                var min = FindMin(values);
                var indexMin = values.IndexOf(min);

                x2 = listx[indexMin];
                fx2 = min;
                values.Remove(min);
                listx.RemoveAt(indexMin);

                if (values[0] <= values[1])
                {
                    x1 = listx[0];
                    fx1 = values[0];
                    x3 = listx[1];
                    fx3 = values[1];
                }
                else
                {
                    x1 = listx[1];
                    fx1 = values[1];
                    x3 = listx[0];
                    fx3 = values[0];
                }
                
                var a1 = (fx2 - fx1) / x2 - x1;
                a1 = Math.Round(a1, Accuracy);
                Console.WriteLine($"a1 = {a1}");

                var a2 = (1 / (x3 - x2)) * ((fx3-fx2)/(x3-x2) - (fx2-fx1)/(x2-x1));
                a2 = Math.Round(a2, Accuracy);
                Console.WriteLine($"a2 = {a2}");

                xs = (x1 + x2) / 2 - (a1 / (2 * a2));
                xs = Math.Round(xs, Accuracy);
                Console.WriteLine($"x* = ({x1} + {x2}) / 2 - {a1} / {2*a2} = {xs}");
                ls = xs;
            }
        }
    }

    private static void ShortSven(double l0, double step, double[] xk, double[] s, out double ls, bool isLast = false)
    {
        Console.WriteLine("Алгоритм Свена");
        var fx0 = F(xk, s, l0);
        Console.WriteLine($"l0 = {l0}  f(l0) = {fx0}");
        
        var multiplayer = 2;
        var tempX1Plus = F(xk, s, l0 + step);
        Console.WriteLine($"l0 + d = {l0 + step}  f(l0 + d) = {tempX1Plus}");
        var tempX1Minus =  F(xk, s, l0 - step);
        Console.WriteLine($"l0 - d = {l0 - step}  f(l0 - d) = {tempX1Minus}");
        List<double> values = [fx0, tempX1Minus, tempX1Plus];
        double[] interval;
        if (tempX1Minus <= tempX1Plus)
        {
            interval = [l0-step, l0+step];
        }
        else
        {
            interval = [l0+step, l0-step];
        }

        interval[0] = Math.Round(interval[0], Accuracy);
        interval[1] = Math.Round(interval[1], Accuracy);
        Console.WriteLine($"Iнтервал невизначеностi: [{Print(interval)}]\n");
        
        FindXS(out ls, isLast, interval, s, xk);
    }

    public static double F(double[] xn)
    {
        return F(xn[0],xn[1]);
    }

    private static double F(double[] x, double[] s, double l, bool print = true)
    {
        var x1 = Math.Round(x[0] + s[0] * l, Accuracy);
        var x2 = Math.Round(x[1] + s[1] * l, Accuracy);
        if(print) Console.Write($"x1 = {x1}   x2 = {x2}   ");
        return F(x1,x2);
    }

    private static void GoldCut(double[] startInterval, double e, double[] xk, double[] s)
    {
        Console.WriteLine("Метод золотого перетину");
        if (CheckInterval(startInterval, e)) return;

        var newInterval = startInterval;

        var x = 1;
        while (!CheckInterval(newInterval, e))
        {
            var l = newInterval[1] - newInterval[0];
            Console.WriteLine($"L = b - 1 = {Math.Round(l,Accuracy)}");
            var x1 = FindX1(newInterval, l);
            var x2 = FindX2(newInterval, l);
            
            Console.WriteLine($"Iнтервал на {x} кроцi: [{Math.Round(newInterval[0], Accuracy)}, {Math.Round(x1, Accuracy)}, {Math.Round(x2, Accuracy)}, {Math.Round(newInterval[1], Accuracy)}]");
                
            if (F(xk,s,x1, false) > F(xk,s,x2, false))
            {
                newInterval = [x1, newInterval[1]];
            }
            else
            {
                newInterval = [newInterval[0], x2];
            }

            newInterval = [Math.Round(newInterval[0], Accuracy), Math.Round(newInterval[1], Accuracy)];
            
            Console.WriteLine($"Значення: x1 = {Math.Round(x1, Accuracy)}, x2 = {Math.Round(x2,Accuracy)}");

            Console.WriteLine($"Iнтервал пiсля {x} кроцi: [{newInterval[0]}, {newInterval[1]}]");
            x++;
        }
                
        Console.WriteLine($"Iнтервал: [{Print(newInterval)}]");
    }

    private static double FindX2(IReadOnlyList<double> startInterval, double l)
    {
        return startInterval[0] + Math.Round(0.618 * l, Accuracy);
    }

    private static double FindX1(double[] startInterval, double l)
    {
        return startInterval[0] + Math.Round(0.382 * l, Accuracy);
    }
    
    private static bool CheckInterval(double[] startInterval, double e)
    {
        var l = startInterval[1] - startInterval[0];
        return l <= e;
    }
    
    static double FindMax(List<double> numbers)
    {
        double max = numbers[0];
        foreach (double number in numbers)
        {
            if (number >= max)
            {
                max = number;
            }
        }
        return max;
    }
    
    static double FindMin(List<double> numbers)
    {
        double min = numbers[0];
        foreach (double number in numbers)
        {
            if (number <= min)
            {
                min = number;
            }
        }
        return min;
    }
}