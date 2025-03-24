using System;
using System.Collections.Generic;
using System.IO;

namespace WarehouseOptimization
{
    public class Cargo
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Volume { get; set; }
        public int ContainerId { get; set; } = -1;
    }

    public class Container
    {
        public int Id { get; set; }
        public double MaxWeight { get; set; }
        public double MaxVolume { get; set; }
    }

    public class KnapsackSolver
    {
        public static void Solve(List<Cargo> cargos, List<Container> containers)
        {
            foreach (var container in containers)
            {
                double remainingWeight = container.MaxWeight;
                double remainingVolume = container.MaxVolume;

                foreach (var cargo in cargos)
                {
                    if (cargo.ContainerId == -1 && cargo.Weight <= remainingWeight && cargo.Volume <= remainingVolume)
                    {
                        cargo.ContainerId = container.Id;
                        remainingWeight -= cargo.Weight;
                        remainingVolume -= cargo.Volume;
                    }
                }
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Cargo> cargos = new List<Cargo>();
            List<Container> containers = new List<Container>();

            if (args.Length > 0 && args[0] == "--file")
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Usage: WarehouseOptimization --file <inputFilePath> <outputFilePath>");
                    return;
                }

                string inputFile = args[1];
                string outputFile = args[2];

                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file {inputFile} not found!");
                    return;
                }

                try
                {
                    var lines = File.ReadAllLines(inputFile);
                    int containerCount = int.Parse(lines[0]);

                    for (int i = 1; i <= containerCount; i++)
                    {
                        var containerData = lines[i].Split();
                        containers.Add(new Container
                        {
                            Id = int.Parse(containerData[0]),
                            MaxWeight = double.Parse(containerData[1]),
                            MaxVolume = double.Parse(containerData[2])
                        });
                    }

                    for (int i = containerCount + 1; i < lines.Length; i++)
                    {
                        var cargoData = lines[i].Split();
                        cargos.Add(new Cargo
                        {
                            Name = cargoData[0],
                            Weight = double.Parse(cargoData[1]),
                            Volume = double.Parse(cargoData[2])
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading input file: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Enter number of containers:");
                int containerCount = int.Parse(Console.ReadLine());

                for (int i = 0; i < containerCount; i++)
                {
                    Console.WriteLine($"Enter container {i + 1} (ID MaxWeight MaxVolume):");
                    var containerData = Console.ReadLine().Split();
                    containers.Add(new Container
                    {
                        Id = int.Parse(containerData[0]),
                        MaxWeight = double.Parse(containerData[1]),
                        MaxVolume = double.Parse(containerData[2])
                    });
                }

                Console.WriteLine("Enter number of cargos:");
                int cargoCount = int.Parse(Console.ReadLine());

                for (int i = 0; i < cargoCount; i++)
                {
                    Console.WriteLine($"Enter cargo {i + 1} (Name Weight Volume):");
                    var cargoData = Console.ReadLine().Split();
                    cargos.Add(new Cargo
                    {
                        Name = cargoData[0],
                        Weight = double.Parse(cargoData[1]),
                        Volume = double.Parse(cargoData[2])
                    });
                }
            }

            KnapsackSolver.Solve(cargos, containers);

            if (args.Length > 0 && args[0] == "--file")
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(args[2]))
                    {
                        foreach (var cargo in cargos)
                        {
                            writer.WriteLine($"{cargo.Name} {cargo.Weight} {cargo.Volume} {cargo.ContainerId}");
                        }
                    }
                    Console.WriteLine("Output file generated: " + args[2]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing output file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Result:");
                foreach (var cargo in cargos)
                {
                    Console.WriteLine($"{cargo.Name} {cargo.Weight} {cargo.Volume} {cargo.ContainerId}");
                }
            }
        }
    }
}
