using McsCoreLib.Interfaces.Hardware;

namespace McsCoreLib.Services
{
    public class SysControl : ISysControl, ISingletonDependency
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool isPause = false;
        public bool IsRunning { get; private set; }

        /// <summary>
        ///  流程退出
        /// </summary>
        public bool IsQuit { get; private set; }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        public McsSysControlType SysStatus { get; private set; }

        /// <summary>
        ///  写入系统状态
        /// </summary>
        /// <param name="sysType"></param>
        public void ChangeSysStatus(McsSysControlType sysType)
        {
            SysStatus = sysType;
            switch (sysType)
            {
                case McsSysControlType.Run:

                    IsRunning = true;
                    IsQuit = false;
                    isPause = false;
                    _logger.Info("任务运行中...");
                    break;

                //流程退出
                case McsSysControlType.Stop:
                    isPause = false;
                    IsQuit = true;
                    IsRunning = false;
                    _logger.Info("任务结束");
                    break;

                case McsSysControlType.Paused:
                case McsSysControlType.Error:
                    IsQuit = false;
                    isPause = IsRunning;
                    _logger.Info("任务暂停中...");
                    break;

                case McsSysControlType.SysInit:
                    IsQuit = false;
                    isPause = false;
                    IsRunning = false;
                    break;

                case McsSysControlType.Reset:
                    IsQuit = false;
                    isPause = false;

                    break;

                case McsSysControlType.UnLockPaused:
                    IsQuit = false;
                    isPause = false;
                    break;

                default:
                    break;
            }
        }

        public async Task<bool> PauseCheck()
        {
            while (isPause && IsRunning)
            {
                await Task.Delay(200).ConfigureAwait(false);
            }
            if (IsQuit) return false;
            else return true;
        }
    }
}