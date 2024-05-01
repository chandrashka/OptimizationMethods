namespace Optimization.Test4;

public static class DFP
{
    private static double[][] A0 = [[1.0,0], [0.0,1]]; 
    
    public static void Calculate(double[] x0)
    {
        Console.WriteLine("\nМетодом Девiдона-Флетчера-Пауелла визначити точки: x(1), x(2).\nОбчислити матрицю А2 та порiвняти її з оберненою матрицею Гессе.");
        Console.WriteLine();
        var gradient = Test4.FindGradient(x0, out var norm);
        var A = A0;
        var x = x0;
        var allX = new List<double[]>();
        
        for (int i = 1; i < 3; i++)
        {
            Console.WriteLine($"\nIтерацiя {i}");
            
            if (i > 1)
            {
                A = FindA(x0, x, A0);
            }
            
            var s = FindS(gradient, A);
            var l = FindL(gradient, s);
            
            Console.WriteLine($"\nВиконується перехiд в точку x{i}:");
            x = FindX(x, l, s);
            allX.Add(x);
            gradient = Test4.FindGradient(x, out norm);
        }

        Console.WriteLine("Вiдзначимо також, що матриця A2 прагне до H-1: ");
        var reverse = FindInverse(Test4.H);
        
        Console.WriteLine($"H = [{Test4.H[0][0]} {Test4.H[0][1]}][{Test4.H[1][0]} {Test4.H[1][1]}]");
        Console.WriteLine($"H-1 = [{reverse[0][0]} {reverse[0][1]}][{reverse[1][0]} {reverse[1][1]}]");
        
        A = FindA(allX[^2], allX[^1], A);
    }

    private static double[][] FindA(double[] x0, double[] x, double[][] A)
    {
        double[] dx = [x[0] - x0[0], x[1] - x0[1]];
        
        var gx0 = Test4.FindGradient(x0, out _);
        var gx = Test4.FindGradient(x, out _);
        double[] dg = [gx[0] - gx0[0], gx[1] - gx0[1]];
        
        Console.WriteLine($"dx = ({x[0]}, {x[1]})^T - ({x0[0]}, {x0[1]})^T = ({dx[0]}, {dx[1]})^T");
        Console.WriteLine($"dg = ({gx[0]}, {gx[1]})^T - ({gx0[0]}, {gx0[1]})^T = ({dg[0]}, {dg[1]})^T");

        var secondTop = MultiplyMatrices([[dx[0]], [dx[1]]], [[dx[0], dx[1]]]);
        var secondBot = MultiplyMatrices([[dx[0], dx[1]]], [[dg[0]], [dg[1]]]);

        var thirdTopFirst = MultiplyMatrices(A, [[dg[0]], [dg[1]]]);
        var thirdTopSecond = MultiplyMatrices(thirdTopFirst, [[dg[0], dg[1]]]);
        var thirdTop = MultiplyMatrices(thirdTopSecond, A);

        var thirdBotFirst = MultiplyMatrices([[dg[0], dg[1]]], A);
        var thirdBot = MultiplyMatrices(thirdBotFirst, [[dg[0]], [dg[1]]]);

        var second = Divide(secondTop, secondBot);
        var third = Divide(thirdTop, thirdBot);

        double[][] result = [[A[0][0] + second[0][0] - third[0][0], A[0][1] + second[0][1] - third[0][1]], 
            [A[1][0] + second[1][0] - third[1][0], A[1][1] + second[1][1] - third[1][1]]];

        foreach (var t in result)
        {
            for (int j = 0; j < result[0].Length; j++)
            {
                t[j] = Math.Round(t[j], EntryPoint.Accuracy);
            }
        }
        
        Console.WriteLine($"A = [{result[0][0]} {result[0][1]}][{result[1][0]} {result[1][1]}]");
        return result;
    }

    private static double[][] Divide(double[][] secondTop, double[][] secondBot)
    {
        var result = secondTop;

        for (int i = 0; i < secondTop.Length; i++)
        {
            for (int j = 0; j < secondTop[0].Length; j++)
            {
                result[i][j] /= secondBot[0][0];
            }
        }
        
        return result;
    }

