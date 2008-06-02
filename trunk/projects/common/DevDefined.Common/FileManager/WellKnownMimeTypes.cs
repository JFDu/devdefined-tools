using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevDefined.Common.FileManager
{
    /// <summary>
    /// A collection of well known types, and some methods which can be used to check
    /// a mime type for it's base type i.e. Text, Image, Video or Audio.
    /// </summary>
    public static class WellKnownMimeTypes
    {
        public const string TextPlain = "text/plain";
        public const string ImageJpeg = "image/jpeg";
        public const string ImageBitmap = "image/bitmap";
        public const string ImageGif = "image/gif";
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Png")]
        public const string ImagePng = "image/png";
        public const string Text = "text";
        public const string Image = "image";
        public const string Video = "video";
        public const string Audio = "audio";
        public const string Application = "application";
        public const string ApplicationOctetStream = "application/octet-stream";

        public static bool IsApplication(string mimeType)
        {
            return mimeType.ToLower().StartsWith(Application);
        }

        public static bool IsText(string mimeType)
        {
            return mimeType.ToLower().StartsWith(Text);
        }

        public static bool IsImage(string mimeType)
        {
            return mimeType.ToLower().StartsWith(Image);
        }

        public static bool IsVideo(string mimeType)
        {
            return mimeType.ToLower().StartsWith(Video);
        }

        public static bool IsAudio(string mimeType)
        {
            return mimeType.ToLower().StartsWith(Audio);
        }
    }
}
