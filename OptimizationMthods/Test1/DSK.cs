namespace Optimization;

public class DSK
{
    public static double Calculate(double[] startInterval, double e)
        {
                var center = (startInterval[0] + startInterval[1])/2;

                var x = startInterval[1] + ((startInterval[1] - center) *
                                            (EntryPoint.F(startInterval[0] - EntryPoint.F(startInterval[1])) / (2*(EntryPoint.F(startInterval[0]) - 2*EntryPoint.F(center) + EntryPoint.F(startInterval[1])))));
                
                var newInterval = new double[3];
                if (center >= x)
                {
                        newInterval = new[] { center, x, startInterval[1] };
                }
                else
                {
                        newInterval = new[] { startInterval[0], center, x };
                }
                while(!(Math.Abs(EntryPoint.F(newInterval[2] - EntryPoint.F(x))) <= e && Math.Abs(newInterval[2] - x) <= e))
                {
                        var aFirst = (EntryPoint.F(newInterval[1] - EntryPoint.F(newInterval[0]))) /
                                     (newInterval[1] - newInterval[0]);
                        
                        var aSecond = (1 / (newInterval[2] - newInterval[1])) *
                                      ((EntryPoint.F(newInterval[2] - EntryPoint.F(newInterval[0]))) /
                                       (newInterval[2] - newInterval[0]) -
                                       (EntryPoint.F(newInterval[1] - EntryPoint.F(newInterval[0]))) /
                                       (newInterval[1] - newInterval[0]));

                        x = (newInterval[0] + newInterval[1]) / 2 - aFirst / (2 * aSecond);
                        center = newInterval[2];
                        
                        if (center >= x)
                        {
                                newInterval = new[] { center, x, newInterval[1] };
                        }
                        else
                        {
                                newInterval = new[] { newInterval[0], center, x };
                        }
                }
                
                return x;
        }
}