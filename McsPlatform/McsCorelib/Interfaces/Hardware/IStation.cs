using McsCoreLib.Models.Hardware;

namespace McsCoreLib.Interfaces.Hardware
{
    public interface IStation
    {
        List<StationFunType> FunTypes { get; }
    }
}