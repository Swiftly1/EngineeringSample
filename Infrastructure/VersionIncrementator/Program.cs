using System;
using System.IO;
using System.Linq;
using System.Text;

namespace VersionIncrementator
{
    internal static class Program
    {
        public static void Main()
        {
            #if DEBUG
                Console.WriteLine(Directory.GetCurrentDirectory());
                var pathCombined = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
                Directory.SetCurrentDirectory(pathCombined);
                Console.WriteLine(Directory.GetCurrentDirectory());
            #endif

            var path = Path.Combine("..", "..", "src");

            var files = Directory
                .GetFiles(path, "*", new EnumerationOptions { RecurseSubdirectories = true })
                .Where(x => x.EndsWith(".csproj"))
                .ToList();

            foreach (var file in files)
            {
                Console.WriteLine($"Trying to change version in file: '{file}'.");
                IncrementVersions(file);
            }
        }

        private static void IncrementVersions(string file)
        {
            var txt = File.ReadAllLines(file);
            var sb = new StringBuilder();

            foreach (var line in txt)
            {
                // this is terrible, but I've been struggling with csproj's XML class that'd be future proof
                // it works and is relatively simple, I guess?
                if (line.Trim().StartsWith("<AssemblyVersion>"))
                {
                    var version = line.Replace("<AssemblyVersion>", "").Replace("</AssemblyVersion>", "").Trim();
                    var versions = version.Split(".").Select(x => Convert.ToInt32(x)).ToList();

                    sb
                        .Append("    <AssemblyVersion>")
                        .Append(versions[0]).Append('.').Append(versions[1]).Append('.').Append(versions[2]).Append('.').Append(versions[3] + 1)
                        .AppendLine("</AssemblyVersion>");
                }
                else if (line.Trim().StartsWith("<FileVersion>"))
                {
                    var version = line.Replace("<FileVersion>", "").Replace("</FileVersion>", "").Trim();
                    var versions = version.Split(".").Select(x => Convert.ToInt32(x)).ToList();
                    sb
                        .Append("    <FileVersion>")
                        .Append(versions[0]).Append('.').Append(versions[1]).Append('.').Append(versions[2]).Append('.').Append(versions[3] + 1)
                        .AppendLine("</FileVersion>");
                }
                else
                {
                    sb.AppendLine(line);
                }
            }

            File.WriteAllText(file, sb.ToString());
        }
    }
}
