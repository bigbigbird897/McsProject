namespace McsCoreLib.Core.DBCore
{
    public class McsRepository<T> : SimpleClient<T> where T : class, new()
    {
        private ISqlSugarClient mDbRef = null;
        /// <summary>
        /// 事务处理用
        /// </summary>
        public ITenant Itenant { get => DbManager.McsDb; }

        /// <summary>
        ///  创建仓储
        /// </summary>

        public McsRepository() : base()
        {
            typeof(T).GetCustomAttributes(typeof(McsDbTypeAttribute), false).ToList()
                           .ForEach(x =>
                             {
                                 if (x is McsDbTypeAttribute mAtt)
                                 {
                                     mDbRef = DbManager.DbRef(mAtt.McsDB, default, false);
                                     return;
                                 }
                             });
        }

        public override ISqlSugarClient Context { get => mDbRef; }
    }
}