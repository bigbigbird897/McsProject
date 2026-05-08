namespace CoreServiceLib.Interfaces.Hardware
{
    public interface ISysControl
    {
        bool IsQuit { get; }
        bool IsRunning { get; }
        McsSysControlType SysStatus { get; }

        void ChangeSysStatus(McsSysControlType sysType);

        Task<bool> PauseCheck();
    }

    public enum McsSysControlType
    {
        SysInit = 0, //系统初始化
        Run = 1,  //启动
        Paused = 2, //暂停
        Stop = 3, //停止
        Reset=4, //复位执行流程
        UnLockPaused=5, //解锁暂停
        Error=6, //错误
    }
}