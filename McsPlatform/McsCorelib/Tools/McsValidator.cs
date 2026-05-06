using FluentValidation;

using McsCoreLib.Core.IocExt;

namespace McsCoreLib.Tools
{
    public class McsValidator<T>
    {
        public static IValidator<T> Obj { get => McsApp.McsServiceProvider.Resolve<IValidator<T>>(); }
    }
}