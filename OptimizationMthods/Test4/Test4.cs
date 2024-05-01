namespace Optimization.Test4;

public class Test4
{
    public static double[][] H = [[6.0, -1], [-1, 8]];
    public static void Calculate()
    {
        double[] x0 = [-5.2,-5.2]; 
        DFP.Calculate(x0);
        FP.Calculate(x0);
    }
    
    public static double[] FindGradient(double[] x0, out double norm)
    {
        var grad = EntryPoint.Gradient(x0[0], x0[1]);
        Console.WriteLine($"gf(x0) = ({grad[0]}, {grad[1]})^T");

        norm = EntryPoint.GradNorm(grad);
        Console.WriteLine($"||gf(x0)|| = {norm}");
        return grad;
    }

    
}