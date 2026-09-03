using AiCodeReviewer.Models;
using AiCodeReviewer.Services;
using System.CommandLine;

RootCommand rootCommand =
    new("AI-powered code review CLI");

Command reviewCommand =
    new("review", "Review source code using AI");

Argument<string> pathArgument =
    new("path")
    {
        Description = "File or directory to review",
        Arity = ArgumentArity.ZeroOrOne
    };

Option<bool> pasteOption =
    new("--paste")
    {
        Description = "Paste code directly into the terminal"
    };

Option<bool> jsonOption =
    new("--json")
    {
        Description = "Output the review as JSON"
    };

Option<string?> modelOption =
    new("--model")
    {
        Description = "Override the Ollama model"
    };

reviewCommand.Arguments.Add(pathArgument);
reviewCommand.Options.Add(pasteOption);
reviewCommand.Options.Add(jsonOption);
reviewCommand.Options.Add(modelOption);

reviewCommand.SetAction(async parseResult =>
{
    string? path =
        parseResult.GetValue(pathArgument);

    bool paste =
        parseResult.GetValue(pasteOption);

    bool json =
        parseResult.GetValue(jsonOption);

    string? modelOverride =
        parseResult.GetValue(modelOption);

    try
    {
        ConfigService configService = new();

        AppConfig config =
            configService.Load();

        string model =
            string.IsNullOrWhiteSpace(modelOverride)
                ? config.Model
                : modelOverride;

        AiReviewService aiReviewer =
            new(
                model,
                config.TimeoutMinutes);

        ConsoleOutputService consoleOutput =
            new();

        JsonOutputService jsonOutput =
            new();

        if (!json)
        {
            Console.WriteLine();
            Console.WriteLine("AI Code Reviewer");
            Console.WriteLine("================");
            Console.WriteLine($"Model: {model}");
        }

        if (paste)
        {
            PasteCodeService pasteService = new();

            CodeFile pastedCode =
                pasteService.ReadFromConsole();

            CodeReviewResult review =
                await aiReviewer.ReviewAsync(pastedCode);

            if (json)
            {
                jsonOutput.Print(
                    pastedCode.FilePath,
                    review);
            }
            else
            {
                consoleOutput.PrintReview(
                    pastedCode.FilePath,
                    review);
            }

            return;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine(
                "Please provide a file, folder, or use --paste.");

            return;
        }

        FileScannerService scanner = new();

        List<CodeFile> files =
            await scanner.ScanAsync(path);

        if (files.Count == 0)
        {
            Console.WriteLine(
                "No supported source code files were found.");

            return;
        }

        if (!json)
        {
            Console.WriteLine($"Files found: {files.Count}");
        }

        foreach (CodeFile file in files)
        {
            CodeReviewResult review =
                await aiReviewer.ReviewAsync(file);

            if (json)
            {
                jsonOutput.Print(
                    file.FilePath,
                    review);
            }
            else
            {
                consoleOutput.PrintReview(
                    file.FilePath,
                    review);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Error: {ex.Message}");
    }
});

rootCommand.Subcommands.Add(reviewCommand);

return rootCommand.Parse(args).Invoke();