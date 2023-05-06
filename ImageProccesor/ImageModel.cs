using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProccesor
{
    public class ImageModel
    {
        public string ImageSource { get; }
        public string Name { get; }

        private static int _imagesCount = 0;
        public int ImageNumber { get; }

        public ImageModel(string source) 
        {
            ImageSource = source;
            Console.WriteLine($"source");
            _imagesCount++;
            ImageNumber = _imagesCount;
            Name = GetFileNameFromPath(source);
        }
        public static string GetFileNameFromPath(string path)
        {

            var filename = Path.GetFileName(path);

            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            return filename;
        }
    }
}
