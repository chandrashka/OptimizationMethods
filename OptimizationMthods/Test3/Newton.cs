namespace Optimization.Test3;

public class Newton
{
    public static double[][] H = [[6.0, -1], 
                                    [-1, 8]];
    public static double[][] HMinus = [[8.0, 1], 
                                        [1, 6]];
    public static double coef = 47;
    
    public static void Calculate(double[] x0)
    {
        Console.WriteLine();
        Console.WriteLine("df/dx1 = 6x1 - 6 - x2,     df/dx2 = -x1 + 8x2");
        Console.WriteLine("f(x0) = " + Test3.F(x0));
        
        var grad = FindGradient(x0);
        var x1 = FindX(x0, grad);
        
        Console.WriteLine($"x1 = x0 - [H(x0)]^-1 gf(x0) = ({x0[0]}, {x0[1]})^T - 1/{coef} [({HMinus[0][0]}  {HMinus[0][1]})({HMinus[1][0]}  {HMinus[1][1]})] ({grad[0]}, {grad[1]})^T = ({x1[0]}, {x1[1]})^T");
    }

    private static double[] FindX(double[] x0, double[] grad)
    {
        var first = (1 / coef) * (grad[0] * HMinus[0][0] + grad[1]*HMinus[0][1]);
        var second = (1 / coef) * (grad[0] * HMinus[1][0] + grad[1]*HMinus[1][1]);

        return [Math.Round(x0[0] - first, EntryPoint.Accuracy), Math.Round(x0[1] - second, EntryPoint.Accuracy)];
    }

    private static double[] FindGradient(double[] x0)
    {
        var grad = Test3.Gradient(x0[0], x0[1]);
        Console.WriteLine($"gf(x0) = ({grad[0]}, {grad[1]})^T");

        return grad;
    }
}