using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCodeReviewer.Models;

namespace AiCodeReviewer.Services
{
    public class ConsoleOutputService
    {
        public void PrintReview(string filePath, CodeReviewResult review)
        {
            Console.WriteLine();
            Console.WriteLine($"Reviewing: {filePath}");
            Console.WriteLine();

            Console.WriteLine($"Score: {review.Score}/10");
            Console.WriteLine();

            Console.WriteLine("Summary:");
            Console.WriteLine(review.Summary);
            Console.WriteLine();

            List<ReviewIssue> sortedIssues = review.Issues
                .OrderBy(issue => GetSeverityOrder(issue.Severity))
                .ToList();

            Console.WriteLine($"Issues found: {sortedIssues.Count}");
            Console.WriteLine();

            foreach (ReviewIssue issue in sortedIssues)
            {
                SetSeverityColor(issue.Severity);

                Console.WriteLine(
                    $"[{issue.Severity.ToUpper()}] {issue.Title}");

                Console.ResetColor();

                Console.WriteLine(
                    $"Problem: {issue.Explanation}");

                Console.WriteLine(
                    $"Fix: {issue.RecommendedFix}");

                Console.WriteLine();
            }

            PrintIssueSummary(sortedIssues);

            Console.WriteLine();
            Console.WriteLine(
                "--------------------------------------------------");
        }

        private int GetSeverityOrder(string severity)
        {
            return severity.ToLower() switch
            {
                "critical" => 1,
                "high" => 2,
                "medium" => 3,
                "low" => 4,
                "suggestion" => 5,
                _ => 6
            };
        }

        private void SetSeverityColor(string severity)
        {
            Console.ForegroundColor =
                severity.ToLower() switch
                {
                    "critical" => ConsoleColor.Red,
                    "high" => ConsoleColor.DarkYellow,
                    "medium" => ConsoleColor.Yellow,
                    "low" => ConsoleColor.Cyan,
                    "suggestion" => ConsoleColor.Blue,
                    _ => ConsoleColor.Gray
                };
        }

        private void PrintIssueSummary(List<ReviewIssue> issues)
        {
            int critical = issues.Count(i =>
                i.Severity.Equals(
                    "Critical",
                    StringComparison.OrdinalIgnoreCase));

            int high = issues.Count(i =>
                i.Severity.Equals(
                    "High",
                    StringComparison.OrdinalIgnoreCase));

            int medium = issues.Count(i =>
                i.Severity.Equals(
                    "Medium",
                    StringComparison.OrdinalIgnoreCase));

            int low = issues.Count(i =>
                i.Severity.Equals(
                    "Low",
                    StringComparison.OrdinalIgnoreCase));

            int suggestions = issues.Count(i =>
                i.Severity.Equals(
                    "Suggestion",
                    StringComparison.OrdinalIgnoreCase));

            Console.WriteLine("Issue Summary:");
            Console.WriteLine($"Critical: {critical}");
            Console.WriteLine($"High: {high}");
            Console.WriteLine($"Medium: {medium}");
            Console.WriteLine($"Low: {low}");
            Console.WriteLine($"Suggestions: {suggestions}");
        }
    }
}
