namespace Optimization;

public class HukJivs
{
    public static int counter = 1;
    public static void Calculate(double[] x0, double[] deltax, double fx0)
    {
        var xr = x0;
        var fxr = fx0;
        double[] pbtx = x0;
        var newDeltaX = deltax;
        List<double[]> basisX = [x0];
        double[] lastpbtX = [];

        while(counter < 5)
        {
            Console.WriteLine("Iтерацiя " + counter);
            lastpbtX = pbtx;
            pbtx = FindPBT(xr, newDeltaX, fxr, basisX.Count);
            Console.WriteLine($"Отримана точка x^({basisX.Count}) = ({pbtx[0]}, {pbtx[1]})^T");
            var lastBtx = basisX[^1];
            Console.WriteLine();

            if (counter == 1)
            {
                Console.WriteLine($"Таким чином в результатi дослiджуючого пошуку знайдена \nпоточна  базисна   точка  (ПБТ) x^(1) = ({pbtx[0]}, {pbtx[1]})^T");
                Console.WriteLine();
                basisX = new List<double[]>() { basisX[0], basisX[0]};
            }
            
            if (EntryPoint.F(lastpbtX[0], lastpbtX[1]) < EntryPoint.F(pbtx[0], pbtx[1]))
            {
                Console.WriteLine($"Оскльки f({pbtx[0]}, {pbtx[1]}) = {Math.Round(EntryPoint.F(pbtx[0], pbtx[1]), EntryPoint.Accuracy)} > f(x^({basisX.Count})) = f({lastpbtX[0]}, {lastpbtX[1]}) = {Math.Round(EntryPoint.F(lastpbtX[0], lastpbtX[1]), EntryPoint.Accuracy)}");
                Console.WriteLine($"пошук не є успiшним, необхiдно зменшити значення приросту:");
                newDeltaX = [Math.Round(newDeltaX[0]/2,EntryPoint.Accuracy), Math.Round(newDeltaX[1]/2, EntryPoint.Accuracy)];
                Console.WriteLine($"dxn = dx/2 = ({newDeltaX[0]}, {newDeltaX[1]})^T");
                xr = basisX[^1];
                fxr = Math.Round(EntryPoint.F(xr[0], xr[1]), EntryPoint.Accuracy);
                pbtx = xr;
                Console.WriteLine();
                counter++;
                continue;
            }

            if(counter > 1)
            {
                Console.WriteLine($"Оскльки f({pbtx[0]}, {pbtx[1]}) = {Math.Round(EntryPoint.F(pbtx[0], pbtx[1]), EntryPoint.Accuracy)} < f(x^({basisX.Count - 1})) = f({lastpbtX[0]}, {lastpbtX[1]}) = {Math.Round(EntryPoint.F(lastpbtX[0], lastpbtX[1]), EntryPoint.Accuracy)}");
                Console.WriteLine($"Точка x^({basisX.Count-1}) = ({lastpbtX[0]}, {lastpbtX[1]})^T стає базисною, а точка x^({basisX.Count}) = ({pbtx[0]}, {pbtx[1]})^T - поточною базисною");
                basisX.Add(lastpbtX);
            }
            
            xr = FindXr(pbtx, basisX[^1], basisX.Count());
            fxr = Math.Round(EntryPoint.F(xr[0], xr[1]), EntryPoint.Accuracy);
            Console.WriteLine();

            counter++;
        }

        Console.WriteLine($"Базиснi точки: x^(1) = ({basisX[2][0]}, {basisX[2][1]})^T   x^(2) = ({basisX[3][0]}, {basisX[3][1]})^T    x^(3) = ({basisX[4][0]}, {basisX[4][1]})^T");
        CheckEnd(basisX[^1], pbtx, newDeltaX);
    }

