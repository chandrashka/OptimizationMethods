namespace Optimization.Test3;

public static class Test3
{
    public static double E = 0.001;
    public static double[][] H = [[6.0, -1], [-1, 8]];
    
    public static void Calculate()
    {
        double[] x0 = [-5.2, -5.2];
        var lConst = 1;
        
        // Console.WriteLine("1. Методом найшвидшого спуску, lconst=1,");
        // MNCConst.Calculate(x0,lConst);
        Console.WriteLine("2. Методом найшвидшого спуску, lopt,");
        var results = MNCOpt.Calculate(x0);
        Console.WriteLine("\n3. Партан – методом найшвидшого спуску");
        Partan.Calculate(results, x0);
        Console.WriteLine("\n4. Методом Ньютона.");
        Newton.Calculate(x0);
    }

    public static double F(double x1, double x2)
    {
        var result =  3 * Math.Pow(x1 - 1, 2) - x1 * x2 + 4 * x2 * x2;

        return Math.Round(result, EntryPoint.Accuracy);
    }

    public static double F(double[] x)
    {
        return F(x[0], x[1]);
    }

    public static double[] Gradient(double x1, double x2)
    {
        return [Math.Round(6 * x1 - 6 - x2, EntryPoint.Accuracy), Math.Round(- x1 + 8 * x2, EntryPoint.Accuracy)];
    }

    public static double GradNorm(double[] gradient)
    {
        var result = Math.Sqrt(gradient[0] * gradient[0] + gradient[1] * gradient[1]);

        return Math.Round(result, EntryPoint.Accuracy);
    }
}