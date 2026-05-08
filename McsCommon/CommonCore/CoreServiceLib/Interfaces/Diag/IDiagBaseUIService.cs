using CoreServiceLib.Core.IocExt;

namespace CoreServiceLib.Interfaces.Diag
{
    public interface IDiagBaseUIService
    {
        /// <summary>
        /// 非模态
        /// </summary>
        void ShowUI();

        /// <summary>
        /// 模态
        /// </summary>
        void ShowDialogUI();

        /// <summary>
        /// 窗口标识ID
        /// </summary>
        string WindowID { get; }

        /// <summary>
        ///  窗口参数传递
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ReturnData<T>() where T : new()
        {
            return McsApp.GetParaData<T>();
        }
    }
}