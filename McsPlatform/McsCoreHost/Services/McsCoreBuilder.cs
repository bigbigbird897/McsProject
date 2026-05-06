using McsCoreLib.Core.IocExt;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace McsHost.Services
{
    internal class McsCoreBuilder
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();
        private readonly WebApplicationBuilder builder;
        private WebApplication app;
        public WebApplicationBuilder Webbuilder => builder;

        public McsCoreBuilder()
        {
            //创建宿主
            builder = WebApplication.CreateBuilder();
        }

        public async Task HostBuilder()
        {
            mLogger.Info("WEB服务启动");
            builder.Host.UseAutofac();
            McsApp.McsRegistry = builder.Services;



            //主数据库注册
            McsDbTool.DBRegist();

            var _WebApiPort = builder.Configuration.GetSection("McsPort:WebAPIPort").Get<int>();
            builder.WebHost.ConfigureKestrel(k =>
            {
                k.ListenAnyIP(_WebApiPort, config =>
                {
                    config.Protocols = HttpProtocols.Http1;
                });
            });

            await builder.AddApplicationAsync<McsHostModle>(options =>
            {
                //插件模块加载
            }).ConfigureAwait(false);

            app = builder.Build();
            //预编译容器
            McsApp.McsHostServiceProvider = app.Services;

        }

        public async Task HostRunAsync()
        {
            try
            {
                //启动WEB服务
                mLogger.Info("WEB服务启动完成");
                app.MapFallbackToFile("/index.html");
                await app.RunAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        public async Task HostStop()
        {
            try
            {
                if (app == null) return;
                //停止WEB服务
                mLogger.Info("WEB服务停止");
                await app.StopAsync().ConfigureAwait(false);
                LogManager.Shutdown();
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        public void WebServiceInit()
        {
            //数据库初始化
            McsDbTool.MasterDBInit();
            app.InitializeApplicationAsync().ConfigureAwait(false);
        }
    }
}