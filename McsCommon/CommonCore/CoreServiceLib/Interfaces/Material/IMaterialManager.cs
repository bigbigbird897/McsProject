using CoreServiceLib.Models.Material;

namespace CoreServiceLib.Interfaces.Material
{
    public interface IMaterialManager
    {
        Task MaterialTypeInit();

        (IMaterial material, MaterialTypeConfig config) GetMaterial(int materialId);
    }
}