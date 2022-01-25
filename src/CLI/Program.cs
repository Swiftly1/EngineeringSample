using System.IO;
using System.Text.Json;

namespace CLI;

internal static class Program
{
    private static void Main(string[] args)
    {
        var settings = JsonSerializer.Deserialize<RunnerSettings>(File.ReadAllText("appsettings.json"));
        new Runner(settings).Main(args);
    }
}