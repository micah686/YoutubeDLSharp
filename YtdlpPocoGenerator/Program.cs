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

(var videoFields, var formatFields) = CommonPyParser.Parse(source);

Console.WriteLine($"Found {videoFields.Count} video fields, {formatFields.Count} format fields.");

var files = PocoGenerator.GenerateAll(videoFields, formatFields);

Directory.CreateDirectory(outputDir);

foreach (var (fileName, content) in files)
{
    var path = Path.Combine(outputDir, fileName);
    await File.WriteAllTextAsync(path, content);
    Console.WriteLine($"  Wrote: {path}");
}

Console.WriteLine();
Console.WriteLine($"Done. Generated {files.Count} files in: {outputDir}");
