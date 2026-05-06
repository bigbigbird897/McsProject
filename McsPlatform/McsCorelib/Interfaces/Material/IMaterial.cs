using Newtonsoft.Json.Linq;

namespace McsCoreLib.Interfaces.Material
{
    /// <summary>
    ///  不同类型物料对应不同参数
    /// </summary>
    public interface IMaterial
    {
        int MaterialTypeID { get; }  //定义时指定，不可改变
        string DiagUIName { get; }
        string MaterialTypeName { get; }

        JObject MaterialParaConfigUI(JObject config);  //参数配置界面
    }
}