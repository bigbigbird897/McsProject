namespace McsCoreLib.Core.Extensions
{
    public static class ServiceCorsExtensions
    {
        /// <summary>
        /// 配置跨域访问扩展方法
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services) =>
                //配置Cors
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", corsConfig =>
                        corsConfig.AllowAnyOrigin()     //配置允许的域名
                                          .AllowAnyMethod()     //配置允许的HTTP方法
                                          .AllowAnyHeader()); 	//配置允许的头内容
                });
    }
}