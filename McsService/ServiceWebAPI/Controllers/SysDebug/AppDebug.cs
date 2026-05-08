
using HitbotMqtt.Messages;
using HitbotMqtt.Paras;

using HitbotMQTT.Messages;

using CoreServiceLib.Bases;
using CoreServiceLib.Core.Attributes;
using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.Hardware;
using CoreServiceLib.Paras;
using CoreServiceLib.Tools;

using CoreServiceLib.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;

using System.Text;
using Tcp.Interfaces;

namespace ServiceWebAPI.Controllers.SysDebug
{
    /// <summary>
    ///  系统调试
    /// </summary>
    [McsApiGroup("系统调试")]
    [AllowAnonymous]
    public class AppDebug : McsControlerBase
    {
        private readonly IHardwareManager mDevice = McsApp.McsServiceProvider.Resolve<IHardwareManager>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        ///  MQTT初始化
        /// </summary>

        [HttpPost]
        public void MqttInit()
        {
            Task.Run(async () =>
            {
                try
                {
                    var device = mDevice.GetDevice("中央控制器");
                    SendMessage<SetParaBase> message = new()
                    {
                        instruct = "SysInit",
                        set = new SetParaBase()
                    };

                    var result = await device.DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(message), 60000)
                    {
                    });
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
            });
        }

        /// <summary>
        /// MQTT 主题订阅
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TopicSend(string mTopicName)
        {
            var device = mDevice.GetDevice("中央控制器");
            await device.TopicSubscribe(mTopicName);
        }

        /// <summary>
        /// 消息发送测试
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<McsWebApiResult> SendEndMessage()
        {
            try
            {
                var device = mDevice.GetDevice("中央控制器");
                await device.PublishData("TaskStatus", $"TaskStatus:{"test"}-终止");
                return SuccessResult();
            }
            catch (Exception)
            {
                return ErrorResult();
            }
        }

        /// <summary>
        /// 服务器向客户端主动发送消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<McsWebApiResult> SendTcpMessage(string mdata)
        {
            try
            {
                var tcpm = McsApp.McsServiceProvider.Resolve<IMcsTcpManage>();

                await tcpm.GetServer(1000).SendMessage("127.0.0.1", Encoding.ASCII.GetBytes(mdata));
                return SuccessResult();
            }
            catch (Exception)
            {
                return ErrorResult();
            }
        }

        /// <summary>
        /// 向TCP服务器发送消息
        /// </summary>
        /// <param name="mdata"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<McsWebApiResult<string>> SendTcpServerMessage(string mdata)
        {
            try
            {
                var tcpm = McsApp.McsServiceProvider.Resolve<IMcsTcpManage>();

                var datas = await tcpm.SendServerDataAsync("127.0.0.1", 2000, Encoding.ASCII.GetBytes(mdata), true, 60000);
                return SuccessResult(Encoding.ASCII.GetString(datas));
            }
            catch (Exception)
            {
                return ErrorResult<string>();
            }
        }




    }
}