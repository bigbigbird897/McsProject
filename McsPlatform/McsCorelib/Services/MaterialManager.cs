using McsCoreLib.Interfaces.Material;
using McsCoreLib.Models.Material;

using System.Collections.Concurrent;

namespace McsCoreLib.Services
{
    public class MaterialManager(IServiceProvider service) : IMaterialManager, ISingletonDependency
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ConcurrentDictionary<int, (IMaterial material, MaterialTypeConfig config)> mMaterials = [];

        public (IMaterial material, MaterialTypeConfig config) GetMaterial(int materialId)
        {
            try
            {
                var material = mMaterials[materialId];
                return material;
            }
            catch (Exception ex)
            {
                logger.Error(ex); ;
                return (null, null);
            }
        }

        public async Task MaterialTypeInit()
        {
            try
            {
                logger.Info("物料类型加载开始");
                var db = McsDbTool.GetDBRepositoryRef<MaterialTypeConfig>();
                var msc = service.GetServices<IMaterial>();
                if (db == null)
                {
                    logger.Error(new Exception("物料加载，数据库连接错误"));
                    return;
                }

                mMaterials.Clear();
                foreach (var item in msc)
                {
                    var x = await db.AsQueryable().Where(a => a.MaterialTypeID == item.MaterialTypeID).FirstAsync().ConfigureAwait(false);
                    if (x == null)
                    {
                        x = new MaterialTypeConfig { MaterialTypeID = item.MaterialTypeID, MaterialTypeName = item.MaterialTypeName };
                        await db.InsertAsync(x).ConfigureAwait(false);
                    }
                    mMaterials.TryAdd(x.MaterialTypeID, (item, x));
                }
                logger.Info("物料类型加载完成");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}