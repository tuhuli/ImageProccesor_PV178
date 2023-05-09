using ImageProccesor.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ImageProccesor.Transformers;
using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.Versioning;

namespace ImageProccesor.ViewModel
{
    [SupportedOSPlatform("Windows")]
    public partial class ImageViewModel : BaseViewModel

    {

        public ObservableCollection<ImageModel> Images { get; } = new();
        public ImageService ImageService { get; }
        [ObservableProperty]
        private int? _completedPictures;


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
            CompletedPictures = 0;
            foreach (ImageModel image in Images)
            {
                LinearFilters.SmoothingAsync(image);
                CompletedPictures++;
            }
            await GetImagesAsync();
            
        }

        [RelayCommand]
        public async Task BrightenImagesAsync() 
        {
            int brightnessAddition = 10;
            CompletedPictures = 0;
            foreach (ImageModel image in Images)
            {
                await LinearFilters.AddBrightnessToImageAsync(image, brightnessAddition);
                
                CompletedPictures++;
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
