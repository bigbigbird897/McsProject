using FluentValidation;

using CoreServiceLib.Core.IocExt;

namespace CoreServiceLib.Tools
{
    public class McsValidator<T>
    {
        public static IValidator<T> Obj { get => McsApp.McsServiceProvider.Resolve<IValidator<T>>(); }
    }
}