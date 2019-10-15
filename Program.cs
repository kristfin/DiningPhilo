using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiningPhilo
{
    class Program
    {
        static string[] names = new string[] {
            "Aristotle", "Kant", "Plato", 
            "Confusius","Hume", "Descartes" };

        static void Main(string[] args)
        {
            int max = names.Length;
            Console.WriteLine(max + " philosophers will dine");
                        
            var chopsticks = new List<object>();
            for (int i=0; i<max; i++)
            {
                chopsticks.Add(new String(""+i));
            }
                        
            var tasks = new List<Task>();
            for (int i = 0; i < max; i++)
            {
                int left = (max+i-1)% max;
                int right = i % max;
                int tmpI = i; 
                tasks.Add(new Task(() => new Philosopher(names[tmpI], chopsticks[left], chopsticks[right]).Dine()));              
            }
                        
            Parallel.ForEach(tasks, t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }
    }

    class Philosopher
    {
        static Random rand = new Random();
        public string Name { get; set; }
        public object LeftChopstick { get; set; }
        public object RightChopstick { get; set; }

        public Philosopher(string name, object left, object right)
        {
            this.Name = name;
            this.LeftChopstick = left;
            this.RightChopstick = right;
        }
        public void Dine()
        {
            Console.WriteLine(Name + " will dine");
            lock (LeftChopstick)
            {
                Console.WriteLine(Name + " got his left chopstick:"+(string)LeftChopstick);
                lock(RightChopstick)
                {
                    Console.WriteLine(Name + " got his right chopstick:"+(string)RightChopstick);
                    Console.WriteLine(Name + " dining");
                    Task.Delay(rand.Next(500, 2000)).Wait();
                }
            }
        }
    }
}
