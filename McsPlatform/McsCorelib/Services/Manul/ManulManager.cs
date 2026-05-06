using McsCoreLib.Interfaces.ManulControl;
using McsCoreLib.Models.ManulInfo;

namespace McsCoreLib.Services.Manul
{
    public class ManulManager(IServiceProvider service) : IManulManager, ISingletonDependency
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();
        public void ManulTypeRegist()
        {
            try
            {
                var mm = service.GetServices<IManul>();
                if (mm != null)
                {
                    List<ManulControlType> types = [];
                    foreach (var item in mm)
                    {
                        types.Add(new ManulControlType() { ManulTypeID = item.ManulTypeID, ManulTypeName = item.ManulControlName });
                    }

                    var db= McsDbTool.GetDBRepositoryRef<ManulControlType>();
                    db.InsertOrUpdate(types);
                }
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }

        }
    }
}
