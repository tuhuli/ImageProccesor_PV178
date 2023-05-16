
using ImageProccesor.ViewModel;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
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

    private async void FilePickerClickedAsync( object sender, EventArgs e)
    {
        var results = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Pick Images",
            FileTypes = FilePickerFileType.Images
        });

        foreach( var result in results )
        {
            await _viewModel.AddPicturesAsync(result.FullPath);
        }
        
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

