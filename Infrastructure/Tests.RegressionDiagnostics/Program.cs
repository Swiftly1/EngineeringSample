using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Running;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Tests.LexerTests;

namespace Tests.RegressionDiagnostics
{
    public class Program
    {
        public static void Main()
        {
            #if DEBUG
                Console.WriteLine(Directory.GetCurrentDirectory());
                var pathCombined = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
                Directory.SetCurrentDirectory(pathCombined);
                Console.WriteLine(Directory.GetCurrentDirectory());
            #endif

            const string resultsPath = "./BenchmarkDotNet.Artifacts/results";

            var dirInfo = new DirectoryInfo(resultsPath);

            if (dirInfo.Exists)
            {
                Console.WriteLine($"Removing folder: '{dirInfo.FullName}'");
                dirInfo.Delete(true);
            }

            BenchmarkSwitcher.FromAssembly(typeof(BasicTests).Assembly).RunAll();

            const string perfDataPath = "../PerformanceData/data.json";
            var oldJson = File.ReadAllText(perfDataPath);
            var oldData = JsonConvert.DeserializeObject<List<BenchmarkResult>>(oldJson);

            var newData = ReadResults(resultsPath);

            var version = GetVersion();

            Console.WriteLine(version);

            foreach (var entry in newData)
            {
                entry.CollectedAtVersion = version;
            }

            oldData.AddRange(newData);

            var json = JsonConvert.SerializeObject(oldData, Formatting.Indented);

            File.WriteAllText(perfDataPath, json);
        }

        private static string GetVersion()
        {
            var path = Path.Combine("..", "..", "src");

            var file = Directory
                       .GetFiles(path, "*", new EnumerationOptions { RecurseSubdirectories = true })
                       .First(x => x.EndsWith(".csproj"));

            var lines = File.ReadAllLines(file);

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("<AssemblyVersion>"))
                {
                    var version = line.Replace("<AssemblyVersion>", "").Replace("</AssemblyVersion>", "").Trim();
                    return version;
                }
            }

            throw new Exception($"Version is not defined in '{file}'");
        }

        private static List<BenchmarkResult> ReadResults(string resultsPath)
        {
            var list = new List<BenchmarkResult>();

            var dir = new DirectoryInfo(resultsPath);
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            foreach (var csvPath in dir.EnumerateFiles("*.csv"))
            {
                using (var reader = new StreamReader(csvPath.FullName))
                using (var csv = new CsvReader(reader, cfg))
                {
                    csv.Context.RegisterClassMap<ModelClassMap>();
                    list.AddRange(csv.GetRecords<BenchmarkResult>());
                }
            }

            return list;
        }
    }
}
