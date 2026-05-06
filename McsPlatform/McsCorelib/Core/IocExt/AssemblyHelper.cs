using Microsoft.Extensions.DependencyModel;

using System.Reflection;
using System.Runtime.Loader;

namespace McsCoreLib.Core.IocExt
{
    /// <summary>
    /// 程序集帮助类
    /// </summary>

    public static class AssemblyHelper
    {
        private static readonly string[] Filters = ["dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows"];
        private static readonly IEnumerable<Assembly> _allAssemblies;

        static AssemblyHelper()
        {
            _allAssemblies = DependencyContext.Default?.GetDefaultAssemblyNames()
                .Where(c => c.Name is not null && !Filters.Any(c.Name.StartsWith) && !FilterLibs.Any(c.Name.StartsWith))
                .Select(Assembly.Load);
        }

        public static IEnumerable<Assembly> AllAssemblies
        {
            get => _allAssemblies;
        }

        public static IEnumerable<Type> AllTypes
        {
            get
            {
                return _allAssemblies?.SelectMany(c => c.GetTypes());
            }
        }

        /// <summary>
        /// 需要排除的项目
        /// </summary>
        private static readonly List<string> FilterLibs = [];

        /// <summary>
        /// 根据程序集名字得到程序集
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesByName(params string[] assemblyNames) => assemblyNames.Select(o => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(AppContext.BaseDirectory, $"{o}.dll")));

        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static IEnumerable<Type> FindTypes(Func<Type, bool> predicate) => AllTypes!.Where(predicate).ToArray();

        /// <summary>
        /// 查找所有指定特性标记的类型
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindTypesByAttribute<TAttribute>() where TAttribute : Attribute => FindTypesByAttribute(typeof(TAttribute));

        /// <summary>
        /// 查找所有指定特性标记的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> FindTypesByAttribute(Type type) => [.. AllTypes!.Where(a => a.IsDefined(type, true)).Distinct()];

        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static IEnumerable<Assembly> FindAllItems(Func<Assembly, bool> predicate) => [.. _allAssemblies!.Where(predicate)];
    }
}