using McsCoreLib.Models.UserAuth;

namespace McsCoreInterface.CoreDtos.McsAuth
{
    /// <summary>
    /// 系统资源参数
    /// </summary>
    public class AuthResDto
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public long MenuID { get; set; }

        /// <summary>
        /// 根菜单编号
        /// </summary>
        public long ParentID { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 组件名称
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 重定向地址
        /// </summary>
        public string Redirect { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///  排序号
        /// </summary>
        public int OrderNo { get; set; } = 1;

        /// <summary>
        /// 元数据
        /// </summary>
        public MetaInfo Meta { get; set; }

        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<AuthResDto> Children { get; set; } = [];
    }
}