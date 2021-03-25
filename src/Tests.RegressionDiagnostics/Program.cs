using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Tests.RegressionDiagnostics
{
    public static class Program
    {
        public static void Main()
        {
            const string folderPath = "../../../../Tests.Regression";
            const string resultsPath = "../../../../Tests.Regression/BenchmarkDotNet.Artifacts/results";
            CreateBenchmarkDat(folderPath);
            var data = ReadResults(resultsPath);

            foreach (var item in data)
            {
                Console.WriteLine($"{item.Method} - {item.Mean}");
            }
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

        private static void CreateBenchmarkDat(string folderPath)
        {
            var extension = "";

            if (OperatingSystem.IsWindows())
            {
                extension = "cmd";
            }
            else if (OperatingSystem.IsLinux())
            {
                extension = "sh";
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            var cmdPath = Path.Combine(folderPath, $"run.{extension}");
            var fileInfo = new FileInfo(cmdPath);

            if (!fileInfo.Exists)
                throw new Exception($"Script file '{cmdPath}' not found");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = fileInfo.FullName,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(cmdPath)
                }
            };

            process.Start();

            string standard_output;
            while ((standard_output = process.StandardOutput.ReadLine()) != null)
            {
                Console.WriteLine(standard_output);
            }

            process.WaitForExit();
        }
    }
}
