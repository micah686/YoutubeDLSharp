using System.Text.RegularExpressions;
using YtdlpPocoGenerator.Generation;
using YtdlpPocoGenerator.Parsing;

const string CommonPyUrl = "https://raw.githubusercontent.com/yt-dlp/yt-dlp/master/yt_dlp/extractor/common.py";
const string ReadmeUrl = "https://raw.githubusercontent.com/yt-dlp/yt-dlp/master/README.md";
const string DefaultPocoOutputDir = "YtdlpPoco";
const string DefaultOptionSetOutputDir = "../YoutubeDLSharp/Options";

var pocoOutputDir = args.Length > 0 ? args[0] : DefaultPocoOutputDir;
pocoOutputDir = Path.GetFullPath(pocoOutputDir);
var optionSetOutputDir = args.Length > 1 ? args[1] : DefaultOptionSetOutputDir;
optionSetOutputDir = Path.GetFullPath(optionSetOutputDir);

Console.WriteLine("YtdlpPocoGenerator");
Console.WriteLine("==================");
Console.WriteLine($"Fetching: {CommonPyUrl}");
Console.WriteLine($"Fetching: {ReadmeUrl}");

string source;
string readme;
using (var client = new HttpClient())
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("YtdlpPocoGenerator/1.0");
    var sourceTask = client.GetStringAsync(CommonPyUrl);
    var readmeTask = client.GetStringAsync(ReadmeUrl);
    source = await sourceTask;
    readme = await readmeTask;
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

Directory.CreateDirectory(pocoOutputDir);

// Remove stale generated files that no longer correspond to any parsed class.
foreach (var existing in Directory.GetFiles(pocoOutputDir, "*.cs"))
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
    var path = Path.Combine(pocoOutputDir, fileName);
    await File.WriteAllTextAsync(path, content);
    Console.WriteLine($"  Wrote: {path}");
}

Console.WriteLine();
Console.WriteLine($"Done. Generated {files.Count} POCO files in: {pocoOutputDir}");

// ── OptionSet generation (ported from autoconvert.py) ──────────────────
Console.WriteLine();
Console.WriteLine("OptionSet Generation");
Console.WriteLine("====================");

// Extract the USAGE AND OPTIONS section from the README.
// Everything between "# USAGE AND OPTIONS" and "# CONFIGURATION"
var helpTextMatch = Regex.Match(readme,
    @"# USAGE AND OPTIONS\s*\n(.*?)(?=\n# CONFIGURATION\b)",
    RegexOptions.Singleline);

string helpText = helpTextMatch.Success ? helpTextMatch.Value : string.Empty;

if (string.IsNullOrWhiteSpace(helpText))
{
    Console.WriteLine("WARNING: Could not extract help text from README. Skipping OptionSet generation.");
}
else
{
    Console.WriteLine($"Extracted help text: {helpText.Length:N0} characters. Parsing...");

    var optionSections = HelpTextParser.Parse(helpText);

    // Extract deprecated options from the README
    // Only grab the "Redundant options" and "Not recommended" subsections to match
    // the original autoconvert.py behavior.
    List<YtdlpPocoGenerator.Models.OptionDefinition>? deprecatedOptions = null;
    var redundantMatch = Regex.Match(readme,
        @"#### Redundant options\s*\n(.*?)(?=\n####\s|\n###\s|\n##\s|\n#\s|$)",
        RegexOptions.Singleline);
    var notRecommendedMatch = Regex.Match(readme,
        @"#### Not recommended\s*\n(.*?)(?=\n####\s|\n###\s|\n##\s|\n#\s|$)",
        RegexOptions.Singleline);
    var deprecatedText = "";
    if (redundantMatch.Success)
        deprecatedText += redundantMatch.Groups[1].Value + "\n";
    if (notRecommendedMatch.Success)
        deprecatedText += notRecommendedMatch.Groups[1].Value + "\n";
    if (!string.IsNullOrWhiteSpace(deprecatedText))
    {
        deprecatedOptions = HelpTextParser.ParseDeprecatedOptions(deprecatedText);
    }

    var optionFiles = OptionSetGenerator.GenerateAll(optionSections, deprecatedOptions);

    Directory.CreateDirectory(optionSetOutputDir);

    // Only remove stale OptionSet.*.cs files (not OptionSet.cs or OptionSet.Custom.cs)
    foreach (var existing in Directory.GetFiles(optionSetOutputDir, "OptionSet.*.cs"))
    {
        var name = Path.GetFileName(existing);
        // Preserve manually-maintained files
        if (name is "OptionSet.Custom.cs")
            continue;
        if (!optionFiles.ContainsKey(name))
        {
            File.Delete(existing);
            Console.WriteLine($"  Removed stale: {existing}");
        }
    }

    foreach (var (fileName, content) in optionFiles)
    {
        var path = Path.Combine(optionSetOutputDir, fileName);
        // Write with BOM encoding to match existing files
        var encoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
        await File.WriteAllTextAsync(path, content, encoding);
        Console.WriteLine($"  Wrote: {path}");
    }

    Console.WriteLine();
    Console.WriteLine($"Done. Generated {optionFiles.Count} OptionSet files in: {optionSetOutputDir}");
}
