using ImageProccesor.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using ImageProccesor.Transformers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.Versioning;
using ImageProccesor.Transformers.Kernels;
   

namespace ImageProccesor.ViewModel
{
    [SupportedOSPlatform("Windows")]
    public partial class ImageViewModel : BaseViewModel

    {

        public ObservableCollection<ImageModel> Images { get; } = new();
        public ImageService ImageService { get; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CompletedPicturesInfo))]
        private int? _completedPictures;
        public string CompletedPicturesInfo => $"Finished Pictures = {CompletedPictures}";
        public Kernel SmoothingKernel { get; set; }
        private string _brightness;
        public string Brightness
        {
            get { return _brightness; }
            set
            {
                int result;
                if (int.TryParse(value, out result) && result >= -256 && result <= 256)
                {
                    _brightness = value;
                    OnPropertyChanged(nameof(Brightness));
                }
            }
        }
        [ObservableProperty]
        private int _hue;
       

        public ImageViewModel()
        {
            ImageService = new ImageService();

            CompletedPictures = 0;
            SmoothingKernel= new SmallGaussianKernel();
            
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
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                CompletedPictures = 0;
                foreach (ImageModel image in Images)
                {
                    await LinearFilters.SmoothingAsync(image, SmoothingKernel);
                    CompletedPictures++;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" Smoothing Error", $"{ex.Message}", "click");
            }
            finally
            {
                IsBusy = false;
            }
            
            await GetImagesAsync();
            
        }

        [RelayCommand]
        public async Task BrightenImagesAsync() 
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;

                CompletedPictures = 0;
                foreach (ImageModel image in Images)
                {
                    await LinearFilters.AddBrightnessToImageAsync(image, int.Parse(Brightness));

                    CompletedPictures++;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" BrightnessError", $"Write correct value ", "click");
            }
            finally
            {
                IsBusy = false;
            }
            
            await GetImagesAsync();

        }

        [RelayCommand]
        public async Task ModifyHueImagesAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;

                CompletedPictures = 0;
                foreach (ImageModel image in Images)
                {
                    await LinearFilters.ModifyHueAsync(image, Hue);

                    CompletedPictures++;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" Hue Error", $"{ex.Message}", "click");
            }
            finally
            {
                IsBusy = false;
            }

            await GetImagesAsync();

        }

        [RelayCommand]
        public async Task SharpenImageAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;

                CompletedPictures = 0;
                foreach (ImageModel image in Images)
                {
                    await LinearFilters.SmoothingAsync(image, new UnsharpMaskingKernel());

                    CompletedPictures++;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" Hue Error", $"{ex.Message}", "click");
            }
            finally
            {
                IsBusy = false;
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

        [RelayCommand]
        public async Task SaveImagesAsync()
        {
            if (IsBusy)
            {
                return;
            }
            try
            {
                IsBusy = true;
                var images = ImageService.Images;
                images.ForEach(image => ImageService.SaveImage(image.ImageId));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert(" GetImagesError", $"{ex.Message}", "click");
            }
            finally
            {
                IsBusy = false;
            }

        }

        


    }


}
