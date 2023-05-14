
using ImageProccesor.ViewModel;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using ImageProccesor.Transformers.Kernels;

namespace ImageProccesor;

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
            // Update _smoothingKernel with the large kernel
            _viewModel.SmoothingKernel = Kernel.LargeGaussianKernel;
        }
    }

    private void MediumRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            // Update _smoothingKernel with the medium kernel
            _viewModel.SmoothingKernel = Kernel.MediumGaussianKernel;
        }
    }

    private void SmallRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            // Update _smoothingKernel with the small kernel
            _viewModel.SmoothingKernel = Kernel.SmallGaussianKernel;
        }
    }


}

