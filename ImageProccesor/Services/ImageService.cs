
using System.Runtime.Versioning;

namespace ImageProccesor.Services
{
    [SupportedOSPlatform("Windows")]
    public class ImageService
    {
        private string _imageDirectoryPath;
       

        private readonly string _directoryName = "SavedImages";
        public List<ImageModel> Images { get; }
        public ImageService()
        {
            
            _imageDirectoryPath = Path.Combine(AppContext.BaseDirectory, _directoryName);

            if (!Directory.Exists(_imageDirectoryPath))
            {
                Directory.CreateDirectory(_imageDirectoryPath);
            }

            Images = new List<ImageModel>();
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
                string uniqueFileName = image.ImageInfo;
                string filePath = Path.Combine(_imageDirectoryPath, uniqueFileName);
                image.ImageBitmap.Save(filePath);
            }
        }
    }
}