    private static void CheckEnd(double[] xk, double[] xk1, double[] delta)
    {
        Console.WriteLine("Обчислимо критерiй закiнчення: ");
        
        Console.WriteLine("Спосiб 1:");
        Console.WriteLine($"||dx|| = sqrt({delta[0]} ^ 2, {delta[1]} ^ 2) = {Math.Sqrt(delta[0]*delta[0] + delta[1]*delta[1])} <= e    {Math.Sqrt(delta[0]*delta[0] + delta[1]*delta[1]) <= EntryPoint.E}");
        
        Console.WriteLine("Спосiб 2:");
        Console.WriteLine($"||({xk1[0]}, {xk1[1]})^T - ({xk[0]}, {xk[1]})^T|| / ||({xk[0]}, {xk[1]})^T|| = {Math.Round(Math.Sqrt(Math.Pow(xk1[0] - xk[0], 2) + Math.Pow(xk1[1] - xk[1], 2)) / Math.Sqrt(Math.Pow(xk[0],2) + Math.Pow(xk[1], 2)), EntryPoint.Accuracy)} <= e   {Math.Sqrt(Math.Pow(xk[0] - xk1[0], 2) + Math.Pow(xk[1] - xk1[1], 2)) / Math.Sqrt(Math.Pow(xk[0] + xk[1], 2)) <= EntryPoint.E}");
        Console.WriteLine($"|{Math.Round(EntryPoint.F(xk1[0], xk1[1]), EntryPoint.Accuracy)} - {Math.Round(EntryPoint.F(xk[0], xk[1]), EntryPoint.Accuracy)}| / |{Math.Round(EntryPoint.F(xk[0], xk[1]), EntryPoint.Accuracy)}| = {Math.Round(Math.Abs(EntryPoint.F(xk[0], xk[1]) - EntryPoint.F(xk1[0], xk1[1]))/Math.Abs(EntryPoint.F(xk[0], xk[1])), EntryPoint.Accuracy)} <= e      {Math.Round(Math.Abs(EntryPoint.F(xk[0], xk[1]) + EntryPoint.F(xk1[0], xk1[1]))/Math.Abs(EntryPoint.F(xk[0], xk[1])), EntryPoint.Accuracy) <= EntryPoint.E}");
    }

    private static double[] FindXr(double[] newX, double[] delta, int basisXXount)
    {
        double[] result = [Math.Round(2*newX[0] - delta[0], EntryPoint.Accuracy), Math.Round(2*newX[1] - delta[1],EntryPoint.Accuracy)];
        Console.WriteLine($"xp = 2x^({basisXXount-1}) - x^({basisXXount-2}) = ({result[0]}, {result[1]})^T      f(xp) = {Math.Round(EntryPoint.F(result[0], result[1]), EntryPoint.Accuracy)}");
        return result;
    }

