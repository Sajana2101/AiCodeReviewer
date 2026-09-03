using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCodeReviewer.Models
{
    public class CodeReviewResult
    {
        public double Score { get; set; }
        public string Summary { get; set; } = string.Empty;
        public List<ReviewIssue> Issues { get; set; } = new();
    }
}