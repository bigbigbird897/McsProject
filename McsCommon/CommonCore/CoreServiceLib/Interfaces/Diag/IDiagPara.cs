using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Interfaces.Diag
{
    public interface IDiagPara
    {
        JObject Value { get; set; }
    }
}