using CoreServiceLib.Core.DBCore;

namespace CoreServiceLib.Models.UserAuth
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class SystemUser
    {
        [SugarColumn(IsPrimaryKey = true, Length = 20, ColumnDescription = "用户编号")]
        public string UserID { get; set; }

        [SugarColumn(Length = 40, ColumnDescription = "用户名")]
        public string UserName { get; set; }

        [SugarColumn(Length = 400, ColumnDescription = "用户密码")]
        public string Password { get; set; }

        [SugarColumn(ColumnDescription = "权限id")]
        public int AuthLevelId { get; set; }

        [SugarColumn(ColumnDescription = "禁用")]
        public bool Disabled { get; set; }
    }
}