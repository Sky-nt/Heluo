using Microsoft.Extensions.Logging;
using Heluo.Infrastructure.Core.TypeFinder;
using Heluo.Infrastructure.Core.Initialization;
using Heluo.Infrastructure.Core.IOC;
using Autofac;
using Autofac.Extensions.DependencyInjection;
namespace Heluo;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		SingletonToolInitialization.Instance().Init();
		builder.UseMauiApp<App>()
			   .ConfigureFonts(fonts =>
			   {
			   	  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			   });
		builder.ConfigureContainer(new AutofacServiceProviderFactory(), builder =>
		{
			builder.RegisterModule<AutoFacRegisterModlue>();
		});
		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddMasaBlazor();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
