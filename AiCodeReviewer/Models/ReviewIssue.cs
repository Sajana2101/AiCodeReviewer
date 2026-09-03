using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCodeReviewer.Models
{
    public class ReviewIssue
    {
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public string RecommendedFix { get; set; } = string.Empty;
    }
}
