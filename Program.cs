using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CsvHelper;
using System.Globalization;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("JSON Analyzer Tool");
        Console.WriteLine("------------------");

        while (true)
        {
            Console.Write("Enter the path to the JSON file (or 'exit' to quit): ");
            string filePath = Console.ReadLine();

            if (filePath.ToLower() == "exit")
                break;

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found. Please try again.");
                continue;
            }

            string jsonContent = File.ReadAllText(filePath);
            AnalyzeJson(jsonContent);

            Console.WriteLine("\nDo you want to convert the JSON to another format? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
            {
                ConvertJson(jsonContent);
            }
        }
    }

    static void AnalyzeJson(string jsonContent)
    {
        try
        {
            JToken parsedJson = JToken.Parse(jsonContent);
            Console.WriteLine("JSON is valid.");

            var stats = AnalyzeJsonContent(parsedJson);
            Console.WriteLine($"Unique keys: {stats.UniqueKeys}");
            Console.WriteLine($"Arrays: {stats.Arrays}");
            Console.WriteLine($"Nested objects: {stats.NestedObjects}");
            Console.WriteLine($"Primitive values: {stats.PrimitiveValues}");
        }
        catch (JsonReaderException ex)
        {
            Console.WriteLine($"JSON is invalid. Error: {ex.Message}");
        }
    }

    static (int UniqueKeys, int Arrays, int NestedObjects, int PrimitiveValues) AnalyzeJsonContent(JToken token)
    {
        int uniqueKeys = 0;
        int arrays = 0;
        int nestedObjects = 0;
        int primitiveValues = 0;

        if (token is JObject obj)
        {
            uniqueKeys += obj.Properties().Count();
            foreach (var property in obj.Properties())
            {
                var childStats = AnalyzeJsonContent(property.Value);
                arrays += childStats.Arrays;
                nestedObjects += childStats.NestedObjects;
                primitiveValues += childStats.PrimitiveValues;
            }
            nestedObjects++;
        }
        else if (token is JArray arr)
        {
            arrays++;
            foreach (var item in arr)
            {
                var childStats = AnalyzeJsonContent(item);
                uniqueKeys += childStats.UniqueKeys;
                arrays += childStats.Arrays;
                nestedObjects += childStats.NestedObjects;
                primitiveValues += childStats.PrimitiveValues;
            }
        }
        else
        {
            primitiveValues++;
        }

        return (uniqueKeys, arrays, nestedObjects, primitiveValues);
    }

    static void ConvertJson(string jsonContent)
    {
        Console.WriteLine("Select output format:");
        Console.WriteLine("1. CSV");
        Console.WriteLine("2. XML");
        Console.Write("Enter your choice (1 or 2): ");

        string choice = Console.ReadLine();
        string outputPath = "";

        switch (choice)
        {
            case "1":
                outputPath = "output.csv";
                ConvertToCsv(jsonContent, outputPath);
                break;
            case "2":
                outputPath = "output.xml";
                ConvertToXml(jsonContent, outputPath);
                break;
            default:
                Console.WriteLine("Invalid choice. Conversion cancelled.");
                return;
        }

        Console.WriteLine($"Conversion complete. Output saved to {outputPath}");
    }

    static void ConvertToCsv(string jsonContent, string outputPath)
    {
        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonContent);
        using (var writer = new StreamWriter(outputPath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords((IEnumerable<dynamic>)jsonObject);
        }
    }

    static void ConvertToXml(string jsonContent, string outputPath)
    {
        var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonContent);
        var xml = JsonConvert.DeserializeXmlNode(jsonContent, "root");
        xml.Save(outputPath);
    }
}
