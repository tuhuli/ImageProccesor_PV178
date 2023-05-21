
using ImageProccesor.ViewModel;
using ImageProccesor.Transformers.Kernels;
using System.Runtime.Versioning;

namespace ImageProccesor;
[SupportedOSPlatform("Windows")]
public partial class MainPage : ContentPage
{
    private ImageViewModel _viewModel;


	public MainPage()
	{
		InitializeComponent();

        _viewModel= new ImageViewModel();
        BindingContext= _viewModel;
    }

    private async void FileImagePickerAsync( object sender, EventArgs e)
    {
        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Pick Images",
            FileTypes = FilePickerFileType.Images
        });

        await _viewModel.AddPicturesAsync(results); 
    }

    private async void FileSaverPickerAsync(object sender, EventArgs e)
    {

        var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { "" } }, // file extension
                });

        var directory = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Save Images",
            FileTypes = customFileType
        }); ;

        await _viewModel.SaveImagesAsync(directory);
    }

    private void LargeRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _viewModel.SmoothingKernel = new LargeGaussianKernel();
        }
    }

    private void MediumRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _viewModel.SmoothingKernel = new MediumGaussianKernel();
        }
    }

    private void SmallRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _viewModel.SmoothingKernel = new SmallGaussianKernel();
        }
    }


}

