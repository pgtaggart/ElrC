using System;
using System.Diagnostics;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main()
        {

            var lib = new Library();
            //var one = new Problems(lib);
            var two = new ProblemsTwenty(lib);
            var one = new Problems(lib);
            var three = new NewProblems(lib);
            var cp = new CombinationProblems(lib);
            var n = new NewProblems(lib);
            var s = Stopwatch.StartNew();

            Console.Out.WriteLine("Starting...");
            Console.Out.WriteLine("");

            three.Problem98();

            s.Stop();

            Console.Out.WriteLine();
            Console.Out.WriteLine("Finished in {0}", s.Elapsed);
            Console.ReadLine();
        }


    }

}