using ImageProccesor.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ImageProccesor.Transformers;
using System.Drawing;

namespace ImageProccesor.ViewModel
{
    public partial class ImageViewModel : BaseViewModel

    {

        public ObservableCollection<ImageModel> Images { get; } = new();
        public ImageService ImageService { get; }


        public ImageViewModel() {
            ImageService = new ImageService();
            
        }

        [RelayCommand]
        public async Task GetImagesAsync()
        {

            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var images = ImageService.Images;
                Images.Clear();
                foreach (var image in images) 
                {
                    image.RefreshMauiImage();
                    Images.Add(image);
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" GetImagesError",$"{ex.Message}", "click");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task SmoothenImagesAsync()
        {
            int completed = 0;
            foreach(ImageModel image in Images)
            {
                LinearFilters.SmoothingAsync(image);
                completed++;
            }
            await GetImagesAsync();
        }

        [RelayCommand]
        public async Task BrightenImagesAsync() 
        {
            int brightnessAddition = 10;
            int completed = 0;
            foreach (ImageModel image in Images)
            {
                LinearFilters.AddBrightnessToImage(image, brightnessAddition);
                completed++;
            }
            await GetImagesAsync();
        }

        public async Task AddPicturesAsync(string path)
        {
   
            ImageService.AddImage(path);
            await GetImagesAsync();


            
        }
        [RelayCommand]
        public async Task DeletePicturesAsync(int id)
        {

            ImageService.RemoveImage(id);
            await GetImagesAsync();

        }
    }
}
