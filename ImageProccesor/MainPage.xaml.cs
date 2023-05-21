
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