    private static double[] FindX(double[] x, double l, double[] gradient)
    {
        var x1 = x[0] + l * gradient[0];
        var x2 = x[1] + l * gradient[1];

        x1 = Math.Round(x1, EntryPoint.Accuracy);
        x2 = Math.Round(x2, EntryPoint.Accuracy);
        
        Console.WriteLine($"x = ({x[0]}, {x[1]})^T + {l} * ({gradient[0]}, {gradient[1]})^T = ({x1}, {x2})^T");

        return [x1, x2];
    }

    static double[][] MultiplyMatrices(double[][] matrix1, double[][] matrix2)
    {
        int rows1 = matrix1.Length;
        int cols1 = matrix1[0].Length;
        int cols2 = matrix2[0].Length;

        double[][] result = new double[rows1][];

        for (int i = 0; i < rows1; i++)
        {
            result[i] = new double[cols2];
            for (int j = 0; j < cols2; j++)
            {
                for (int k = 0; k < cols1; k++)
                {
                    result[i][j] += matrix1[i][k] * matrix2[k][j];
                }
            }
        }

        return result;
    }
    
    private static double[] FindS(double[] x0, double[][] A)
    {
        var x1 = -(A[0][0] * x0[0] + A[0][1]*x0[1]);
        var x2 = -(A[1][0] * x0[0] + A[1][1]*x0[1]);
        
        x1 = Math.Round(x1, EntryPoint.Accuracy);
        x2 = Math.Round(x2, EntryPoint.Accuracy);
        
        Console.WriteLine($"S = -A * df(x) = - [{A[0][0]} {A[0][1]}][{A[1][0]} {A[1][1]}] * ({x0[0]}, {x0[1]})^T = ({x1}, {x2})^T");
        
        return [x1,x2];
    }
    
    private static double FindL(double[] grad, double[] s)
    {
        var top = grad[0] * s[0] + grad[1] * s[1];
        double[] bot =
            [s[0] * Test4.H[0][0] + s[1] * Test4.H[1][0], s[0] * Test4.H[0][1] + s[1] * Test4.H[1][1]];
        var botres = bot[0] * s[0] + bot[1] * s[1];
        var result = Math.Round(top / botres, EntryPoint.Accuracy);
        Console.WriteLine($"l = {-result}");
        return -result;
    }
    
    public static double[][] FindInverse(double[][] matrix)
    {
        int n = matrix.Length;
        double[][] inverse = new double[n][];

        for (int i = 0; i < n; i++)
        {
            inverse[i] = new double[n];
        }

        double det = Determinant(matrix);

        if (det == 0)
        {
            Console.WriteLine("Матрица не обратима.");
            return null;
        }

        double detInverse = 1.0 / det;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double[][] minor = GetMinor(matrix, i, j);
                inverse[j][i] = detInverse * Math.Pow(-1, i + j) * Determinant(minor);
            }
        }

        foreach (var t in inverse)
        {
            for (int j = 0; j < inverse[0].Length; j++)
            {
                t[j] = Math.Round(t[j], EntryPoint.Accuracy);
            }
        }
        
        return inverse;
    }

    private static double Determinant(double[][] matrix)
    {
        int n = matrix.Length;

        if (n == 1)
            return matrix[0][0];

        double det = 0;

        for (int j = 0; j < n; j++)
        {
            double[][] minor = GetMinor(matrix, 0, j);
            det += Math.Pow(-1, 0 + j) * matrix[0][j] * Determinant(minor);
        }

        return det;
    }

    private static double[][] GetMinor(double[][] matrix, int row, int col)
    {
        int n = matrix.Length;
        double[][] minor = new double[n - 1][];

        for (int i = 0, k = 0; i < n; i++)
        {
            if (i == row)
                continue;

            minor[k] = new double[n - 1];

            for (int j = 0, l = 0; j < n; j++)
            {
                if (j == col)
                    continue;

                minor[k][l] = matrix[i][j];
                l++;
            }

            k++;
        }

        return minor;
    }
}