namespace CoreServiceLib.Interfaces.Hardware
{
    /// <summary>
    /// 按工位配置加载设备
    /// </summary>
    public interface IHardwareManager
    {
        /// <summary>
        /// 线条创建
        /// </summary>
        void LineCreate();

        /// <summary>
        /// 工站类型创建
        /// </summary>
        void StationFunctionTypeRegist();

        /// <summary>
        /// 设备类型注册
        /// </summary>
        /// <returns></returns>
        Task DeviceTypeRegist();

        /// <summary>
        /// 设备创建
        /// </summary>
        /// <returns></returns>
        Task<bool> DeviceCreate();

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        IDevice GetDevice(string DeviceId);

        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <returns></returns>
        IDevice GetDeviceType(int DeviceId);

        /// <summary>
        /// 设备停止退出
        /// </summary>
        /// <returns></returns>
        Task DeviceQuit();
    }
}