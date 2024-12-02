using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heluo.Infrastructure.Core.IOC;
public static class ServiceCollectionExtension
{
	/// <summary>
	/// Ioc
	/// </summary>
	/// <param name="services"></param>
	/// <returns></returns>
	public static IServiceCollection AddIOC(this IServiceCollection services)
	{ 


		return services;
	}
}
