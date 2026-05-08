using System.Reflection;

namespace CoreServiceLib.Core.IocExt
{
    public class McsToRegion
    {
        public static void Regist(IRegionManager reg)
        {
            var mAllTypes = AssemblyHelper.AllTypes;
            if (mAllTypes != null)
            {
                var entityTypes = mAllTypes.Where(u =>
                     !u.IsInterface && !u.IsAbstract && u.IsClass
                     && u.IsDefined(typeof(McsRegionAttribute), false));

                if (entityTypes.Any() && reg != null)
                {
                    foreach (var item in entityTypes)
                    {
                        var ma = item.GetCustomAttribute<McsRegionAttribute>();
                        if (!string.IsNullOrEmpty(ma.RegionName))
                        {
                            reg.RegisterViewWithRegion(ma.RegionName, item);
                        }
                    }
                }
            }
        }
    }
}