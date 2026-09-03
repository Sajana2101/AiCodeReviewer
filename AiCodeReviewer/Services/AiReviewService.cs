using AiCodeReviewer.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace AiCodeReviewer.Services
{
    public class AiReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public AiReviewService(
            string model,
            int timeoutMinutes)
        {
            _model = model;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:11434"),
                Timeout = TimeSpan.FromMinutes(timeoutMinutes)
            };
        }

        public async Task<CodeReviewResult> ReviewAsync(CodeFile codeFile)
        {
            string prompt = $$"""
                You are a senior software engineer performing a professional code review.

                Review the following source code.

                File:
                {{codeFile.FilePath}}

                Language / Extension:
                {{codeFile.Extension}}

                Identify:
                - bugs
                - security vulnerabilities
                - performance problems
                - poor coding practices
                - maintainability problems
                - possible improvements

                Return ONLY valid JSON.

                Do not include markdown.
                Do not include ```json.
                Do not include any text before or after the JSON.

                Use exactly this structure:

                {
                  "score": 5.5,
                  "summary": "Short overall review summary",
                  "issues": [
                    {
                      "severity": "High",
                      "title": "Short issue title",
                      "explanation": "Explain the problem",
                      "recommendedFix": "Explain how to fix it"
                    }
                  ]
                }

                Severity must be one of:
                Critical
                High
                Medium
                Low
                Suggestion

                Score must be between 0 and 10.

                Source code:

                {{codeFile.Content}}
                """;

            var request = new
            {
                model = _model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                stream = false,
                format = "json"
            };

            HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync("/api/chat", request);

            response.EnsureSuccessStatusCode();

            string responseJson =
                await response.Content.ReadAsStringAsync();

            using JsonDocument responseDocument =
                JsonDocument.Parse(responseJson);

            string aiContent =
                responseDocument.RootElement
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()
                ?? throw new InvalidOperationException(
                    "Ollama returned an empty response.");

            using JsonDocument reviewDocument =
                JsonDocument.Parse(aiContent);

            JsonElement root =
                reviewDocument.RootElement;

            CodeReviewResult review = new()
            {
                Score = root.TryGetProperty(
                    "score",
                    out JsonElement scoreElement)
                    && scoreElement.TryGetDouble(
                        out double score)
                        ? score
                        : 0,

                Summary = root.TryGetProperty(
                    "summary",
                    out JsonElement summaryElement)
                        ? summaryElement.GetString()
                            ?? "No summary provided."
                        : "No summary provided."
            };

            if (root.TryGetProperty(
                    "issues",
                    out JsonElement issuesElement)
                && issuesElement.ValueKind
                    == JsonValueKind.Array)
            {
                foreach (
                    JsonElement issueElement
                    in issuesElement.EnumerateArray())
                {
                    if (issueElement.ValueKind
                        != JsonValueKind.Object)
                    {
                        continue;
                    }

                    ReviewIssue issue = new()
                    {
                        Severity = GetString(
                            issueElement,
                            "severity",
                            "Suggestion"),

                        Title = GetString(
                            issueElement,
                            "title",
                            "Untitled issue"),

                        Explanation = GetString(
                            issueElement,
                            "explanation",
                            "No explanation provided."),

                        RecommendedFix = GetString(
                            issueElement,
                            "recommendedFix",
                            "No recommended fix provided.")
                    };

                    review.Issues.Add(issue);
                }
            }

            return review;
        }

        private static string GetString(
            JsonElement element,
            string propertyName,
            string defaultValue)
        {
            if (!element.TryGetProperty(
                    propertyName,
                    out JsonElement property))
            {
                return defaultValue;
            }

            return property.ValueKind
                == JsonValueKind.String
                    ? property.GetString()
                        ?? defaultValue
                    : property.ToString();
        }
    }
}