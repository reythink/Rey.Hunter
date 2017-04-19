using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rey.Hunter.TagHelpers.Hash {
    internal class HashUtility {
        public static string MapPath(string root, string href) {
            if (string.IsNullOrEmpty(root))
                throw new ArgumentNullException(nameof(root));

            if (string.IsNullOrEmpty(href))
                return null;

            var path = root;
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                path += Path.DirectorySeparatorChar;
            }

            if (!href.StartsWith("/"))
                throw new InvalidOperationException($"Cannot map path for relative href. \"{href}\"");

            return path + href.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        }

        public static string Hash(string path) {
            if (string.IsNullOrEmpty(path))
                return null;

            var buffer = SHA1.Create().ComputeHash(File.ReadAllBytes(path));
            return string.Join("", buffer.Select(x => x.ToString("X2")));
        }

        public static string Hash(string root, string href) {
            return Hash(MapPath(root, href));
        }
    }
}
