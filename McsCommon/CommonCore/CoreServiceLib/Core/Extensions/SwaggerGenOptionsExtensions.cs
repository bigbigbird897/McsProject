using CoreServiceLib.Core.Filters;
using CoreServiceLib.Core.IocExt;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

using System.Reflection;

namespace CoreServiceLib.Core.Extensions
{
    /// <summary>
    /// Swagger扩展
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        private static readonly Dictionary<string, string> docsDic = [];
        private static readonly Dictionary<string, string> endPointDic = [];

        /// <summary>
        /// 添加预定义的Swagger配置
        /// </summary>
        /// <param name="op"></param>
        /// <param name="defaultDocName">默认文档名称</param>
        public static void McsSwaggerGenOptions(this SwaggerGenOptions op, string defaultDocName = "")
        {
            //显示权限按钮
            op.AddSecurityDefinition("JWTBearer", new OpenApiSecurityScheme()
            {
                Description = "在输入框中输入认证信息",
                Name = "Authorization",        //jwt默认的参数名称
                In = ParameterLocation.Header,  //jwt默认存放Authorization信息的位置(请求头中)
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });


            var controllers = AssemblyHelper.FindTypesByAttribute<McsApiGroupAttribute>();
            foreach (var ctrl in controllers)
            {
                var attr = ctrl.GetCustomAttribute<McsApiGroupAttribute>();
                if (attr is null) continue;
                if (docsDic.ContainsKey(attr.Name)) continue;
                _ = docsDic.TryAdd(attr.Name, attr.Description);
                op.SwaggerDoc(attr.Name, new()
                {
                    Title = attr.Title,
                    Version = attr.Version,
                    Description = attr.Description
                });
            }
            op.DocInclusionPredicate((docName, apiDescription) =>
            {
                //反射拿到值
                var actionList = apiDescription.ActionDescriptor.EndpointMetadata.Where(x => x is McsApiGroupAttribute).ToList();
                if (actionList.Count != 0)
                {
                    return actionList.FirstOrDefault() is McsApiGroupAttribute attr && attr.Name == docName;
                }
                var not = apiDescription.ActionDescriptor.EndpointMetadata.Where(x => x is not McsApiGroupAttribute).ToList();
                return not.Count != 0 && docName == defaultDocName;
                //判断是否包含这个分组
            });
            var files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var file in files) op.IncludeXmlComments(file, true);

            op.OperationFilter<SwaggerAuthorizeFilter>();
            op.SchemaFilter<SwaggerDefaultValueFilter>();
            op.SchemaFilter<SwaggerEnumSchemaFilter>();


            op.DocumentFilter<SwaggerHiddenApiFilter>();
            op.OperationFilter<SwaggerParamIgnoreFilter>();
            op.SchemaFilter<SwaggerPropertyIgnoreFilter>();

        }

        /// <summary>
        /// 添加预定义的SwaggerUI配置
        /// </summary>
        /// <param name="op"></param>
        public static void McsSwaggerUIOptions(this SwaggerUIOptions op)
        {
            var controllers = AssemblyHelper.FindTypesByAttribute<McsApiGroupAttribute>();
            foreach (var ctrl in controllers)
            {
                var attr = ctrl.GetCustomAttribute<McsApiGroupAttribute>();
                if (attr is null) continue;
                if (endPointDic.ContainsKey(attr.Name)) continue;
                _ = endPointDic.TryAdd(attr.Name, attr.Description);
                op.SwaggerEndpoint($"/swagger/{attr.Name}/swagger.json", $"{attr.Title} {attr.Version}");
                op.DocExpansion(DocExpansion.None);
                op.DefaultModelExpandDepth(-1);

            }
        }
    }
}