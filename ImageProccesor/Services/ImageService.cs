
using System.Runtime.Versioning;

namespace ImageProccesor.Services
{
    [SupportedOSPlatform("Windows")]
    public class ImageService
    {
        private readonly string _imageDirectoryPath;
        // my directory path while debugging C:\Users\Peter\Desktop\C#\ImageProccesorApp\ImageProccesor\ImageProccesor\bin\Debug\net6.0-windows10.0.19041.0\win10-x64\AppX\524985_MyAppImageProccesor

        private readonly string _directoryName = "524985_MyAppImageProccesor";
        public List<ImageModel> Images { get; }
        public ImageService()
        {
            
            _imageDirectoryPath = Path.Combine(AppContext.BaseDirectory, _directoryName);

            if (!Directory.Exists(_imageDirectoryPath))
            {
                Directory.CreateDirectory(_imageDirectoryPath);
            }

            Images = new List<ImageModel>();

            Images.Add(new ImageModel("C:\\Users\\Peter\\OneDrive\\Obrázky\\Snímky obrazovky\\1.PNG"));
        }


        public void AddImage(string path)
        {
            Images.Add(new ImageModel(path));
        }

        public void RemoveImage(int id)
        {
            Images.FindAll(image => image.ImageId == id)
                .ForEach(image => image.ImageBitmap.Dispose());
            Images.RemoveAll(image => image.ImageId == id);

        }

        public void SaveImage(int id)
        {
            ImageModel image = Images.FirstOrDefault(img => img.ImageId == id);
            if (image != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.ImageSourcePath);
                string filePath = Path.Combine(_imageDirectoryPath, uniqueFileName);
                image.ImageBitmap.Save(filePath);
            }
        }
    }
}
