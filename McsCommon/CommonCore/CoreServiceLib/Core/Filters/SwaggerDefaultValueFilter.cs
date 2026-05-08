using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Nodes;

namespace CoreServiceLib.Core.Filters
{
    /// <summary>
    /// 添加默认值显示
    /// </summary>

    public sealed class SwaggerDefaultValueFilter : ISchemaFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties is null) return;
            foreach (var info in context.Type.GetProperties())
            {
                // Look for class attributes that have been decorated with "[DefaultAttribute(...)]".
                var defaultAttribute = info.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultAttribute is null) continue;

                foreach (var property in schema.Properties)
                {

                    // Only assign default value to the proper element.
                    if (ToCamelCase(info.Name) != property.Key) continue;
                    if (property.Value is OpenApiSchema s)
                    {
                        s.Example = defaultAttribute.Value as JsonNode;
                        break;
                    }
                  
                }

            }
        }

        /// <summary>
        /// 转成驼峰形式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string ToCamelCase(string name) => char.ToLowerInvariant(name[0]) + name[1..];
    }
}