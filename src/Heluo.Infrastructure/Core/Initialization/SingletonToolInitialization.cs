
using Heluo.Infrastructure.Core.TypeFinder;
using System.Diagnostics.CodeAnalysis;

namespace Heluo.Infrastructure.Core.Initialization;

/// <summary>
/// 单例工具初始化
/// </summary>
public class SingletonToolInitialization
{
	#region fields

	private static SingletonToolInitialization? _instance;

	private static readonly object _lock = new object(); // 用于线程安全的锁

	private InstanceContainer? _instanceContainer;

	#endregion

	#region ctor

	/// <summary>
	/// 单例工具初始化
	/// </summary>
	/// <param name="container"></param>
	private SingletonToolInitialization(InstanceContainer container)
	{
		_instanceContainer = container;
	}

	#endregion

	#region 获取容器

	/// <summary>
	/// 获取容器
	/// </summary>
	/// <returns></returns>
	public static SingletonToolInitialization Instance()
	{
		// 双重检查锁定，确保线程安全
		if (_instance == null)
		{
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = new SingletonToolInitialization(new InstanceContainer());
				}
			}
		}
		return _instance!;
	}

	#endregion

	#region 初始化

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init()
	{
		AddTool<IHLTypeFinder>(new HLTypeFinder());
	}

	#endregion


	#region 添加工具

	/// <summary>
	/// 添加工具
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="instance"></param>
	/// <exception cref="ArgumentNullException"></exception>
	public void AddTool<T>([NotNull]T instance)
	{
		if (_instance == null)
		{
			throw new ArgumentNullException(nameof(instance));
		}
		var key = nameof(T);
		_instanceContainer!.AddInstance(key, instance!);
	}

	#endregion

	#region 获取工具

	/// <summary>
	/// 获取工具
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public T GetTool<T>()
	{
	   return _instanceContainer!.GetInstance<T>(nameof(T))??throw new Exception("获取的单例实例不存在");
	}

	#endregion
}
