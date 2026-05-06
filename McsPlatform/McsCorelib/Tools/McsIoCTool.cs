using McsCoreLib.Core.IocExt;

namespace McsCoreLib.Tools
{
    public class McsIoCTool
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObj<T>()
        {
            return McsApp.McsServiceProvider.Resolve<T>();
        }

        /// <summary>
        /// 1对多依据条件获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static T GetObj<T>(Func<T, bool> mid)
        {
            var objs = McsApp.McsServiceProvider.Resolve<IEnumerable<T>>();
            var x = objs.Where(mid).FirstOrDefault();
            return x;
        }

    }
}
