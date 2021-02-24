#nullable enable
using System;

namespace DeadInside
{
    internal static class Program
    {
        private static void Main()
        {
            var random = new Random();

            var averageTimeInQueue = 0.0f;
            var firstDowntimeProbability = 0.0f;
            var secondDowntimeProbability = 0.0f;

            const int iterations = 25;
            
            for (var i = 1; i <= iterations; ++i)
            {
                var simulation = new Simulation(random);

                var result = simulation.Run();

                averageTimeInQueue += result.AverageTimeInQueue;
                firstDowntimeProbability += result.FirstDowntimeProbability;
                secondDowntimeProbability += result.SecondDowntimeProbability;
            }

            Console.WriteLine($"Average time in queue: {averageTimeInQueue / iterations}");
            Console.WriteLine($"Downtime probability of 1: {firstDowntimeProbability / iterations}");
            Console.WriteLine($"Downtime probability of 2: {secondDowntimeProbability / iterations}");
        }
    }
}