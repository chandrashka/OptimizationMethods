namespace Optimization;

public class Sven
{
    public static double[] Calculate(double x0, double step)
    {
        Console.WriteLine($"x0 = {x0}  f(x0) = {EntryPoint.F(x0)}");
        
        var multiplyer = 2;
        var tempX1Plus = EntryPoint.F(x0 + step);
        Console.WriteLine($"x0 + d = {x0 + step}  f(x0 + d) = {tempX1Plus}");
        var tempX1Minus = EntryPoint.F(x0 - step);
        Console.WriteLine($"x0 - d = {x0 - step}  f(x0 - d) = {tempX1Minus}");
        var x0Value = EntryPoint.F(x0);
        double firstStep = 0;
        double secondStep = 0;
        var x = new List<double> { x0 };
        var minusPlusMupltiplyer = 1;
        var multiplyerString = " ";
                
        if (tempX1Minus < x0Value)
        {
            minusPlusMupltiplyer = -1;
            secondStep = tempX1Minus;
            multiplyerString = "-";
        }
        else
        {
            secondStep = tempX1Plus;
            multiplyerString = "+";
        }

        firstStep = x0Value;
        x.Add(x[^1] + step * minusPlusMupltiplyer);
        Console.WriteLine($"x{x.Count-1} = x{x.Count-2} {multiplyerString} d = {x[^1]}  f(x{x.Count - 1}) = {EntryPoint.F(x[^1])}");
                        
        while (firstStep > secondStep)
        {
            x.Add(x[^1] + step * multiplyer * minusPlusMupltiplyer);
            var newValue = EntryPoint.F(x[^1]);
            Console.WriteLine($"x{x.Count-1} = x{x.Count-2} {multiplyerString} {multiplyer}d = {Math.Round(x[^1], 2)}  f(x{x.Count - 1}) = {Math.Round(EntryPoint.F(x[^1]),2)}");
            firstStep = secondStep;
            secondStep = newValue;
            multiplyer *= 2;
        }
                
        var middle = (x[^1] + x[^2]) / 2;
        var secondEnd = x[^2];
        var firstend = x[^1];

        Console.WriteLine($"x{x.Count} = (x{x.Count-2} / x{x.Count-3}) / 2 = {Math.Round(middle, 2)}  f(x{x.Count}) = {Math.Round(EntryPoint.F(middle), 2)}");
        
        if (firstend > secondEnd)
        {
            return [secondEnd, firstend];
        }
                
        return [firstend,secondEnd];
    }
}