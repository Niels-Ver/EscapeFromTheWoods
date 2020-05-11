using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DataManagement dm = new DataManagement(@"Data Source=DESKTOP-FUU1BI0\SQLEXPRESS;Initial Catalog=Forest;Integrated Security=True");


            Forest forest1 = new Forest(0, 0, 100, 0, 100);
            Forest forest2 = new Forest(1, 0, 100, 0, 100);
            Forest forest3 = new Forest(2, 0, 100, 0, 100);
            Forest forest4 = new Forest(3, 0, 100, 0, 100);

            Monkey jack = new Monkey(1, "Jack");
            Monkey abu = new Monkey(2, "Abu");
            Monkey donkeyKong = new Monkey(3, "Donkey Kong");
            Monkey kingKong = new Monkey(4, "King Kong");

            forest1.monkeys.Add(jack);
            forest1.monkeys.Add(abu);
            forest1.monkeys.Add(donkeyKong);
            forest1.monkeys.Add(kingKong);

            forest2.monkeys.Add(jack);
            forest2.monkeys.Add(abu);
            forest2.monkeys.Add(donkeyKong);

            forest3.monkeys.Add(jack);
            forest3.monkeys.Add(abu);
            forest3.monkeys.Add(kingKong);

            forest4.monkeys.Add(jack);
            forest4.monkeys.Add(donkeyKong);
            forest4.monkeys.Add(kingKong);

            ForestGenerator forestGenerator = new ForestGenerator();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() => forestGenerator.process(forest1)));
            tasks.Add(Task.Run(() => forestGenerator.process(forest2)));
            tasks.Add(Task.Run(() => forestGenerator.process(forest3)));
            tasks.Add(Task.Run(() => forestGenerator.process(forest4)));


            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

        }
    }
}
