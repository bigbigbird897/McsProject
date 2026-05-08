using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.Material;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Bases
{
    public abstract class MaterialBase : IMaterial
    {
        public abstract int MaterialTypeID { get; }

        public abstract string DiagUIName { get; }
        public abstract string MaterialTypeName { get; }

        public JObject MaterialParaConfigUI(JObject config)
        {
            config ??= [];
            var dig = McsApp.GetDiag(DiagUIName, config);
            dig.ShowDialogUI();
            return dig.ReturnData<JObject>();
        }
    }
}