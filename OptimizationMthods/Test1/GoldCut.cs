namespace Optimization;

public class GoldCut
{
    public static double[] Calculate(double[] startInterval, double e)
    {
        if (CheckInterval(startInterval, e)) return startInterval;

        var newInterval = startInterval;

        var x = 1;
        while (!CheckInterval(newInterval, e))
        {
            var L = newInterval[1] - newInterval[0];
            Console.WriteLine($"L = b - 1 = {Math.Round(L,2)}");
            var x1 = FindX1(newInterval, L);
            var x2 = FindX2(newInterval, L);
            
            Console.WriteLine($"Iнтервал на {x} кроцi: [{Math.Round(newInterval[0], 2)}, {Math.Round(x1, 2)}, {Math.Round(x2, 2)}, {Math.Round(newInterval[1], 2)}]");
                
            if (EntryPoint.F(x1) > EntryPoint.F(x2))
            {
                newInterval = [x1, newInterval[1]];
            }
            else
            {
                newInterval = [newInterval[0], x2];
            }
            
            Console.WriteLine($"Значення: x1 = {Math.Round(x1, 2)}, x2 = {Math.Round(x2,2)}");

            Console.WriteLine($"Iнтервал пiсля {x} кроцi: [{Math.Round(newInterval[0], 2)}, {Math.Round(newInterval[1], 2)}]");
            x++;
        }
                
        return newInterval;
    }

    private static double FindX2(double[] startInterval, double L)
    {
        return startInterval[0] + Math.Round(0.618 * L, EntryPoint.Accuracy);
    }

    private static double FindX1(double[] startInterval, double L)
    {
        return startInterval[0] + Math.Round(0.382 * L, EntryPoint.Accuracy);
    }
    
    private static bool CheckInterval(double[] startInterval, double e)
    {
        var L = startInterval[1] - startInterval[0];
        if (L <= e)
        {
            {
                return true;
            }
        }

        return false;
    }
}