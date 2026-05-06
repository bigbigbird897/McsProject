using Newtonsoft.Json.Linq;

namespace McsCoreLib.Interfaces.Diag
{
    public interface IDiagPara
    {
        JObject Value { get; set; }
    }
}