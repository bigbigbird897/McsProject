namespace CoreServiceLib.Core.IocExt
{
    public static class RegistUtils<T> where T : Attribute
    {
        public static Type[] GetTypes()
        {
            var mAllTypes = AssemblyHelper.AllTypes;
            List<Type> list = [];
            if (mAllTypes != null)
            {
                var entityTypes = mAllTypes.Where(u =>
                     !u.IsInterface && !u.IsAbstract && u.IsClass
                     && u.IsDefined(typeof(T), false));

                if (entityTypes.Any())
                {
                    list.AddRange(entityTypes);
                }
            }
            return [.. list];
        }
    }
}