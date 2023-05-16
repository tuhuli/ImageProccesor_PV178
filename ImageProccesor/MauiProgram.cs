using ImageProccesor.Services;
using ImageProccesor.ViewModel;
using System.Runtime.Versioning;


namespace ImageProccesor;

public static class MauiProgram
{
    [SupportedOSPlatform("Windows")]
    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()

			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<ImageService>();

		builder.Services.AddSingleton<ImageViewModel>();
		return builder.Build();
	}
}
