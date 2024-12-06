using Microsoft.Extensions.Logging;
using Heluo.Infrastructure.Core.TypeFinder;

namespace Heluo;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder.UseMauiApp<App>()
			   .ConfigureFonts(fonts =>
			   {
			   	  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			   });

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddMasaBlazor();
		builder.Services.BuildServiceProvider();



#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Services.AddSingleton<IHLTypeFinder,HLTypeFinder>();
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
