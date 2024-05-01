namespace Optimization;

public class Dyhotomia
{
    public static double[] Calculate(double[] startInterval, double e)
    { 
            if (CheckInterval(startInterval, e)) return startInterval;
            var x = 1;

            var result = startInterval;
            while (!CheckInterval(result, e))
            { 
                    var L = result[1] - result[0]; 
                    Console.WriteLine($"L = b - 1 = {Math.Round(L,2)}"); 
                    var interval = FindInterval(result); 
                    Console.WriteLine($"Iнтервал на {x} кроцi: [{Math.Round(interval[0], 2)}, {Math.Round(interval[1], 2)}, {Math.Round(interval[2], 2)}, {Math.Round(interval[3], 2)}, {Math.Round(interval[4], 2)}]"); 
                    var values = new double[interval.Length]; 
                    for (int i = 0; i < interval.Length; i++) 
                    { 
                            values[i] = EntryPoint.F(interval[i]);
                    }
                    Console.WriteLine($"Значення: [{Math.Round(values[1], 2)}, {Math.Round(values[2], 2)}, {Math.Round(values[3], 2)}]");
                    
                    result = CalculateNewInterval(interval, values); 
                    Console.WriteLine($"Iнтервал пiсля {x} кроцi: [{Math.Round(result[0], 2)}, {Math.Round(result[1], 2)}]"); 
                    x++;
            }
            
            return result;
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

    private static double[] CalculateNewInterval(double[] interval, double[] values)
    {
            var minimum = values[0];
            var index = 0;

            if (values[1] < values[2])
            {
                    return [interval[0], interval[2]];
                
            }

            if(values[3] < values[2])
            {
                    return [interval[2], interval[4]];
            }

            return [interval[1], interval[3]];
    }

    private static double[] FindInterval(double[] startInterval)
    {
            var newInterval = new double[5];
            newInterval[0] = startInterval[0];
            newInterval[4] = startInterval[1];
            newInterval[2] = (startInterval[1] + startInterval[0]) / 2;
            newInterval[1] = (newInterval[0] + newInterval[2]) / 2;
            newInterval[3] = (newInterval[4] + newInterval[2]) / 2;

            return newInterval;
    }
}