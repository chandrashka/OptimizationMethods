namespace Optimization;

public class Test2
{
    public static void Calculate()
    {
        // Console.WriteLine();
        // Console.WriteLine("Мiнiмiзувати функцiю f(x) = (x1 - 2)^2 + 2x2^2 методом Хука-Дживса. Початкова точка x0 = (5,6)^T, \nзначення приросту dx = (0.6, 0.8)^T");
        // Console.WriteLine("Визначити наступнi базовi точки: x1,x2, x3.");
        // Console.WriteLine("Обчислити критерiй закiнчення. ");
        // Console.WriteLine();
        // HukJivs.Calculate([5,6], [0.6, 0.8], EntryPoint.F([5,6]));
        
        Console.WriteLine();
        Console.WriteLine("Мiнiмiзувати функцiю f(x) = 4(x1 - 2)^2 + (x2 - 1)^2 методом Нелдера-Мiда. \nПочатковий багатогранник x^(1) = (7,5)^T, x^(2) = (9,7)^T, x^(3) = (7,7)^T.");
        Console.WriteLine("Параметри вiдображення a = 1, b = +-10,5, g = 2, М=3. ");
        Console.WriteLine("Обчислення закiнчити пiсля проведення другої редукцiї. ");
        Console.WriteLine("Обчислити критерiй закiнчення. ");
        NedlerMid.Calculate([7,5], [9,7], [7,7], 1, 0.5, 2, 3);
    }
}