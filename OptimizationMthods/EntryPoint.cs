using Optimization.RGR;
using Optimization.Test4;

public class EntryPoint
{
        public static int Accuracy = 3;
        public static double E = 0.01;
        
        public static void Main()
        {
                RGR.Calculate();
        }

        public static void Print(double[] interval)
        {
                Console.WriteLine($"Вiдповiдь: [{Math.Round(interval[0], 2)}, {Math.Round(interval[1], 2)}]");
        }

        public static double F(double x)
        {
                return Math.Round(Math.Pow(x, 2) - 2 * x, Accuracy);
        }

        public static double F(double x1, double x2)
        {
                //return Math.Round(2*Math.Pow(x1, 2) + x1*x2 + 8*Math.Pow(x2, 2), Accuracy);
                return Math.Round(3 * Math.Pow(x1 - 1, 2) - x1 * x2 + 4 * Math.Pow(x2, 2), Accuracy);
        }

        public static double F(double[] xn)
        {
                return F(xn[0],xn[1]);
        }
        
        public static double[] Gradient(double x1, double x2)
        {
                //return [Math.Round(4 * x1 + x2 + 8, Accuracy), Math.Round(x1 + 4 * x2, Accuracy)];
                return [Math.Round(6 * x1 - x2 - 6 , Accuracy), Math.Round(- x1 + 8 * x2, Accuracy)];
        }

        public static double GradNorm(double[] gradient)
        {
                var result = Math.Sqrt(gradient[0] * gradient[0] + gradient[1] * gradient[1]);

                return Math.Round(result, Accuracy);
        }
}