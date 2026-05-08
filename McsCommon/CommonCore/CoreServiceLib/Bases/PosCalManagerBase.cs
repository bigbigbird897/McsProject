using CoreServiceLib.Interfaces.PosCalConfig;
using CoreServiceLib.Models.PosCalConfig;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Bases
{
    public abstract class PosCalManagerBase : IPosCalManager
    {
        //点位初始化
        public void PosCalInit<AreaType, PosType, ActionType>()
                    where AreaType : Enum
                    where PosType : Enum
                    where ActionType : Enum
        {
            var mdbAction = McsDbTool.GetDBRepositoryRef<ActionTypeInfo>();
            var mdbArea = McsDbTool.GetDBRepositoryRef<AreaTypeDef>();
            var mdbPos = McsDbTool.GetDBRepositoryRef<PosTypeInfo>();

            var actionTypeList = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().Select(x => new ActionTypeInfo
            {
                ActionTypeID = Convert.ToInt32(x),
                ActionTypeName = x.ToString()
            }).ToList();

            mdbAction.Context.DbMaintenance.TruncateTable<ActionTypeInfo>();
            mdbAction.InsertRange(actionTypeList);

            var areaTypeList = Enum.GetValues(typeof(AreaType)).Cast<AreaType>().Select(x => new AreaTypeDef
            {
                AreaTypeID = Convert.ToInt32(x),
                AreaTypeName = x.ToString()
            }).ToList();

            mdbArea.Context.DbMaintenance.TruncateTable<AreaTypeDef>();
            mdbArea.InsertRange(areaTypeList);

            var posTypeList = Enum.GetValues(typeof(PosType)).Cast<PosType>().Select(x => new PosTypeInfo
            {
                PosTypeID = Convert.ToInt32(x),
                PosTypeName = x.ToString()
            }).ToList();

            mdbPos.Context.DbMaintenance.TruncateTable<PosTypeInfo>();
            mdbPos.InsertRange(posTypeList);

            //更新区域配置
            var mdbAreaData = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var configs = mdbAreaData.AsQueryable().ToList();
            foreach (var area in areaTypeList)
            {
                var imp = configs.Where(x => x.AreaID == area.AreaTypeID).FirstOrDefault();
                if (imp == null)
                {
                    var config = new AreaDataConfig
                    {
                        AreaID = area.AreaTypeID,
                        AreaName = area.AreaTypeName
                    };
                    mdbAreaData.Insert(config);
                }
            }

            //不存在的删除
            configs = mdbAreaData.AsQueryable().ToList();
            foreach (var config in configs)
            {
                var area = areaTypeList.Where(x => x.AreaTypeID == config.AreaID).FirstOrDefault();
                if (area == null) mdbAreaData.Delete(config);
            }

        }
        //获取区域类型定义
        public List<AreaTypeDef> GetAreaTypeDefs()
        {
            var mdbArea = McsDbTool.GetDBRepositoryRef<AreaTypeDef>();
            return mdbArea.AsQueryable().ToList();
        }
        //获取动作类型定义
        public List<ActionTypeInfo> GetActionTypeInfoDefs()
        {
            var mdbAction = McsDbTool.GetDBRepositoryRef<ActionTypeInfo>();
            return mdbAction.AsQueryable().ToList();
        }
        //获取点位类型定义
        public List<PosTypeInfo> GetPosTypeInfoDefs()
        {
            var mdbPos = McsDbTool.GetDBRepositoryRef<PosTypeInfo>();
            return mdbPos.AsQueryable().ToList();
        }
        //获取区域参数
        public Tpara GetAreaPara<Tpara>(int mAreaID) where Tpara : class
        {
            Tpara data = null;
            var mdb = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var config = mdb.AsQueryable().Where(x => x.AreaID == mAreaID).First();
            if (config != null) data = config.AreaPara.ToObject<Tpara>();
            return data;
        }
        //保存区域参数
        public void SetAreaPara<Tpara>(int mAreaID, Tpara mAreaPara) where Tpara : class
        {
            var mdb = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var config = mdb.AsQueryable().Where(x => x.AreaID == mAreaID).First();
            if (config == null)
            {
                config = new AreaDataConfig
                {
                    AreaID = mAreaID,
                    AreaPara = JObject.FromObject(mAreaPara)
                };
                mdb.Insert(config);
            }
            else
            {
                config.AreaPara = JObject.FromObject(mAreaPara);
                mdb.Update(config);
            }
        }
        //保存点位标定数据
        public void PosCalDataSave(int mAreaID, List<PosCalData> mPosCalDatas)
        {
            var mdb = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var config = mdb.AsQueryable().Where(x => x.AreaID == mAreaID).First();
            if (config != null)
            {
                config.PosCalDatas.Clear();
                config.PosCalDatas.AddRange(mPosCalDatas);
                mdb.Update(config);
            }
        }
        //保存标定计算点
        public void SaveCalPos(int mAreaID, List<CalPosDef> mCalPosDatas)
        {
            var mdb = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var config = mdb.AsQueryable().Where(x => x.AreaID == mAreaID).First();
            if (config != null)
            {
                config.CalPosDatas.Clear();
                config.CalPosDatas.AddRange(mCalPosDatas);
                mdb.Update(config);
            }
        }
        //获取标定点数据
        public (Tpara areaPara, PosCalData mposData) GetPosCalData<Tpara>(int mAreaID, int mPosID) where Tpara : class
        {
            Tpara para = null;
            PosCalData mposData = null;
            var mdb = McsDbTool.GetDBRepositoryRef<AreaDataConfig>();
            var config = mdb.AsQueryable().Where(x => x.AreaID == mAreaID).First();
            if (config != null)
            {
                para = config.AreaPara.ToObject<Tpara>();
                mposData = config.PosCalDatas.Where(x => x.AreaPosID == mPosID).FirstOrDefault();
            }

            return (para, mposData);
        }
        
        //标定点位计算
        public abstract List<PosCalData> PosCal<Tpara>(int mAreaID, List<CalPosDef> mCalPosDatas);
        //点位示教
        public abstract int PosTeach(PosCalData mPos);
    }
}
