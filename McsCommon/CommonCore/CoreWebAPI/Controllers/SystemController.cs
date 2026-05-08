using HitbotMqtt.Dtos;
using HitbotMqtt.Paras;

using HitbotMQTT.Messages;

using CoreWebAPI.CoreMessages;

using CoreServiceLib.Bases;
using CoreServiceLib.Core.Attributes;
using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Core.McsEvent;
using CoreServiceLib.Interfaces.Hardware;
using CoreServiceLib.Paras;
using CoreServiceLib.Tools;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace CoreWebAPI.Controllers
{
    /// <summary>
    /// 系统控制接口
    /// </summary>
    [McsApiGroup("系统控制")]
    [AllowAnonymous]
    public class SystemController() : McsControlerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// 系统启动
        /// </summary>
        [HttpPost]
        public async Task<McsWebApiResult> SysStart(string mTask)
        {

            ISysControl mControl = McsApp.McsServiceProvider.Resolve<ISysControl>();
            try
            {
                var mDevice = McsApp.McsServiceProvider.Resolve<IHardwareManager>();
                var device = mDevice.GetDevice("中央控制器");

                await device.DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(new SendMessage<McsAction>()
                {
                    instruct = "ChangeSysStatus",
                    set = new McsAction() { SysControl = 1 }
                }), 2000));
                if (!mControl.IsRunning)
                {

                }


                mControl.ChangeSysStatus(McsSysControlType.Run);
                _logger.Info($"任务 {mTask} 启动");

                return SuccessResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }

        }

        /// <summary>
        /// 系统停止
        /// </summary>
        [HttpPost]
        public async Task<McsWebApiResult> SysStop(string mTask)
        {
            try
            {
                ISysControl mControl = McsApp.McsServiceProvider.Resolve<ISysControl>();
                mControl.ChangeSysStatus(McsSysControlType.Stop);

                IHardwareManager mDevice = McsApp.McsServiceProvider.Resolve<IHardwareManager>();
                var device = mDevice.GetDevice("中央控制器");
                await device.DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(new SendMessage<McsAction>()
                {
                    instruct = "ChangeSysStatus",
                    set = new McsAction() { SysControl = 3 }
                }), 2000));

                _logger.Info($"任务 {mTask} 停止");

                return SuccessResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }

        }

        /// <summary>
        /// 系统暂停
        /// </summary>
        [HttpPost]
        public async Task<McsWebApiResult> SysPused(string mTask)
        {
            try
            {
                ISysControl mControl = McsApp.McsServiceProvider.Resolve<ISysControl>();
                if (mControl.IsRunning)
                {
                    mControl.ChangeSysStatus(McsSysControlType.Paused);
                    IHardwareManager mDevice = McsApp.McsServiceProvider.Resolve<IHardwareManager>();
                    var device = mDevice.GetDevice("中央控制器");
                    await device.DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(new SendMessage<McsAction>()
                    {
                        instruct = "ChangeSysStatus",
                        set = new McsAction() { SysControl = 2 }
                    }), 2000));

                    _logger.Info($"任务 {mTask} 暂停");

                }
                return SuccessResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }
        }

        /// <summary>
        ///  系统复位/故障清除
        /// </summary>     
        /// <returns></returns>
        [HttpPost]
        public async Task<McsWebApiResult> SysReset()
        {
            try
            {
                ISysControl mControl = McsApp.McsServiceProvider.Resolve<ISysControl>();
                if (!mControl.IsRunning)
                {
                    mControl.ChangeSysStatus(McsSysControlType.SysInit);
                    IHardwareManager mDevice = McsApp.McsServiceProvider.Resolve<IHardwareManager>();
                    var device = mDevice.GetDevice("中央控制器");
                    await device.DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(new SendMessage<McsAction>()
                    {
                        instruct = "ChangeSysStatus",
                        set = new McsAction() { SysControl = 1 }
                    }), 2000));
                    _logger.Info("系统复位成功");
                }
                return SuccessResult();
            }
            catch (Exception ex)
            {
                return ErrorResult(ex.Message);
            }
        }

        /// <summary>
        ///  主窗口外观控制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult MainWindowControl(ChangeWindow change)
        {
            try
            {
                McsEventMessage.Instance.Publish(change);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }
    }
}