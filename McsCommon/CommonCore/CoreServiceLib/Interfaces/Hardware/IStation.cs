using CoreServiceLib.Models.Hardware;

namespace CoreServiceLib.Interfaces.Hardware
{
    public interface IStation
    {
        List<StationFunType> FunTypes { get; }
    }
}