using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HeatSite
{
    public static class FileEnumerationExtensions
    {
        private static readonly string[] _blackList = new[]
                                                          {"obj\\", ".svn", "_svn", "resharper", "Cassini.dll"};

        private static readonly string[] _extensionBlackList = new[]
                                                                   {
                                                                       ".cs", ".svn", ".suo", ".csproj", ".resharper",
                                                                       ".user"
                                                                   };

        public static IEnumerable<string> ExcludeBlackListFiles(this IEnumerable<string> inputs)
        {
            return inputs.Where(
                name => !_blackList.Any(item => name.IndexOf(item, StringComparison.InvariantCultureIgnoreCase) >= 0))
                .Where(
                name =>
                !_extensionBlackList.Any(item => name.EndsWith(item, StringComparison.InvariantCultureIgnoreCase)))
                .Where(name => !(name.Contains("bin/") || name.EndsWith(".xml")));
        }

        public static IEnumerable<string> AllFiles(this string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }

        public static IEnumerable<string> AllDirectories(this IEnumerable<string> files)
        {
            return files.Select(file => file.DirectoryName())
                .SelectMany(directory => directory.ExpandDirectory()).Distinct();
        }

        public static IEnumerable<string> ExpandDirectory(this string path)
        {
            string parentDirectory = path.ParentDirectory();
            if (!string.IsNullOrEmpty(parentDirectory)) return new[] {path}.Concat(ExpandDirectory(parentDirectory));
            return new[] {path};
        }

        public static IEnumerable<string> ToRelative(this IEnumerable<string> files, string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                path = path.Substring(0, path.Length - 1);
            }

            foreach (string file in files)
            {
                if (file.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (file.Length > (path.Length + 1))
                        yield return file.Substring(path.Length + 1);
                    else yield return file.Substring(path.Length);
                }
                else
                {
                    yield return file;
                }
            }
        }

        public static string DirectoryName(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static string ToWixId(this string path)
        {
            var builder = new StringBuilder(path.Length);

            foreach (char c in path)
            {
                if (char.IsLetter(c) || char.IsDigit(c) || c == '_') builder.Append(c);
                else builder.Append(".");
            }

            return builder.ToString();
        }

        public static string ParentDirectory(this string path)
        {
            int lastIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastIndex < 0) lastIndex = path.LastIndexOf(Path.AltDirectorySeparatorChar);
            if (lastIndex > 0) return path.Substring(0, lastIndex);
            return null;
        }

        public static string NameOnly(this string path)
        {
            int lastIndex = path.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastIndex < 0) lastIndex = path.LastIndexOf(Path.AltDirectorySeparatorChar);
            if (lastIndex >= 0) return path.Substring(lastIndex + 1);
            return path;
        }
    }
}