namespace CoreWebAPI.CoreDtos.Material
{
    /// <summary>
    /// 物料信息
    /// </summary>
    public class MaterialInfoDto
    {
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialID { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料类型名称
        /// </summary>
        public int MaterialTypeID { get; set; }

        /// <summary>
        /// 最大容积
        /// </summary>
        public float MaxVolume { get; set; } = 0.0f;
        }
}
