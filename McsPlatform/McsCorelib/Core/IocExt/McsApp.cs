using McsCoreLib.Interfaces.Diag;

using Newtonsoft.Json.Linq;

namespace McsCoreLib.Core.IocExt
{
    public static class McsApp
    {
        /// <summary>
        /// 通用IOC容器
        /// </summary>
        public static IServiceCollection McsRegistry { get; set; }

        /// <summary>
        /// 启动注册的单根实例从此容器取出
        /// </summary>
        public static IServiceProvider McsHostServiceProvider { get; set; }

        /// <summary>
        /// IOC对象提供器;
        /// </summary>
        public static IContainerProvider McsServiceProvider { get => ContainerLocator.Container; }

        /// <summary>
        /// 获取对话框服务
        /// </summary>
        public static IEnumerable<IDiagBaseUIService> Diags { get => McsServiceProvider.Resolve<IEnumerable<IDiagBaseUIService>>(); }

        /// <summary>
        /// 获取指定对话框 --传递参数
        /// </summary>
        /// <param name="diagName">对话框UI名称</param>
        /// <param name="mPara">传递对话框参数</param>
        public static IDiagBaseUIService GetDiag<T>(string diagName, T mPara) where T : new()
        {
            SetParaData(mPara);
            var diag = Diags?.FirstOrDefault(x => x.WindowID == diagName);
            return diag ?? throw new Exception($"Diag {diagName} not found");
        }

        /// <summary>
        /// 获取指定对话框- 不传递参数
        /// </summary>
        /// <param name="diagName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IDiagBaseUIService GetDiag(string diagName)
        {
            var diag = Diags?.FirstOrDefault(x => x.WindowID == diagName);

            return diag ?? throw new Exception($"Diag {diagName} not found");
        }

        /// <summary>
        /// 设置对话框参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="obj"></param>

        public static void SetParaData<T>(T obj) where T : new()
        {
            var diag = McsServiceProvider.Resolve<IDiagPara>();
            if (diag != null)
            {
                diag.Value = JObject.FromObject(obj);
            }
            else
            {
                throw new Exception("DiagPara not found");
            }
        }

        /// <summary>
        /// 获取对话框参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <returns></returns>

        public static T GetParaData<T>() where T : new()
        {
            var diag = McsServiceProvider.Resolve<IDiagPara>();
            return JsonTool.JsonObjectToObject<T>(diag.Value) ?? default;
        }
    }
}