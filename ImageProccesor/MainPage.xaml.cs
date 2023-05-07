
using ImageProccesor.ViewModel;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;

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


}

