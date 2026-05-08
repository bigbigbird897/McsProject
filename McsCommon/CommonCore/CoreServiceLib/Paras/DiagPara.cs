using CoreServiceLib.Interfaces.Diag;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Paras
{
    public class DiagPara : IDiagPara, ISingletonDependency
    {
        public JObject Value { get; set; }
    }
}