using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCodeReviewer.Models;

namespace AiCodeReviewer.Services
{
    public class FileScannerService
    {
        private readonly string[] _supportedExtensions =
        {
            ".cs",
            ".js",
            ".ts",
            ".py",
            ".java"
        };

        private readonly string[] _ignoredFolders =
        {
            "bin",
            "obj",
            ".git",
            "node_modules"
        };

        public async Task<List<CodeFile>> ScanAsync(string path)
        {
            if (File.Exists(path))
            {
                return await ScanSingleFileAsync(path);
            }

            if (Directory.Exists(path))
            {
                return await ScanDirectoryAsync(path);
            }

            throw new FileNotFoundException("The file or directory could not be found.");
        }

        private async Task<List<CodeFile>> ScanSingleFileAsync(string path)
        {
            string extension = Path.GetExtension(path);

            if (!_supportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Unsupported file type: {extension}");
            }

            string content = await File.ReadAllTextAsync(path);

            return new List<CodeFile>
            {
                new CodeFile
                {
                    FilePath = path,
                    Content = content,
                    Extension = extension
                }
            };
        }

        private async Task<List<CodeFile>> ScanDirectoryAsync(string path)
        {
            List<CodeFile> codeFiles = new();

            IEnumerable<string> files = Directory
                .EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(file =>
                    _supportedExtensions.Contains(
                        Path.GetExtension(file),
                        StringComparer.OrdinalIgnoreCase))
                .Where(file => !IsIgnored(file));

            foreach (string file in files)
            {
                string content = await File.ReadAllTextAsync(file);

                codeFiles.Add(new CodeFile
                {
                    FilePath = file,
                    Content = content,
                    Extension = Path.GetExtension(file)
                });
            }

            return codeFiles;
        }

        private bool IsIgnored(string filePath)
        {
            string[] parts = filePath.Split(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar);

            return parts.Any(part =>
                _ignoredFolders.Contains(
                    part,
                    StringComparer.OrdinalIgnoreCase));
        }
    }
}