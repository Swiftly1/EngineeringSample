using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
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

            var reMapped = oldData
            .Select(x => new
            {
                x.Method,
                x.EnvironmentVariables,
                x.Jit,
                x.Platform,
                x.Runtime,
                x.AllowVeryLargeObjects,
                x.Concurrent,
                x.CpuGroups,
                x.Force,
                x.Server,
                x.Mean,
                x.Error,
                x.StdDev,
                x.CollectedAt,
                x.CollectedAtVersion
            })
            .ToList();

            var json = JsonConvert.SerializeObject(reMapped, Formatting.Indented);

            File.WriteAllText(perfDataPath, json);
        }

        private static string GetVersion()
        {
            var path = Path.Combine("..", "..", "src");

            var file = Directory
                       .GetFiles(path, "*", new EnumerationOptions { RecurseSubdirectories = true })
                       .First(x => x.EndsWith(".csproj"));

            var doc = XDocument.Load(file);

            var foundElements = doc
                                .Descendants()
                                .Where(x => x.Name == "FileVersion" || x.Name == "AssemblyVersion")
                                .ToList();

            foreach (var entry in foundElements)
            {
                return entry.Value;
            }

            throw new Exception($"Version is not defined in '{file}'");
        }

        private static List<BenchmarkResult> ReadResults(string resultsPath)
        {
            var list = new List<BenchmarkResult>();

            var dir = new DirectoryInfo(resultsPath);

            // this is terrible 
            // TODO: learn how to use CultureInfo and fix this mess 

            var delimiter = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "," : ";";

            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
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
