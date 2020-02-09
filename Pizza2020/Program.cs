using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pizza2020
{
    class Program
    {
        static void Main(string[] args)
        {
            //var lines = System.IO.File.ReadAllLines(@"C:\Users\sviat\source\repos\Pizza2020\Pizza2020\b_small.in");
            var lines = System.IO.File.ReadAllLines(@"C:\Users\sviat\source\repos\Pizza2020\Pizza2020\c_medium.in");
            //var lines = System.IO.File.ReadAllLines(@"C:\Users\sviat\source\repos\Pizza2020\Pizza2020\a_example.in");

            var (d, indexes) = CalculateMatrix(lines);

            var maxPieces = int.Parse(lines[0].Split(" ")[0]);
            var totalPizzas = int.Parse(lines[0].Split(" ")[1]);
            var pizzas = lines[1].Split(" ").Select(int.Parse).ToList();

            PrintMatrix(d, totalPizzas, maxPieces);

            Console.WriteLine();

            foreach(var index in indexes)
            {
                Console.Write($"{pizzas[index]} ");
            }

            var resultFileStrings = new List<string>();
            resultFileStrings.Add(indexes.Count.ToString());
            resultFileStrings.Add(string.Join(" ", indexes));

            var outPath = @"C:\Users\sviat\source\repos\Pizza2020\Pizza2020\out.out";

            if (File.Exists(outPath))
            {
                File.Delete(outPath);
            }

            File.WriteAllLines(outPath, resultFileStrings);

            Console.ReadLine();
        }

        static (int[,], List<int>) CalculateMatrix(string[] lines)
        {
            var maxPieces = int.Parse(lines[0].Split(" ")[0]);
            var totalPizzas = int.Parse(lines[0].Split(" ")[1]);
            var pizzas = lines[1].Split(" ").Select(int.Parse).ToList();

            var d = new int[totalPizzas + 1, maxPieces + 1];

            var evenIndexes = new List<int>[maxPieces + 1];
            var oddIndexes = new List<int>[maxPieces + 1];

            for (var i = 0; i < maxPieces + 1; i++)
            {
                evenIndexes[i] = new List<int>();
                oddIndexes[i] = new List<int>();
            }

            var currentIndexes = new List<int>[maxPieces + 1];
            var previousIndexes = new List<int>[maxPieces + 1];

            for (int i = 1; i <= totalPizzas; i++)
            {
                if (i%2 == 0)
                {
                    currentIndexes = evenIndexes;
                    previousIndexes = oddIndexes;
                }
                else
                {
                    currentIndexes = oddIndexes;
                    previousIndexes = evenIndexes;
                }

                foreach(var current in currentIndexes)
                {
                    current.Clear();
                }

                var currentWeight = pizzas[i - 1];

                for(int k = 0; k < currentWeight; k++)
                {
                    currentIndexes[k].AddRange(previousIndexes[k]);
                }

                for (int j = 0; j <= maxPieces; j++)
                {
                    if (j < currentWeight)
                    {
                        d[i, j] = d[i - 1, j];
                    }
                    else
                    {
                        var previous = d[i - 1, j];
                        var better = d[i - 1, j - currentWeight] + currentWeight;

                        if (previous > better)
                        {
                            d[i, j] = previous;
                            currentIndexes[j].AddRange(previousIndexes[j]);
                        }
                        else
                        {
                            d[i, j] = better;
                            currentIndexes[j].AddRange(previousIndexes[j - currentWeight]);
                            currentIndexes[j].Add(i - 1);
                        }
                    }

                    if (d[i, j] == maxPieces)
                    {
                        return (d, currentIndexes[j]);
                    }
                }
            }

            return (d, currentIndexes[maxPieces]);
        }

        static void PrintMatrix(int[,] matrix, int length, long capacity)
        {
            for (int i = 0; i < length + 1; i++)
            {
                for (int j = 0; j < capacity + 1; j++)
                {
                    var txt = matrix[i, j] < 10 ? matrix[i, j].ToString() + "  " : matrix[i, j].ToString();
                    if (matrix[i, j] > 10 && matrix[i, j] < 100)
                    {
                        txt += " ";
                    }
                    Console.Write(txt);
                }
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
