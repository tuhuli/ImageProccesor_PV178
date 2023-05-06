using System;
using CommunityToolkit.Mvvm;
using ImageProccesor.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

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
                var images = ImageService.GetImagesMaui();
                Images.Clear();
                foreach (var image in images) 
                {
                    
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

        public async Task AddPictureAsync(string path)
        {
            ImageService.AddPath(path);
            await GetImagesAsync();
        }
    }
}
