
using CommunityToolkit.Mvvm.ComponentModel;
using ImageProccesor.Transformers;
using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;

namespace ImageProccesor
{
    [SupportedOSPlatform("Windows")]
    public partial class ImageModel: ObservableObject
    {
        public string ImageSourcePath { get; }

        [ObservableProperty]
        public ImageSource imageSourceSource; 
        public int ImageId { get; }
        public string ImageInfo => $"{ImageId}. {GetFileName()}";
        public Bitmap ImageBitmap { get; set; }

        private static int _imagesCount = 0;

        public ImageModel(string source)
        {
            ImageSourcePath = source;
            _imagesCount++;
            ImageId = _imagesCount;
            ImageBitmap = new Bitmap(source);
        }

        public string GetFileName()
        {
            var filename = Path.GetFileName(ImageSourcePath);

            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            return filename;
        }

        public void RefreshMauiImage()
        {
            Bitmap resizedImage = Resizer.FillIn(ImageBitmap, 400, 400);
            using var ms = new MemoryStream();
            resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            var imageBytes = ms.ToArray();

            var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            ImageSourceSource = imageSource;
            resizedImage.Dispose();
        }
    }
}
