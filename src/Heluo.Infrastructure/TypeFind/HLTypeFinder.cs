using Heluo.Infrastructure.Core.Common;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Heluo.Infrastructure.Core.TypeFinder;

public class HLTypeFinder:IHLTypeFinder
{
    #region ctor

    /// <summary>
    /// 类查找器
    /// </summary>
    public HLTypeFinder()
    {
        _assemblySkipLoadingPattern="^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease|^netstandard|^SkyWalking|^Pomelo|^Consul|^Google|^MySqlConnector|^SSkyWalking|^Grpc.Core|^ef";
        _assemblyRestrictToLoadingPattern = ".*";
    }

    #endregion ctor

    #region fields

    /// <summary>
    /// 跳过的程序集
    /// </summary>
    private string _assemblySkipLoadingPattern;

    /// <summary>
    /// 需要加载的程序集
    /// </summary>
    private string _assemblyRestrictToLoadingPattern;

    #endregion

    #region Methods

    #region 根据类型查找类

    /// <summary>
    /// 根据类型查找类
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="isOnlyRealizationClass"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<Type> FinderClass(Type targetType,bool isOnlyRealizationClass=true)
    {
        var assemblies = GetAssembly();
        List<Type> result = new();
        foreach (var assembly in assemblies)
        {
           var types= assembly.GetTypes();
           foreach (var type in types)
           {
               if (targetType.IsAssignableFrom(type) || (targetType.IsGenericTypeDefinition && CompareGeneric(targetType, type)))
               {
                   if (isOnlyRealizationClass)
                   {
                       if (type.IsClass && !type.IsAbstract)
                       {
                           result.Add(type);
                       }
                   }
                   else
                   {
                       result.Add(type);
                   }
               }
           }
        }
        return result;
    }

    #endregion 根据类型查找类

    #region 根据泛型查找类

    /// <summary>
    /// 根据泛型查找类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isOnlyRealizationClass"></param>
    /// <returns></returns>
    public List<Type> FinderClass<T>(bool isOnlyRealizationClass=true)
    {
        return FinderClass(typeof(T), isOnlyRealizationClass);
    }

    #endregion 根据泛型查找类

    #region 根据特性查找类

    /// <summary>
    /// 根据特性查找类
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<Type> FinderClassByAttribute<T>() where T:Attribute
    {
        var assemblies = GetAssembly();
        List<Type> result = new();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (IsExistAttribute<T>(type))
                {
                    result.Add(type);
                }
            }
        }
        return result;
    }

	#endregion 根据特性查找类

	#region 根据类型查找接口

	/// <summary>
	/// 根据类型查找接口
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public List<Type> FinderInterfaces(Type targetType)
	{
		var interfaceTypes=targetType.GetInterfaces();
		List<Type> result = new();
		foreach (var interfaceType in interfaceTypes)
		{
			if (IsMatch(interfaceType.FullName))
			{
				if (targetType.IsAssignableFrom(interfaceType) || (CompareGeneric(targetType, type)))
				{
					if (type.IsInterface)
					{
						result.Add(type);
					}
				}
			}
			
		}
		return result;
	}

	#endregion

	#endregion Methods

	#region private Methods

	#region 获取程序集

	/// <summary>
	/// 获取程序集（暂不考虑引用的第三方类库或热插拔的问题,仅加载当前项目类库，后期需要再考虑）
	/// </summary>
	/// <returns></returns>
	private List<Assembly> GetAssembly()
    {
        //Where(x => x.Type.Equals("Project", StringComparison.InvariantCultureIgnoreCase))
        var libraries=  DependencyContext.Default!.RuntimeLibraries.ToList();
        List<AssemblyName> assemblyNames = new List<AssemblyName>();
        foreach (var library in libraries)
        {
            var assemblyNameList= library.GetDefaultAssemblyNames(DependencyContext.Default);
            foreach (var assemblyName in assemblyNameList)
            {
                if (IsMatch(assemblyName.FullName))
                {
                    assemblyNames.Add(assemblyName);
                }
            }
        }
        List<Assembly> assemblies = new List<Assembly>();
        NAssemblyLoadContext loadContext = new NAssemblyLoadContext();
        foreach (var assemblyName in assemblyNames)
        {
            assemblies.Add(loadContext.LoadFromAssemblyName(assemblyName));
        }
        return assemblies;
    }

    #endregion 获取程序集

    #region 对比泛型

    /// <summary>
    /// 对比泛型
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool CompareGeneric(Type targetType,Type type)
    {
       var targetTypeGeneric= targetType.GetGenericTypeDefinition();
       foreach (var item in type.FindInterfaces((_,_)=> true,null))
       {
           if (!item.IsGenericType)
           {
               continue;
           }
           if (targetTypeGeneric.IsAssignableFrom(item.GetGenericTypeDefinition()))
           {
               return true;
           }
       }
       return false;
    }

    #endregion

    #region 当前类型是否存在指定的特性

    /// <summary>
    /// 是否存在指定特性
    /// </summary>
    /// <returns></returns>
    private bool IsExistAttribute<T>(Type type) where T:Attribute
    {
        if (type.CustomAttributes.Any(x => x.AttributeType ==typeof(T)))
        {
            return true;
        }
        if (type.BaseType != null)
        {
           return IsExistAttribute<T>(type.BaseType);
        }
        return false;
    }

    #endregion

    #region 判断是否符合标准

    private bool IsMatch(string fullName) => !IsMatch(fullName, _assemblySkipLoadingPattern) &&
                                             IsMatch(fullName, _assemblyRestrictToLoadingPattern);

    private bool IsMatch(string fullName, string pattern) => Regex.IsMatch(fullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    #endregion

    #endregion private Methods
}
