using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCodeReviewer.Models
{
    public class AppConfig
    {
        public string Model { get; set; } = "qwen2.5-coder:7b";
        public int TimeoutMinutes { get; set; } = 5;
    }
}
