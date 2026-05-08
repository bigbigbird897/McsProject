using CSnakes.Runtime;

using CoreServiceLib.Core.Exceptions;
using CoreServiceLib.Core.Extensions;
using CoreServiceLib.Core.Filters;
using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.Diag;
using CoreServiceLib.Interfaces.Hardware;
using CoreServiceLib.Interfaces.ManulControl;
using CoreServiceLib.Interfaces.Material;
using CoreServiceLib.Interfaces.Workflow;
using CoreServiceLib.Models.UserAuth;

using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi;

namespace CoreServiceLib.Services
{
    [DependsOn
      (

        typeof(AbpFluentValidationModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpHttpClientModule),
        typeof(AbpAspNetCoreSignalRModule)
      )]
    public class McsCoreModule : AbpModule
    {
        private string mTitle, mVersion;
        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            try
            {
                //自定义GUID创建方式
                StaticConfig.CustomGuidFunc = () =>
                {
                    var d = McsApp.McsServiceProvider.Resolve<IGuidGenerator>().Create();
                    return d;
                };

                context.Services.Configure<AbpSequentialGuidGeneratorOptions>(options =>
                {
                    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
                });

                //添加映射器
                context.Services.AddTransient<IMapper, Mapper>();

                //添加Python 虚拟执行环境
                var pyVer = context.Configuration["PythonConfig:Version"];
                var mPyPath = context.Configuration["PythonConfig:PyPath"];
                var env = Path.Join(Environment.CurrentDirectory, mPyPath);
                var vPah = Path.Join(env, ".venv");
                context.Services
                    .WithPython()
                    .WithHome(env)
                    .FromWindowsInstaller(pyVer)
                    .WithVirtualEnvironment(vPah);


                //注册数据库仓储服务
                McsApp.McsRegistry.AddTransient(typeof(McsRepository<>));


                //注册WEB服务
                AddWebService(context);

                //批量注册服务
                context.Services.Scan(scan =>
                {
                    var asembly = AssemblyHelper.AllAssemblies;
                    //设备驱动服务
                    scan.FromAssemblies(asembly)
                       .AddClasses(classes => classes.AssignableTo<IDevice>())
                       .AsImplementedInterfaces()
                       .WithTransientLifetime();

                    //工位功能类型
                    scan.FromAssemblies(asembly)
                      .AddClasses(classes => classes.AssignableTo<IStation>())
                      .AsImplementedInterfaces()
                      .WithTransientLifetime();

                    //对话框服务
                    scan.FromAssemblies(asembly)
                      .AddClasses(classes => classes.AssignableTo<IDiagBaseUIService>())
                      .AsImplementedInterfaces()
                      .WithTransientLifetime();

                    //加载物料类型
                    scan.FromAssemblies(asembly)
                      .AddClasses(classes => classes.AssignableTo<IMaterial>())
                      .AsImplementedInterfaces()
                      .WithTransientLifetime();

                    //加载工艺步序类型
                    scan.FromAssemblies(asembly)
                     .AddClasses(classes => classes.AssignableTo<IStep>())
                     .AsImplementedInterfaces()
                     .WithTransientLifetime();

                    //添加手动控制类型
                    scan.FromAssemblies(asembly)
                     .AddClasses(classes => classes.AssignableTo<IManul>())
                     .AsImplementedInterfaces()
                     .WithTransientLifetime();
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
                //WEB服务初始化
                DbAndPipInit(context);
                //添加默认用户
                await InitUser(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
            }
        }

        public override async Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
        {

        }

        #region 私有函数

        //添加默认用户
        private static async Task InitUser(ApplicationInitializationContext context)
        {
            //添加权限
            var mResService = McsDbTool.GetDBRepositoryRef<AuthResource>();
            var authLeveSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
            var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();

            if (mResService.AsQueryable().ToList().Count == 0)
            {
                await mResService.InsertRangeAsync(new List<AuthResource>()
                {
                    new() { Name="系统配置", OrderNo=1, ParentID=0, Path="" },
                    new() { Name="权限管理", OrderNo=2, ParentID=1, Path=""},
                    new() { Name="资源配置", OrderNo=3, ParentID=2, Path=""},
                    new() { Name="权限等级", OrderNo=4, ParentID=2, Path=""},
                    new() { Name="系统用户", OrderNo=4, ParentID=2, Path=""}
                }).ConfigureAwait(false);
            }

            if (authLeveSerice.AsQueryable().ToList().Count == 0)
            {
                await authLeveSerice.InsertAsync(new AuthLevel()
                {
                    AuthLevelId = 1,
                    AuthLevelName = "管理员",
                    Resources =
                       [
                           new ResListItem{ MenuID=1, MenuName="系统配置" },
                           new ResListItem{ MenuID=2, MenuName="权限管理" },
                           new ResListItem{ MenuID=3, MenuName="资源配置"},
                           new ResListItem{ MenuID=4, MenuName="权限等级" },
                           new ResListItem{ MenuID=5, MenuName="系统用户" }
                       ]
                }).ConfigureAwait(false);
            }

            if (userService.AsQueryable().ToList().Count == 0)
            {
                await userService.InsertAsync(new SystemUser()
                {
                    AuthLevelId = 1,
                    UserID = "admin",
                    UserName = "管理员",
                    Password = MD5Encrypt.Get("admin")
                }).ConfigureAwait(false);
            }
        }

        //添加WEB服务
        private void AddWebService(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            mTitle = config["ApiConfig:Title"];
            mVersion = config["ApiConfig:Version"];



            object value = context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(mVersion, new OpenApiInfo { Title = mTitle, Version = mVersion });
                options.DocInclusionPredicate((docname, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.McsSwaggerGenOptions();
            });
            context.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            context.Services.AddHttpClient();
            context.Services.AddProblemDetails();
            context.Services.AddExceptionHandler<BusinessExceptionHandler>();
            context.Services.ConfigureCors();
            AddControllers(context);


        }


        private static void DbAndPipInit(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder() as WebApplication;
            var env = context.GetEnvironment();
            var config = context.GetConfiguration();
            bool isDbForceInit = bool.Parse(config["DbConfig:DBForceInit"]);
            IdentityModelEventSource.ShowPII = true;

            //使用Swagger中间件
            app.UseSwagger(op =>
            {

            });


            if (env.IsDevelopment() || isDbForceInit)
            {
                DbManager.AllForceDbInit();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.MapStaticAssets();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseUnitOfWork();
            app.UseDynamicClaims();
            app.UseConfiguredEndpoints();

            app.UseSwaggerUI(options =>
            {
                options.McsSwaggerUIOptions();

            });
        }

        //添加所有模块中包含的WEB API
        private static void AddControllers(ServiceConfigurationContext context)
        {
            var controler = context.Services.AddControllers(options =>
            {
                options.Filters.Add<ResultWrapperFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Conventions.Add(new SwaggerGroupByApiExplorer());
            });
            List<string> assNames = [];

            var controllers = AssemblyHelper.FindTypesByAttribute<McsApiGroupAttribute>();
            foreach (var item in controllers)
            {
                var x = assNames.Where(a => a == item.Assembly.FullName).FirstOrDefault();
                if (x.IsNullOrEmpty())
                {
                    controler.AddApplicationPart(item.Assembly);
                    assNames.Add(item.Assembly.FullName);
                }
            }

            context.Services.AddEndpointsApiExplorer();
        }

        #endregion 私有函数
    }
}