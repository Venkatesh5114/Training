using System;
using System.Linq;
namespace A01;


class Program
{
    static void Main(string[] args)
    {
        Enumerable.Range(1,10).Where(x => x%2 ==0).Select(x => x*x).ToList().ForEach(Console.WriteLine);
    }
}
