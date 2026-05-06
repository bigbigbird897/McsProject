using McsCoreLib.Configs;
using McsCoreLib.Interfaces.Hardware;
using McsCoreLib.Interfaces.ManulControl;
using McsCoreLib.Interfaces.Material;
using McsCoreLib.Interfaces.Workflow;

using McsWpfCore.WpfEx;

using McsWpfUI.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Net;

using WebAPI.Services;

namespace McsHost.Services
{
    [DependsOn(typeof(McsWebApiModle), typeof(McsWpfUIModle))]
    public class McsHostModle : AbpModule
    {
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            try
            {
                // 添加授权
                context.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    //JWT验证参数
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "McsAuth",
                        ValidAudience = "McsClient",
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Jwtsetting.SecretKey)),
                        ClockSkew = TimeSpan.Zero,
                    };
                    //执行验证
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            try
                            {
                                var path = context.Request.Path;

                                //跳过swagger UI请求
                                if (path.Value.Contains("swagger")) return Task.CompletedTask;

                                //重定向到用户登录界面
                                var allowAnonymous = context.HttpContext.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>();
                                if (allowAnonymous != null) return Task.CompletedTask;
                               
                                var auth = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>();
                                if (auth != null)
                                {
                                    //token 验证
                                    var token = context.Request.Headers.Authorization.ToString()?.Replace("Bearer ", "");
                                    if (!string.IsNullOrEmpty(token) && McsTokenTool.ValidateAccessToken(token, out var mNewToken))
                                    {
                                        // 如果续期了,则把新的token添加到头部返回
                                        if (!string.IsNullOrEmpty(mNewToken) && mNewToken != token)
                                        {
                                            context.Response.Headers.Append("New-Token", mNewToken);
                                            context.Token = mNewToken;
                                        }
                                    }
                                    else context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                }
                                else context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                                return Task.CompletedTask;
                            }
                            catch (Exception ex)
                            {
                                mLogger.Error(ex);
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                return Task.CompletedTask;
                            }
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var x = context.Exception;
                            return Task.CompletedTask;
                        }
                    };
                });


            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            try
            {
                // 注册视图和关联的视图模型
                ViewRegist.RegistViewMedels(context);

                //设置设备状态
                var sys = context.McsProvider().Resolve<ISysControl>();
                sys.ChangeSysStatus(McsSysControlType.SysInit);

                //启动设备管理器
                var deviceManager = context.McsProvider().Resolve<IHardwareManager>();
                if (deviceManager != null)
                {
                    deviceManager.LineCreate();
                    await deviceManager.DeviceTypeRegist().ConfigureAwait(false);
                    await deviceManager.DeviceCreate().ConfigureAwait(false);

                    //加载工位功能类型
                    deviceManager.StationFunctionTypeRegist();
                }

                //加载物料
                var mm = context.McsProvider().Resolve<IMaterialManager>();
                if (mm != null) await mm.MaterialTypeInit().ConfigureAwait(false);

                //设备命令系统注册
                var cmd = context.McsProvider().Resolve<IStepManager>();
                cmd?.StepTypeRegist();

                //加载手动控制类型
                var mmanul = context.McsProvider().Resolve<IManulManager>();
                mmanul?.ManulTypeRegist();

            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        public override async Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
        {
            try
            {
                var deviceManager = context.McsProvider().Resolve<IHardwareManager>();
                if (deviceManager != null)
                {
                    await deviceManager.DeviceQuit().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }
    }
}