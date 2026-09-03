# AI Code Reviewer CLI

A local AI-powered command-line tool that reviews source code for bugs, security vulnerabilities, performance problems, maintainability concerns, and general code-quality issues.

The application is built with **C# and .NET 9** and uses **Ollama** to run an AI coding model locally. This means source code can be reviewed without sending it to an external cloud AI API.

## Features

* AI-powered source-code review
* Runs AI locally through Ollama
* Review a single source-code file
* Review an entire folder
* Paste code directly into the terminal
* Structured AI review results
* Code-quality score out of 10
* Severity levels:

  * Critical
  * High
  * Medium
  * Low
  * Suggestion
* Detects:

  * Bugs
  * Security vulnerabilities
  * Performance problems
  * Poor coding practices
  * Maintainability problems
  * Possible improvements
* Coloured terminal output
* Issues automatically sorted by severity
* Issue summary statistics
* JSON output mode
* Configurable AI model
* Configurable request timeout
* Automatically ignores common non-source folders such as:

  * `bin`
  * `obj`
  * `.git`
  * `node_modules`
* Supports:

  * C#
  * JavaScript
  * TypeScript
  * Python
  * Java

## Technologies

* C#
* .NET 9
* System.CommandLine
* HttpClient
* System.Text.Json
* Ollama
* Qwen2.5-Coder 7B

## How It Works

The CLI scans the requested file or folder and loads supported source-code files.

The source code is then sent to the local Ollama API running on the developer's computer.

Ollama runs the configured coding model and returns a structured JSON review.

The application converts the AI response into C# objects and displays the results in the terminal.

```text
Source Code
     |
     v
FileScannerService
     |
     v
CodeFile
     |
     v
AiReviewService
     |
     v
Ollama Local API
     |
     v
Qwen2.5-Coder
     |
     v
Structured JSON Review
     |
     v
CodeReviewResult
     |
     v
Console / JSON Output
```

## Requirements

Before running the application, install:

* .NET 9 SDK
* Ollama
* Qwen2.5-Coder 7B

### .NET 9

Download and install the .NET 9 SDK from Microsoft:

https://dotnet.microsoft.com/download/dotnet/9.0

Confirm the installation with:

```powershell
dotnet --version
```

The application targets .NET 9.

## Installing Ollama

Ollama allows the AI model to run locally on your computer.

### Windows

Download Ollama from:

https://ollama.com/download/windows

Ollama requires Windows 10 or later.

Install Ollama normally using the Windows installer.

After installation, open PowerShell or Windows Terminal and check the installation:

```powershell
ollama --version
```

## Installing the AI Model

This project uses:

```text
qwen2.5-coder:7b
```

Qwen2.5-Coder is a code-focused model designed for code generation, reasoning and code fixing.

Download the model with:

```powershell
ollama run qwen2.5-coder:7b
```

The first run downloads the model to the computer.

The 7B version is approximately 4.7 GB.

Once the model starts, you can test it by entering a message.

To leave the interactive model session:

```text
/bye
```

Ollama can continue running locally in the background.

You can confirm the model is installed with:

```powershell
ollama list
```

You should see something similar to:

```text
NAME                 SIZE
qwen2.5-coder:7b     4.7 GB
```

## Installation

Clone the repository:

```powershell
git clone YOUR_REPOSITORY_URL
```

Enter the repository:

```powershell
cd AiCodeReviewer
```

Enter the project folder containing the `.csproj` file if necessary:

```powershell
cd AiCodeReviewer
```

Restore the NuGet packages:

```powershell
dotnet restore
```

Build the application:

```powershell
dotnet build
```

## Configuration

The project contains:

```text
aicr-config.json
```

Example configuration:

```json
{
  "model": "qwen2.5-coder:7b",
  "timeoutMinutes": 5
}
```

### model

Controls which locally installed Ollama model is used.

Default:

```text
qwen2.5-coder:7b
```

### timeoutMinutes

Controls how long the CLI waits for Ollama to generate a response.

The default is:

```text
5 minutes
```

Local AI inference speed depends on the user's hardware.

## Usage

The application uses the following command structure:

```text
AiCodeReviewer review <path>
```

During development, commands can be executed using:

```powershell
dotnet run -- review <path>
```

## Review a Single File

Example:

```powershell
dotnet run -- review Program.cs
```

The CLI scans the file and sends its contents to the local AI model.

Example output:

```text
AI Code Reviewer
================
Model: qwen2.5-coder:7b
Files found: 1

Reviewing: Program.cs

Score: 7.5/10

Summary:
The code is generally well structured but contains several maintainability concerns.

Issues found: 2

[HIGH] Hard-coded credential
Problem: Sensitive information is stored directly in source code.
Fix: Store secrets in secure configuration or environment variables.

[MEDIUM] Inefficient processing
Problem: A nested loop produces unnecessary repeated operations.
Fix: Consider using a more efficient lookup or processing strategy.

Issue Summary:
Critical: 0
High: 1
Medium: 1
Low: 0
Suggestions: 0
```

AI results can vary between reviews.

## Review an Entire Folder

Example:

```powershell
dotnet run -- review ./Services
```

Or review the current project:

```powershell
dotnet run -- review .
```

The scanner recursively finds supported source-code files.

