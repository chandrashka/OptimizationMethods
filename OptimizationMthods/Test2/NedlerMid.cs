namespace Optimization;

public class NedlerMid
{
    public static int Counter = 1;
    public static void Calculate(double[] x1, double[] x2, double[] x3, double a, double b, double g, double M)
    {
        var newX1 = x1;
        var newX2 = x2;
        var newX3 = x3;
        List<bool> isChanged = [false, false, false];
        var changeCounter = 1;
        double[] xl = [];
        double[] xh = [];
        double[] xg = [];
        var fx1 = EntryPoint.F(newX1[0], newX1[1]);
        var fx2 = EntryPoint.F(newX2[0], newX2[1]);
        var fx3 = EntryPoint.F(newX3[0], newX3[1]);
        int reducted = 0;
        
        while (reducted < 2)
        {
            Console.WriteLine("\nIтерацiя " + Counter);

            if (changeCounter >= M)
            {
                if (isChanged.Contains(false))
                {
                    changeCounter = Reduction(ref newX1, ref newX2, ref newX3, out isChanged);
                    reducted++;
                    Counter++;
                    continue;
                }
            }

            changeCounter++;
            
            fx1 = EntryPoint.F(newX1[0], newX1[1]);
            fx2 = EntryPoint.F(newX2[0], newX2[1]);
            fx3 = EntryPoint.F(newX3[0], newX3[1]);
            xl = [];
            xh = [];
            xg = [];

            CalculateTheSmallest(newX1, newX2, newX3, fx1, fx2, fx3, ref xl, ref xg, ref xh);

            Console.WriteLine($"Виконується пробне симетричне вiдображення (teta = a = 1) точки xh = ({xh[0]}, {xh[1]})^T");
            double[] xc = [Math.Round((xl[0] + xg[0]) / 2, EntryPoint.Accuracy), Math.Round((xl[1] + xg[1]) / 2, EntryPoint.Accuracy)];
            double[] xn = [Math.Round(2 * xc[0] - xh[0], EntryPoint.Accuracy), Math.Round(2 * xc[1] - xh[1], EntryPoint.Accuracy)];
            Console.WriteLine($"xc = (({xl[0]}, {xl[1]})^T) + ({xg[0]}, {xh[1]})^T)/2 = ({xc[0]}, {xc[1]})^T");
            Console.WriteLine($"xn = 2({xc[0]}, {xc[1]})^T + ({xh[0]}, {xh[1]})^T) = ({xn[0]}, {xn[1]})^T   f(xn) = {Math.Round(EntryPoint.F(xn), EntryPoint.Accuracy)}");
            
            if (EntryPoint.F(xn) <= EntryPoint.F(xl))
            {
                Console.WriteLine("Оскiльки f(xn) < f(xl), виконується вiдображення точки xh\nз розтягуванням teta = gamma = 2: ");
                double[] x4 = [Math.Round(3 * xc[0] - 2 * xh[0], EntryPoint.Accuracy), Math.Round(3 * xc[1] - 2 * xh[1], EntryPoint.Accuracy)];
                Console.WriteLine($"x = 3({xc[0]}, {xc[1]})^T - 2({xh[0]}, {xh[1]})^T) = ({x4[0]}, {x4[1]})^T   f(x) = {Math.Round(EntryPoint.F(x4),EntryPoint.Accuracy)}");
                if (newX1 == xh)
                {
                    newX1 = x4;
                    isChanged[0] = true;
                }
                else if (newX2 == xh)
                {
                    newX2 = x4;
                    isChanged[1] = true;
                }
                else if (newX3 == xh)
                {
                    newX3 = x4;
                    isChanged[2] = true;
                }
                Counter++;
                continue;
            }

            if (EntryPoint.F(xn) >= EntryPoint.F(xl) || EntryPoint.F(xn) >= EntryPoint.F(xh))
            {
                Console.WriteLine("Оскiльки f(xn) > f(xl), виконується вiдображення точки xh\nз розтягуванням teta = gamma = 2: ");
                double[] x4 = [Math.Round(b * xc[0] + b * xh[0], EntryPoint.Accuracy), Math.Round(b * xc[1] + b * xh[1], EntryPoint.Accuracy)];
                Console.WriteLine($"x = 0.5({xc[0]}, {xc[1]})^T + 0.5({xh[0]}, {xh[1]})^T) = ({x4[0]}, {x4[1]})^T    f(x) = {Math.Round(EntryPoint.F(x4),EntryPoint.Accuracy)}");
                if (newX1 == xh)
                {
                    newX1 = x4;
                    isChanged[0] = true;
                }
                else if (newX2 == xh)
                {
                    newX2 = x4;
                    isChanged[1] = true;
                }
                else if (newX3 == xh)
                {
                    newX3 = x4;
                    isChanged[2] = true;
                }
                Counter++;
                continue;
            }
        }

        CheckEnd(newX1, newX2, newX3);
    }

