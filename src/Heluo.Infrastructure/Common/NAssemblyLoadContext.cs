using System.Reflection;
using System.Runtime.Loader;

namespace Heluo.Infrastructure.Core.Common;

public class NAssemblyLoadContext:AssemblyLoadContext
{
    #region ctor

    public NAssemblyLoadContext():base(true)
    {

    }

    #endregion ctor

    #region Methods

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        return null;
    }

    #endregion Methods
}
