using CoreServiceLib.Core.IocExt;

using System.Reflection;

namespace CoreServiceLib.Core.DBCore
{
    public static class CodeFirstUtils
    {
        public static Type[] GetTables(McsDbType dbType)
        {
            var mAllTypes = AssemblyHelper.AllTypes;
            List<Type> list = [];
            if (mAllTypes != null)
            {
                var entityTypes = mAllTypes.Where(u =>
                     !u.IsInterface && !u.IsAbstract && u.IsClass
                     && u.IsDefined(typeof(McsDbTypeAttribute), false));

                if (entityTypes.Any())
                {
                    var tt = entityTypes.Where(t => t.GetCustomAttribute<McsDbTypeAttribute>().McsDB == dbType)?.ToList();
                    if (tt != null && tt.Count > 0) list.AddRange(tt);
                }
            }

            return [.. list];
        }
    }
}