    private static double[] FindPBT(double[] x0, double[] deltax, double fx0, int basisXCount)
    {
        var x1Plus = Math.Round(x0[0] + deltax[0], EntryPoint.Accuracy);
        var x1Minus = Math.Round(x0[0] - deltax[0], EntryPoint.Accuracy);
        var fx1Plus = Math.Round(EntryPoint.F(x1Plus, x0[1]), EntryPoint.Accuracy);
        var fx1Minus = Math.Round(EntryPoint.F(x1Minus, x0[1]), EntryPoint.Accuracy);
        var newX1 = fx1Plus < fx1Minus ? x1Plus : x1Minus;
        
        Console.WriteLine($"x1^({basisXCount}) = {x0[0]} + {deltax[0]} = {x1Plus}    f({x1Plus}, {x0[1]}) = {fx1Plus} {ChooseSymbol(fx1Plus, fx0)} {fx0}     {(fx1Plus < fx0).ToString()}");
        Console.WriteLine($"x1^({basisXCount}) = {x0[0]} - {deltax[0]} = {x1Minus}    f({x1Minus}, {x0[1]}) = {fx1Minus} {ChooseSymbol(fx1Minus, fx0)} {fx0}     {(fx1Minus < fx0).ToString()}");
        
        if (fx1Plus != fx1Minus)
        {
            var x2Plus = Math.Round(x0[1] + deltax[1], EntryPoint.Accuracy);
            var x2Minus = Math.Round(x0[1] - deltax[1], EntryPoint.Accuracy);
            var fx2Plus = Math.Round(EntryPoint.F(newX1, x2Plus), EntryPoint.Accuracy);
            var fx2Minus = Math.Round(EntryPoint.F(newX1, x2Minus), EntryPoint.Accuracy);
            var newX2 = fx2Plus < fx2Minus ? x2Plus : x2Minus;
            
            Console.WriteLine($"x2^({basisXCount}) = {x0[1]} + {deltax[1]} = {x2Plus}    f({newX1}, {x2Plus}) = {fx2Plus} {ChooseSymbol(fx2Plus, fx0)} {fx0}     {(fx2Plus < fx0).ToString()}");
            Console.WriteLine($"x2^({basisXCount}) = {x0[1]} - {deltax[1]} = {x2Minus}    f({newX1}, {x2Minus}) = {fx2Minus} {ChooseSymbol(fx2Minus, fx0)} {fx0}     {(fx2Minus < fx0).ToString()}");
        
            return [newX1, newX2];
        }
        else
        {
            Console.WriteLine("Так як значення вийшли однакові, почнемо спочатку рухатись по х2");
            newX1 = x0[0];
            var x2Plus = Math.Round(x0[1] + deltax[1], EntryPoint.Accuracy);
            var x2Minus = Math.Round(x0[1] - deltax[1], EntryPoint.Accuracy);
            var fx2Plus = Math.Round(EntryPoint.F(newX1, x2Plus), EntryPoint.Accuracy);
            var fx2Minus = Math.Round(EntryPoint.F(newX1, x2Minus), EntryPoint.Accuracy);
            var newX2 = fx2Plus < fx2Minus ? x2Plus : x2Minus;
            
            x1Plus = Math.Round(x0[0] + deltax[0], EntryPoint.Accuracy);
            x1Minus = Math.Round(x0[0] - deltax[0], EntryPoint.Accuracy);
            fx1Plus = Math.Round(EntryPoint.F(x1Plus, newX2), EntryPoint.Accuracy);
            fx1Minus = Math.Round(EntryPoint.F(x1Minus, newX2), EntryPoint.Accuracy);

            if (fx1Minus == fx1Plus) newX1 = x0[0];
            else newX1 = fx1Plus < fx1Minus ? x1Plus : x1Minus;
            
            Console.WriteLine($"x2^({basisXCount}) = {x0[1]} + {deltax[1]} = {x2Plus}    f({x0[0]}, {x2Plus}) = {fx2Plus} {ChooseSymbol(fx2Plus, fx0)} {fx0}     {(fx2Plus < fx0).ToString()}");
            Console.WriteLine($"x2^({basisXCount}) = {x0[1]} - {deltax[1]} = {x2Minus}    f({x0[0]}, {x2Minus}) = {fx2Minus} {ChooseSymbol(fx2Minus, fx0)} {fx0}     {(fx2Minus < fx0).ToString()}");

            Console.WriteLine($"x1^({basisXCount}) = {x0[0]} + {deltax[0]} = {x1Plus}    f({x1Plus}, {newX2}) = {fx1Plus} {ChooseSymbol(fx1Plus, fx0)} {fx0}     {(fx1Plus < fx0).ToString()}");
            Console.WriteLine($"x1^({basisXCount}) = {x0[0]} - {deltax[0]} = {x1Minus}    f({x1Minus}, {newX2}) = {fx1Minus} {ChooseSymbol(fx1Minus, fx0)} {fx0}     {(fx1Minus < fx0).ToString()}");
            Console.WriteLine($"x1^({basisXCount}) = {x0[0]}    f({x0[0]}, {newX2}) = {EntryPoint.F(x0[0], newX2)} {ChooseSymbol(EntryPoint.F(x0[0], newX2), fx0)} {fx0}     {(EntryPoint.F(x0[0], newX2) < fx0).ToString()}");
            
            return [newX1, newX2];
        }
    }

    private static string ChooseSymbol(double myFunction, double fx0)
    {
        return myFunction < fx0 ? "<" : ">";
    }
}