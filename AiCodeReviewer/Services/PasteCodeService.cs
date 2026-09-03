using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCodeReviewer.Models;
using System.Text;

namespace AiCodeReviewer.Services
{
    public class PasteCodeService
    {
        public CodeFile ReadFromConsole()
        {
            Console.WriteLine();
            Console.WriteLine("Paste your code below.");
            Console.WriteLine("Type END on a new line when finished.");
            Console.WriteLine();

            StringBuilder code = new();

            while (true)
            {
                string? line = Console.ReadLine();

                if (line == null ||
                    line.Equals(
                        "END",
                        StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                code.AppendLine(line);
            }

            if (string.IsNullOrWhiteSpace(code.ToString()))
            {
                throw new InvalidOperationException(
                    "No code was entered.");
            }

            return new CodeFile
            {
                FilePath = "PastedCode",
                Extension = ".txt",
                Content = code.ToString()
            };
        }
    }
}