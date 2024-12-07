namespace Heluo.Infrastructure.Core.TypeFinder;

public interface IHLTypeFinder
{
    #region 根据类型查找类

    /// <summary>
    /// 根据类型查找类
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="isOnlyRealizationClass"></param>
    /// <returns></returns>
    List<Type> FinderClass(Type targetType, bool isOnlyRealizationClass = true);

    #endregion

    #region 根据泛型查找类

    /// <summary>
    /// 根据泛型查找类
    /// </summary>
    /// <param name="isOnlyRealizationClass"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<Type> FinderClass<T>(bool isOnlyRealizationClass = true);

    #endregion

    #region 根据特性查找类

    /// <summary>
    /// 根据特性查找类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<Type> FinderClassByAttribute<T>() where T : Attribute;

	#endregion
}
