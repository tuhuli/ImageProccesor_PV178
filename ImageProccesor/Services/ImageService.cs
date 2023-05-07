
using SixLabors.ImageSharp;


namespace ImageProccesor.Services
{
    public class ImageService
    {
        private readonly string _imageDirectoryPath;
        private readonly string _directoryName = "524985_MyAppImageProccesor";
        public List<ImageModel> Images { get; }
        public ImageService()
        {
            
            _imageDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _directoryName);

            // If the directory does not exist, create it
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
        
        public void AddPath(string path)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(path);
            string destinationPath = Path.Combine(_imageDirectoryPath, uniqueFileName);

            File.Copy(path, destinationPath);

            Images.Add(new ImageModel(destinationPath));
        }

        public void RemoveImage(int id)
        {
            Images.RemoveAll(image => image.ImageId == id);

        }
    }

    
}
