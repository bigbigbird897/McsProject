using McsCoreLib.Models.ManulInfo;
using McsCoreLib.Services.Manul;

using Newtonsoft.Json.Linq;

namespace McsCoreLib.Interfaces.ManulControl
{
    public interface IManul
    {
        /// <summary>
        /// 手动控件类型
        /// </summary>
        int ManulTypeID { get; }
        /// <summary>
        /// 手动控件名称  
        /// </summary>
        string ManulControlName { get; }

        /// <summary>
        /// wpf  配置界面名称
        /// </summary>
        string DiagUIName { get; }

        int MannulDo(JObject mPara);

        JObject ManulConfigUI(JObject mcfg);
        ManulPara<Title, InputPara, OutPutPara> GetManulPara<Title, InputPara, OutPutPara>(ManulControlConfig mcfg);
        ManulControlConfig SaveManulPara<Title, InputPara, OutPutPara>(ManulPara<Title, InputPara, OutPutPara> mPara);
    }
}
