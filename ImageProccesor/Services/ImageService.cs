
using SixLabors.ImageSharp;


namespace ImageProccesor.Services
{
    public class ImageService
    {
        private List<ImageModel> _imagePaths;
        public ImageService() 
        {
            _imagePaths = new List<ImageModel>();
            _imagePaths.Add(new ImageModel("C:\\Users\\Peter\\OneDrive\\Obrázky\\Snímky obrazovky\\1.PNG"));


        }
        public List<ImageModel> GetImagesMaui()
        {



            return _imagePaths;
            throw new NotImplementedException();
        }
        
        public void AddPath(string path)
        {
            _imagePaths.Add(new ImageModel(path));
        }
    }

    
}
