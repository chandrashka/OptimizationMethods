namespace Optimization;

public class Test1
{
    public void Calculate()
    {
        Console.WriteLine("1. Визначити  iнтервал невизначеностi (алгоритм Свена) при заданiй початковiй точцi x0=2 та величинi кроку = 0,1");
        var sven = Sven.Calculate(2, 0.1);
        EntryPoint.Print(sven);
                
        Console.WriteLine();
        Console.WriteLine("Використовуючи знайдений  iнтервал невизначеностi знайти мiнiмум функцiї з точнiстю 0,2 :");
        Console.WriteLine("а) методом дихотомiї,\n");
        var dyhotomia = Dyhotomia.Calculate(sven, 0.2);
        EntryPoint.Print(dyhotomia);

        Console.WriteLine("\nб) методом золотого перетину з точнiстю 10-2:");
        Console.WriteLine();
        var gold = GoldCut.Calculate(sven, 0.2);
        EntryPoint.Print(gold);

        Console.WriteLine("\nв) методом ДСК-Пауелла.");
        Console.WriteLine("Вiдповiдь: " + Math.Round(DSK.Calculate(sven, 0.2), 2));
    }
}