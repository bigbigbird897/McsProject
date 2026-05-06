using McsCoreLib.Core.DBCore;

namespace McsCoreLib.Models.UserAuth
{
    [McsDbType(McsDbType.SysConfigDB)]
    public class TokenBuffer
    {
        [SugarColumn(Length = 500, IsPrimaryKey = true)]
        public string UserToken { get; set; }

        [SugarColumn(Length = 100)]
        public string UserID { get; set; }
    }
}