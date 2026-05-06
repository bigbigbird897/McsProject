using McsCoreLib.Models.PosCalConfig;

namespace McsCoreLib.Interfaces.PosCalConfig
{
    public interface IPosCalManager
    {
        /// <summary>
        /// 系统点位配置初始化
        /// </summary>
        /// <typeparam name="AreaType"></typeparam>
        /// <typeparam name="PosType"></typeparam>
        /// <typeparam name="ActionType"></typeparam>
        void PosCalInit<AreaType, PosType, ActionType>()
               where PosType : Enum
               where ActionType : Enum
               where AreaType : Enum;

        /// <summary>
        /// 获取区域类型定义列表
        /// </summary>
        /// <returns></returns>
        List<AreaTypeDef> GetAreaTypeDefs();

        /// <summary>
        /// 获取动作动作类型定义列表
        /// </summary>
        /// <returns></returns>
        List<ActionTypeInfo> GetActionTypeInfoDefs();

        /// <summary>
        /// 获取点位类型定义列表
        /// </summary>
        /// <returns></returns>
        List<PosTypeInfo> GetPosTypeInfoDefs();

        /// <summary>
        /// 获取区域配置参数
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <param name="mAreaID"></param>
        /// <returns></returns>

        Tpara GetAreaPara<Tpara>(int mAreaID) where Tpara:class;



       /// <summary>
       ///  保存区域配置参数
       /// </summary>
       /// <typeparam name="Tpara"></typeparam>
       /// <param name="mAreaID">区域编号</param>
       /// <param name="mAreaPara">区域配置参数</param>
        void SetAreaPara<Tpara>(int mAreaID, Tpara mAreaPara) where Tpara : class;

        /// <summary>
        /// 标定点位计算
        /// </summary>
        /// <typeparam name="Tpara"></typeparam>
        /// <param name="mAreaID"></param>
        /// <param name="mCalPosDatas"></param>
        /// <returns></returns>
        List<PosCalData> PosCal<Tpara>(int mAreaID, List<CalPosDef> mCalPosDatas);

        /// <summary>
        /// 标定计算点保存
        /// </summary>
        /// <param name="mAreaID">区域编号</param>
        /// <param name="mCalPosDatas">标定点</param>
        void SaveCalPos(int mAreaID, List<CalPosDef> mCalPosDatas);

        /// <summary>
        /// 标定数据保存
        /// </summary>
        /// <param name="mAreaID"></param>
        /// <param name="mPosCalDatas"></param>
        void PosCalDataSave(int mAreaID, List<PosCalData> mPosCalDatas);

        /// <summary>
        /// 获取标定点数据
        /// </summary>
        /// <typeparam name="Tpara"></typeparam>
        /// <param name="mAreaID"></param>
        /// <param name="mPosID"></param>
        /// <returns></returns>
        (Tpara areaPara, PosCalData mposData) GetPosCalData<Tpara>(int mAreaID, int mPosID) where Tpara : class;

        /// <summary>
        /// 点位示教
        /// </summary>
        /// <param name="mPos"></param>
        /// <returns></returns>
        int PosTeach(PosCalData mPos);
    }
}
