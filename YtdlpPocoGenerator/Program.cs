using YtdlpPocoGenerator.Generation;
using YtdlpPocoGenerator.Parsing;

const string CommonPyUrl = "https://raw.githubusercontent.com/yt-dlp/yt-dlp/master/yt_dlp/extractor/common.py";
const string DefaultOutputDir = "YtdlpPoco";

var outputDir = args.Length > 0 ? args[0] : DefaultOutputDir;
outputDir = Path.GetFullPath(outputDir);

Console.WriteLine("YtdlpPocoGenerator");
Console.WriteLine("==================");
Console.WriteLine($"Fetching: {CommonPyUrl}");

string source;
using (var client = new HttpClient())
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("YtdlpPocoGenerator/1.0");
    source = await client.GetStringAsync(CommonPyUrl);
}

Console.WriteLine($"Fetched {source.Length:N0} characters. Parsing...");

var fields = CommonPyParser.Parse(source);

var complexFields = fields.Where(f => f.SubFields.Count > 0 && f.Name != "entries").ToList();
Console.WriteLine($"Found {fields.Count} top-level fields, " +
                  $"{complexFields.Count} with nested sub-fields: " +
                  $"{string.Join(", ", complexFields.Select(f => f.Name))}");

var enums = YtdlpPocoGenerator.Generation.EnumDetector.Detect(fields);
Console.WriteLine($"Detected {enums.Count} enum(s): " +
                  $"{string.Join(", ", enums.Values.Select(e => $"{e.Name} ({e.Values.Count} values)"))}");


var files = PocoGenerator.GenerateAll(fields);

Directory.CreateDirectory(outputDir);

// Remove stale generated files that no longer correspond to any parsed class.
foreach (var existing in Directory.GetFiles(outputDir, "*.cs"))
{
    var name = Path.GetFileName(existing);
    if (!files.ContainsKey(name))
    {
        File.Delete(existing);
        Console.WriteLine($"  Removed stale: {existing}");
    }
}

foreach (var (fileName, content) in files)
{
    var path = Path.Combine(outputDir, fileName);
    await File.WriteAllTextAsync(path, content);
    Console.WriteLine($"  Wrote: {path}");
}

Console.WriteLine();
Console.WriteLine($"Done. Generated {files.Count} files in: {outputDir}");
