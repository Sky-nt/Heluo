using Heluo.Infrastructure.Core.Initialization;
using Heluo.Infrastructure.Core.TypeFinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartInfrastructure.Dependency;

namespace Heluo.Infrastructure.Core.IOC;
public static class ServiceCollectionExtension
{
	/// <summary>
	/// Ioc
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddIOC(this IServiceCollection service)
	{
		var typeFinder = SingletonToolInitialization.Instance().GetTool<IHLTypeFinder>();
		var scopeImplementationTypes = typeFinder.FinderClass<IScopeDependency>();

		

		foreach (var scopeImplementationType in scopeImplementationTypes)
		{
			var descriptor = new ServiceDescriptor(typeof(IScopeDependency), scopeImplementationType, lifetime.Value);
			services.Add(descriptor);
		}
	}
}
