using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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

        private static void IncrementVersions(string path)
        {
            var doc = XDocument.Load(path);

            var foundElements = doc
                                .Descendants()
                                .Where(x => x.Name == "FileVersion" || x.Name == "AssemblyVersion")
                                .ToList();

            foreach (var entry in foundElements)
            {
                var versions = entry.Value.Split(".").Select(x => Convert.ToInt32(x)).ToList();
                entry.Value = $"{versions[0]}.{versions[1]}.{versions[2]}.{versions[3] + 1}";
            }

            var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
            using (var xw = XmlWriter.Create(path, xws))
            {
                doc.Save(xw);
            }
        }
    }
}
