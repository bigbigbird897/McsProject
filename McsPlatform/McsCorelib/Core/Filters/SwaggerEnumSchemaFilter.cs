using McsCoreLib.Core.Extensions;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

using System.Text;

namespace McsCoreLib.Core.Filters
{
    public class SwaggerEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                StringBuilder stringBuilder = new();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name =>
                    {
                        Enum e = (Enum)Enum.Parse(context.Type, name);
                        var data = $"{name}({e.GetEnumDesc()})={Convert.ToInt64(Enum.Parse(context.Type, name))}";

                        stringBuilder.AppendLine(data);
                    });
              

                if(model is OpenApiSchema openApiSchema)
                {
                    openApiSchema.Type=JsonSchemaType.String;
                    openApiSchema.Format = context.Type.Name;
                    openApiSchema.Description = stringBuilder.ToString();
                }  
            }
        }
    }
}