The following directories are automatically ignored:

```text
bin
obj
.git
node_modules
```

## Paste Code Directly Into the Terminal

Run:

```powershell
dotnet run -- review --paste
```

The CLI displays:

```text
Paste your code below.
Type END on a new line when finished.
```

Paste the code:

```csharp
public class LoginService
{
    private string password = "admin123";

    public bool Login(string input)
    {
        return input == password;
    }
}
```

Then enter:

```text
END
```

The pasted code is sent to the local AI model for review.

## JSON Output

The application can output review results as structured JSON.

Run:

```powershell
dotnet run -- review Program.cs --json
```

Example:

```json
{
  "file": "Program.cs",
  "score": 6.5,
  "summary": "The code contains several areas that could be improved.",
  "issues": [
    {
      "severity": "High",
      "title": "Hard-coded password",
      "explanation": "A password is stored directly inside source code.",
      "recommendedFix": "Store sensitive values securely outside the application."
    }
  ]
}
```

JSON mode makes it possible for the output to be consumed by other tools or future CI/CD workflows.

## Change the AI Model

The configured model can be overridden directly from the command line.

Example:

```powershell
dotnet run -- review Program.cs --model qwen2.5-coder:7b
```

Any compatible Ollama model installed locally can be supplied.

The model must already exist on the user's machine.

Installed models can be viewed with:

```powershell
ollama list
```

## Project Structure

```text
AiCodeReviewer
|
|-- Models
|   |-- AppConfig.cs
|   |-- CodeFile.cs
|   |-- CodeReviewResult.cs
|   `-- ReviewIssue.cs
|
|-- Services
|   |-- AiReviewService.cs
|   |-- ConfigService.cs
|   |-- ConsoleOutputService.cs
|   |-- FileScannerService.cs
|   |-- JsonOutputService.cs
|   `-- PasteCodeService.cs
|
|-- Program.cs
|-- aicr-config.json
`-- AiCodeReviewer.csproj
```

## Main Components

### FileScannerService

Responsible for locating and reading supported source-code files.

It can scan individual files or recursively scan directories while excluding unnecessary build and dependency folders.

### AiReviewService

Communicates with the locally running Ollama API.

It sends source code and code-review instructions to the configured model and converts the AI-generated JSON into application objects.

### CodeReviewResult

Represents the overall AI review.

It contains:

* Code-quality score
* Summary
* Collection of detected issues

### ReviewIssue

Represents an individual problem identified by the AI.

Each issue contains:

* Severity
* Title
* Explanation
* Recommended fix

### ConsoleOutputService

Responsible for presenting review results in a readable terminal format.

It also:

* Sorts issues by severity
* Applies terminal colours
* Displays issue counts

### JsonOutputService

Outputs review results as structured JSON for programmatic use.

### PasteCodeService

Allows source code to be pasted directly into the terminal without first creating a source file.

### ConfigService

Loads configuration from `aicr-config.json`.

## Local AI and Privacy

One of the main design goals of this project is local AI processing.

The application communicates with Ollama using its local HTTP API:

```text
http://localhost:11434
```

The source code is therefore processed by the locally running model rather than requiring a third-party cloud AI API.

This can be useful when reviewing code that developers do not want to send to an external AI service.

## Hardware

AI performance depends heavily on the computer running Ollama.

The default Qwen2.5-Coder 7B model is approximately 4.7 GB.

Users with lower-spec computers can install a smaller model, such as:

```powershell
ollama run qwen2.5-coder:3b
```

and update `aicr-config.json`:

```json
{
  "model": "qwen2.5-coder:3b",
  "timeoutMinutes": 5
}
```

More powerful systems can use larger models.

## Limitations

AI-generated code reviews are not guaranteed to be correct.

The tool should be used as an additional development aid and not as a replacement for:

* Human code review
* Unit testing
* Integration testing
* Static-analysis tools
* Security scanning
* Professional security audits

Local model quality and response speed also depend on the selected model and available hardware.

## Future Improvements

Possible future improvements include:

* Unit tests
* CI/CD integration
* GitHub Actions support
* Git diff review
* Pull request review
* Review only changed files
* Custom severity thresholds
* Markdown report generation
* HTML reports
* Configurable ignored folders
* Additional source-code languages
* Global `aicr` command installation
* Streaming AI responses
* Multiple AI model profiles

## Example Use Cases

The tool can be used to:

* Review a class before committing code
* Scan service-layer code for maintainability issues
* Identify basic security concerns
* Review pasted code snippets
* Generate structured review results for automation
* Experiment with local AI-assisted software development

## Why Local AI?

Many AI developer tools depend on external cloud APIs.

This project explores an alternative approach where AI inference runs locally using Ollama.

This provides:

* No per-request API charges
* Local model execution
* Greater control over the selected model
* Source code remaining on the local machine
* Offline AI review after the model has been downloaded

## Disclaimer

AI-generated suggestions may contain incorrect or incomplete information.

All recommendations should be reviewed by a developer before changes are applied.

## Author

Built as a software-development portfolio project demonstrating:

* C#/.NET development
* Command-line application design
* Local AI integration
* REST API communication
* JSON parsing
* Asynchronous programming
* File-system processing
* Configuration management
* Defensive handling of AI-generated structured output
