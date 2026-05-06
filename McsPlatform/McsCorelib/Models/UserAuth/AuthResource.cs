namespace McsCoreLib.Models.UserAuth
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class AuthResource
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "资源编号", IsIdentity = true)]
        public long MenuID { get; set; }

        [SugarColumn(ColumnDescription = "父Id")]
        public long ParentID { get; set; }

        [SugarColumn(Length = 200, IsNullable = true, ColumnDescription = "路由地址")]
        public string Path { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "组件名称",IsNullable =true)]
        public string Component { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "重定向",IsNullable =true)]
        public string Redirect { get; set; }

        [SugarColumn(Length = 100, ColumnDescription = "菜单名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "排序编号")]
        public int OrderNo { get; set; } = 1;

        [SugarColumn(IsJson =true, ColumnDescription ="菜单元数据",IsNullable =true)]
        public MetaInfo Meta { get; set; }=new MetaInfo();

        [SugarColumn(IsIgnore = true)]
        public List<AuthResource> Children { get; set; }
    }
}