    private static void CheckEnd(double[] x1, double[] x2, double[] x3)
    {
        Console.WriteLine("\nОбчислимо критерiй закiнчення: ");
        double[] xc = [(x1[0] + x2[0] + x3[0]) / 3, (x1[1] + x2[1] + x3[1]) / 3];
        var fxc = EntryPoint.F(xc);
        var fx1 = EntryPoint.F(x1);
        var fx2 = EntryPoint.F(x2);
        var fx3 = EntryPoint.F(x3);

        var temp = (Math.Pow(fx1 - fxc, 2) + Math.Pow(fx2 - fxc, 2) + Math.Pow(fx3 - fxc, 2)) / 3;
        var result = Math.Round(Math.Sqrt(temp), EntryPoint.Accuracy);
        
        
        Console.WriteLine($"xc = ({Math.Round(xc[0], EntryPoint.Accuracy)}, {Math.Round(xc[1], EntryPoint.Accuracy)})  f(xc) = {Math.Round(fxc, EntryPoint.Accuracy)}");
        Console.WriteLine($"sum = sqrt ({Math.Round(Math.Pow(fx1 - fxc, 2), EntryPoint.Accuracy)} + {Math.Round(Math.Pow(fx2 - fxc, 2), EntryPoint.Accuracy)} + {Math.Round(Math.Pow(fx3 - fxc, 2), EntryPoint.Accuracy)}) / 3 = sqrt {Math.Round(temp, EntryPoint.Accuracy)}");
        Console.WriteLine($"Результат: {result} <= e   {result <= EntryPoint.E}");
    }

    private static int Reduction(ref double[] newX1, ref double[] newX2, ref double[] newX3, out List<bool> isChanged)
    {
        Console.WriteLine($"Оскiльки одна з вершин не виключається на протязi  iтерацiй, необхiдно \nпровести редукцiю вiдносно вершини з найменшим значенням цiльової функцiї");
        double fx1;
        double fx2;
        double fx3;
        int changeCounter;
        
        fx1 = EntryPoint.F(newX1[0], newX1[1]);
        fx2 = EntryPoint.F(newX2[0], newX2[1]);
        fx3 = EntryPoint.F(newX3[0], newX3[1]);

        if (fx1 <= fx2 && fx1 <= fx3)
        {
            newX2 = [(newX2[0] + newX1[0]) / 2, (newX2[1] + newX1[1]) / 2];
            newX3 = [(newX3[0] + newX1[0]) / 2, (newX3[1] + newX1[1]) / 2];
        }
        else if (fx2 <= fx1 && fx2 <= fx3)
        {
            newX1 = [(newX1[0] + newX2[0]) / 2, (newX1[1] + newX2[1]) / 2];
            newX3 = [(newX3[0] + newX2[0]) / 2, (newX3[1] + newX2[1]) / 2];
        }
        else if (fx3 <= fx1 && fx3 <= fx2)
        {
            newX1 = [(newX1[0] + newX3[0]) / 2, (newX1[1] + newX3[1]) / 2];
            newX2 = [(newX2[0] + newX3[0]) / 2, (newX2[1] + newX3[1]) / 2];
        }

        newX1 = [Math.Round(newX1[0], EntryPoint.Accuracy), Math.Round(newX1[1], EntryPoint.Accuracy)];
        newX2 = [Math.Round(newX2[0], EntryPoint.Accuracy), Math.Round(newX2[1], EntryPoint.Accuracy)];
        newX3 = [Math.Round(newX3[0], EntryPoint.Accuracy), Math.Round(newX3[1], EntryPoint.Accuracy)];
        
        Console.WriteLine("Отримаємо багатогранник з точками");
        Console.WriteLine($"({newX1[0]}, {newX1[1]})       f({newX1[0]}, {newX1[1]}) = {Math.Round(EntryPoint.F(newX1), EntryPoint.Accuracy)}");
        Console.WriteLine($"({newX2[0]}, {newX2[1]})       f({newX2[0]}, {newX2[1]}) = {Math.Round(EntryPoint.F(newX2), EntryPoint.Accuracy)}");
        Console.WriteLine($"({newX3[0]}, {newX3[1]})       f({newX3[0]}, {newX3[1]}) = {Math.Round(EntryPoint.F(newX3), EntryPoint.Accuracy)}");
        
        changeCounter = 1;
        isChanged = [false, false, false];
        return changeCounter;
    }

    private static void CalculateTheSmallest(double[] x1, double[] x2, double[] x3, double fx1, double fx2, double fx3,
        ref double[] xl, ref double[] xg, ref double[] xh)
    {
        Console.WriteLine(" Обчислюється значення цiльової функцiї в вершинах багатогранника i \nвiдповiдно цих значень визначаються вершини xh xg xl :");
        if (fx1 <= fx2 && fx1 <= fx3)
        {
            xl = x1;

            if (fx2 <= fx3)
            {
                xg = x2;
                xh = x3;
            }
            else
            {
                xg = x3;
                xh = x2;
            }
        }
        else if (fx2 <= fx1 && fx2 <= fx3)
        {
            xl = x2;

            if (fx1 <= fx3)
            {
                xg = x1;
                xh = x3;
            }
            else
            {
                xg = x3;
                xh = x1;
            }
        }
        else if (fx3 <= fx1 && fx3 <= fx2)
        {
            xl = x3;

            if (fx1 <= fx2)
            {
                xg = x1;
                xh = x2;
            }
            else
            {
                xg = x2;
                xh = x1;
            }
        }
        
        Console.WriteLine($"xl - ({xl[0]}, {xl[1]})    f(xl) = {EntryPoint.F(xl)}");
        Console.WriteLine($"xh - ({xh[0]}, {xh[1]})    f(xh) = {EntryPoint.F(xh)}");
        Console.WriteLine($"xg - ({xg[0]}, {xg[1]})    f(xg) = {EntryPoint.F(xg)}");
        Console.WriteLine();
    }
}