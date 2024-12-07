using Autofac;
using Heluo.Infrastructure.Core.Initialization;
using Heluo.Infrastructure.Core.TypeFinder;
using SmartInfrastructure.Dependency;

namespace Heluo.Infrastructure.Core.IOC;

/// <summary>
/// Autofac依赖注入模块
/// </summary>
public class AutoFacRegisterModlue:Module
{
	protected override void Load(ContainerBuilder builder)
	{
		var hlTypeFinder= SingletonToolInitialization.Instance().GetTool<IHLTypeFinder>();
		var scopeInstances = hlTypeFinder.FinderClass<IScopeDependency>().ToArray();
		builder.RegisterTypes(scopeInstances).AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
		var singInstances = hlTypeFinder.FinderClass<ISingletonDependency>().ToArray();
		builder.RegisterTypes(singInstances).AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
		var transientInstances = hlTypeFinder.FinderClass<ITransientDependency>().ToArray();
		builder.RegisterTypes(transientInstances).AsImplementedInterfaces().PropertiesAutowired().InstancePerDependency();
	}
}
