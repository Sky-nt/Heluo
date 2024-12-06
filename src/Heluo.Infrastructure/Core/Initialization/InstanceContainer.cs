namespace Heluo.Infrastructure.Core.Initialization;

/// <summary>
/// 实例容器
/// </summary>
public class InstanceContainer
{
	#region fields

	private static Dictionary<string, object>? _instanceDic;

	#endregion

	/// <summary>
	/// 实例容器
	/// </summary>
	public InstanceContainer()
	{
		_instanceDic = new Dictionary<string, object>();
	}

	#region 添加实例

	/// <summary>
	/// 添加实例
	/// </summary>
	/// <param name="key"></param>
	/// <param name="instance"></param>
	public void AddInstance(string key,object instance)
	{
		_instanceDic!.Add(key,instance);
	}

	#endregion

	#region 获取实例

	/// <summary>
	/// 获取实例
	/// </summary>
	/// <param name="key"></param>
	public object? GetInstance(string key)
	{
		if (_instanceDic!.TryGetValue(key, out object? instance))
		{
			return instance;
		}
		return null;
	}

	#endregion


	#region 获取实例

	/// <summary>
	/// 获取实例
	/// </summary>
	/// <param name="key"></param>
	public T? GetInstance<T>(string key)
	{
		if (_instanceDic.TryGetValue(key, out object? instance))
		{
			return (T)instance;
		}
		return default;
	}

	#endregion
}
