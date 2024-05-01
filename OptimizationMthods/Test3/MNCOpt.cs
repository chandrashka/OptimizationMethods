namespace Optimization.Test3;

public class MNCOpt
{
    public static double[][] Calculate(double[] x0)
    {
        Console.WriteLine();
        Console.WriteLine("df/dx1 = 6x1 - 6 - x2,     df/dx2 = -x1 + 8x2");
        Console.WriteLine("f(x0) = " + Test3.F(x0));

        var x = x0;
        var i = 0;
        double[][] results = [[0.0, 0], [0,0], [0,0]];

        while(true)
        {
            Console.WriteLine($"\nIтерацiя {i+1}");

            var grad = FindGradient(x, out var norm);

            var s0 = FindS(grad);
        
            Console.WriteLine($"S{i} = - gf(x0)/||gf(x0)|| = - ({grad[0]},{grad[1]})^T/{norm} = - ({s0[0]}, {s0[1]})^T");

            var l = FindL(grad, s0);
            
            Console.WriteLine($"\nВиконується перехiд в точку x{i+1}");
            var newX = FindNewX(x, s0, l);
            Console.WriteLine($"x{i+1} = ({newX[0]}, {newX[1]})^T");
            Console.WriteLine($"f(x{i+1}) = {Test3.F(newX)}");

            FindGradient(newX, out var newNorm);
            
            x = newX;
            results[i] = newX;

            if (norm <= Test3.E || i == 2)
            {
                Console.WriteLine("f(x3) < f(x2) => напрямок вибрано правильно");
                break;
            }

            i++;
        }
        
        Console.WriteLine("\nВiдповiдь: ");
        for (int j = 0; j < 3; j++)
        {
            Console.WriteLine($"x{j+1} = ({results[j][0]}, {results[j][1]})^T");
        }

        return results;
    }

    private static double FindL(double[] grad, double[] s)
    {
        var top = grad[0] * s[0] + grad[1] * s[1];
        double[] bot =
            [s[0] * Test3.H[0][0] + s[1] * Test3.H[1][0], s[0] * Test3.H[0][1] + s[1] * Test3.H[1][1]];
        var botres = bot[0] * s[0] + bot[1] * s[1];
        var result = Math.Round(top / botres, EntryPoint.Accuracy);
        Console.WriteLine($"l = {result}");
        return result;
    }

    private static double[] FindGradient(double[] x0, out double norm)
    {
        var grad = Test3.Gradient(x0[0], x0[1]);
        Console.WriteLine($"gf(x0) = ({grad[0]}, {grad[1]})^T");

        norm = Test3.GradNorm(grad);
        Console.WriteLine($"||gf(x0)|| = {norm}");
        return grad;
    }

    private static double[] FindNewX(double[] x0, double[] s0, double l)
    {
        var newX0 = x0[0] - l * s0[0];
        var newX1 = x0[1] - l * s0[1];
        
        Console.WriteLine($"x = ({x0[0]},{x0[1]})^T - {l} ({s0[0]}, {s0[1]})^T");

        return [Math.Round(newX0, EntryPoint.Accuracy), Math.Round(newX1, EntryPoint.Accuracy)];
    }

    private static double[] FindS(double[] grad)
    {
        var s0 = grad[0];
        var s1 = grad[1];

        return [Math.Round(s0, EntryPoint.Accuracy), Math.Round(s1, EntryPoint.Accuracy)];
    }
}