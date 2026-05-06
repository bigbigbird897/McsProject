using McsCoreLib.Models.Material;

namespace McsCoreLib.Interfaces.Material
{
    public interface IMaterialManager
    {
        Task MaterialTypeInit();

        (IMaterial material, MaterialTypeConfig config) GetMaterial(int materialId);
    }
}