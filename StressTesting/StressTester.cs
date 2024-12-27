using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualBasic.Devices;
using MugPlugin;

namespace StressTesting
{
    public class StressTester
    {
        static void Main(string[] args)
        {
            var stressTester = new StressTester();
            stressTester.StressTesting();
        }

        public void StressTesting()
        {
            var builder = new Builder();
            var stopWatch = new Stopwatch();
            var parameters = new Parameters();

            // Set new parameters for stress testing
            parameters.SetParameter(ParameterType.BodyWidth, 100, 100, 150);
            parameters.SetParameter(ParameterType.BaseWidth, 70, 70, 100);
            parameters.SetParameter(ParameterType.BodyRadius1, 300, 300, 350);
            parameters.SetParameter(ParameterType.HandleRadius3, 10, 10, 20);
            parameters.SetParameter(ParameterType.HandleRadius5, 75, 75, 85);
            parameters.SetParameter(ParameterType.BodyLength, 100, 100, 150);

            const double gigabyteInByte = 0.000000000931322574615478515625;
            int maxIterations = 200;
            int count = 0;

            var computerInfo = new ComputerInfo();

            using (var streamWriter = new StreamWriter("log.txt"))
            {
                Console.WriteLine("Начало нагрузочного тестирования...");

                while (count < maxIterations)
                {
                    try
                    {
                        stopWatch.Start();
                        builder.Build(parameters);
                        stopWatch.Stop();

                        var usedMemory = (computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory) * gigabyteInByte;

                        streamWriter.WriteLine($"{++count}\t{stopWatch.Elapsed:hh\\:mm\\:ss\\.fff}\t{usedMemory:F3} GB");
                        streamWriter.Flush();

                        Console.WriteLine($"Итерация {count}: Время: {stopWatch.Elapsed:hh\\:mm\\:ss\\.fff}, Память: {usedMemory:F3} GB");
                    }
                    catch (Exception ex)
                    {
                        streamWriter.WriteLine($"Ошибка на итерации {count}: {ex.Message}");
                        Console.WriteLine($"Ошибка на итерации {count}: {ex.Message}");
                    }
                    finally
                    {
                        stopWatch.Reset();
                    }
                }
            }

            Console.WriteLine("Нагрузочное тестирование завершено.");
        }
    }
}
