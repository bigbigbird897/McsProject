namespace CoreServiceLib.Configs
{
    public class Jwtsetting
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public static string SecretKey { get; } = "8aee58026fe2887cb23bf90326a19c66227289a64592a25d8bd0420bfb4c4f26";

        /// <summary>
        /// 过期时间
        /// </summary>
        public static TimeSpan ExpireTime { get; } = new TimeSpan(0, 10, 0);

        /// <summary>
        /// 续期时间,若在这段时间内请求接口,则续期
        /// </summary>
        public static TimeSpan RefreshExpireTime { get; } = new TimeSpan(1, 00, 0);
    }
}