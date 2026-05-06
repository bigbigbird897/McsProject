using McsCoreLib.Interfaces.Diag;

using Newtonsoft.Json.Linq;

namespace McsCoreLib.Paras
{
    public class DiagPara : IDiagPara, ISingletonDependency
    {
        public JObject Value { get; set; }
    }
}