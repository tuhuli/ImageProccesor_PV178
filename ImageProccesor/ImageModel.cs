
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace ImageProccesor
{
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
            var resizedImage = new Bitmap(ImageBitmap, new System.Drawing.Size(400, 400));
            int newBlue = ImageBitmap.GetPixel(0, 0).B;
            using var ms = new MemoryStream();
            resizedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            var imageBytes = ms.ToArray();

            var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            ImageSourceSource = imageSource;
        }
    }
}
