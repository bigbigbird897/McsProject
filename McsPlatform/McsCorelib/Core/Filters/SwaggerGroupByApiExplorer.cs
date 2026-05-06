using McsCoreLib.Core.Attributes;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace McsCoreLib.Core.Filters
{
    internal class SwaggerGroupByApiExplorer : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var apiGroup = controller.Attributes.OfType<McsApiGroupAttribute>().FirstOrDefault();
            if (apiGroup != null)
            {
                controller.ApiExplorer.GroupName = apiGroup.Name;
            }
        }
    }
}