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
                    Console.WriteLine($"Ошибка: входной файл {inputFile} не найден!");
                    return;
                }

                try
                {
                    var lines = File.ReadAllLines(inputFile);

                    if (lines.Length == 0)
                    {
                        Console.WriteLine("Ошибка: входной файл пуст!");
                        return;
                    }

                    int containerCount;
                    if (!int.TryParse(lines[0], out containerCount))
                    {
                        Console.WriteLine("Ошибка: первая строка должна содержать количество контейнеров!");
                        return;
                    }

                    int lineIndex = 1;


                    for (int i = 0; i < containerCount; i++)
                    {
                        if (lineIndex >= lines.Length)
                        {
                            Console.WriteLine("Ошибка: недостаточно строк в файле для контейнеров!");
                            return;
                        }

                        var containerData = lines[lineIndex].Split();
                        lineIndex++;

                        if (containerData.Length != 3)
                        {
                            Console.WriteLine($"Ошибка: строка {lineIndex} не содержит 3 значения (ID MaxWeight MaxVolume)!");
                            return;
                        }

                        containers.Add(new Container
                        {
                            Id = int.Parse(containerData[0]),
                            MaxWeight = double.Parse(containerData[1]),
                            MaxVolume = double.Parse(containerData[2])
                        });
                    }


                    if (lineIndex >= lines.Length || !int.TryParse(lines[lineIndex], out int cargoCount))
                    {
                        Console.WriteLine("Ошибка: не указано количество грузов!");
                        return;
                    }
                    lineIndex++;


                    for (int i = 0; i < cargoCount; i++)
                    {
                        if (lineIndex >= lines.Length)
                        {
                            Console.WriteLine("Ошибка: недостаточно строк в файле для грузов!");
                            return;
                        }

                        var cargoData = lines[lineIndex].Split();
                        lineIndex++;

                        if (cargoData.Length != 3)
                        {
                            Console.WriteLine($"Ошибка: строка {lineIndex} не содержит 3 значения (Name Weight Volume)!");
                            return;
                        }

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
                    Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                    return;
                }


                KnapsackSolver.Solve(cargos, containers);


                try
                {
                    using (StreamWriter writer = new StreamWriter(outputFile))
                    {
                        foreach (var cargo in cargos)
                        {
                            writer.WriteLine($"{cargo.Name} {cargo.Weight} {cargo.Volume} {cargo.ContainerId}");
                        }
                    }
                    Console.WriteLine("Output file generated: " + outputFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
                }
            }
            else
            {

                Console.WriteLine("Введите количество контейнеров:");
                int containerCount = int.Parse(Console.ReadLine());

                for (int i = 0; i < containerCount; i++)
                {
                    Console.WriteLine($"Введите контейнер {i + 1} (ID MaxWeight MaxVolume):");
                    var containerData = Console.ReadLine().Split();

                    if (containerData.Length != 3)
                    {
                        Console.WriteLine("Ошибка: контейнер должен содержать 3 параметра!");
                        return;
                    }

                    containers.Add(new Container
                    {
                        Id = int.Parse(containerData[0]),
                        MaxWeight = double.Parse(containerData[1]),
                        MaxVolume = double.Parse(containerData[2])
                    });
                }

                Console.WriteLine("Введите количество грузов:");
                int cargoCount = int.Parse(Console.ReadLine());

                for (int i = 0; i < cargoCount; i++)
                {
                    Console.WriteLine($"Введите груз {i + 1} (Name Weight Volume):");
                    var cargoData = Console.ReadLine().Split();

                    if (cargoData.Length != 3)
                    {
                        Console.WriteLine("Ошибка: груз должен содержать 3 параметра!");
                        return;
                    }

                    cargos.Add(new Cargo
                    {
                        Name = cargoData[0],
                        Weight = double.Parse(cargoData[1]),
                        Volume = double.Parse(cargoData[2])
                    });
                }

                KnapsackSolver.Solve(cargos, containers);

                Console.WriteLine("Результат:");
                foreach (var cargo in cargos)
                {
                    Console.WriteLine($"{cargo.Name} {cargo.Weight} {cargo.Volume} {cargo.ContainerId}");
                }
            }
        }
    }
}
