using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCodeReviewer.Models;
using System.Text.Json;

namespace AiCodeReviewer.Services
{
    public class JsonOutputService
    {
        public void Print(
            string filePath,
            CodeReviewResult review)
        {
            var result = new
            {
                file = filePath,
                score = review.Score,
                summary = review.Summary,
                issues = review.Issues
            };

            string json =
                JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            Console.WriteLine(json);
        }
    }
}