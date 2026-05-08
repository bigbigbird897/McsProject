namespace CoreWpfLib.Bases
{
    public abstract class McsModuleBase : IModule
    {
        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}