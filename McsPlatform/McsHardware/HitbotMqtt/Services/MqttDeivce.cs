using HitbotMqtt.Messages;
using HitbotMqtt.Paras;

using HitbotMQTT.Messages;

using McsCoreLib.Bases;
using McsCoreLib.Core.Extensions.McsLogger;
using McsCoreLib.Core.IocExt;
using McsCoreLib.Core.McsEvent;
using McsCoreLib.Interfaces.Hardware;
using McsCoreLib.Models.Hardware.Device;
using McsCoreLib.Paras;
using McsCoreLib.Tools;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MQTTnet;

using NLog;

using System.Collections.Concurrent;

using Volo.Abp.EventBus.Local;

namespace HitbotMqtt.Services
{
    public class MqttDeivce(ILocalEventBus _eventBus, IServiceProvider service) : DeviceBase
    {
        private bool IsInit = false;
        private bool mStop = false;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IMqttClient mqttClient = null!;
        private MqttClientOptions mOptions = null!;
        private string mClientid = "";
        private readonly IConfiguration mDebugConfig = service.GetService<IConfiguration>();

        //订阅的Topic 列表
        private readonly ConcurrentDictionary<string, string> mTopicDataBuffer = new();

        private DeviceConfig config;
        private readonly ConcurrentDictionary<string, MqttRevData> mRevDataBuffer = new();

        private bool isDebug = false;
        public override int DeviceTypeID => 1;
        public override string DeviceTypeName => "HitbotOsMqtt";
        public override string DiagUIName => "HitbotMqttConfigUI";
        public override Func<IDevice, bool> SysInitAction { get; }
        public override void DeviceCreate(DeviceConfig mCfg)
        {
            config = mCfg;
            IsInit = false;
            var para = JsonTool.JsonObjectToObject<HitbotMqttPara>(mCfg.DevicePara);
            if (para != null)
            {
                isDebug = mDebugConfig.GetValue<bool>("MqttDebug");
                mClientid = $"{mCfg.DeviceId}_{para.ClientID}{isDebug}";
                mOptions = new MqttClientOptionsBuilder()
                .WithClientId(mClientid)
                .WithTcpServer(para.ServerIP, para.ServerPort)
                .WithCleanSession()
                .WithCleanStart()
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                .WithTimeout(TimeSpan.FromSeconds(5))
                .Build();
                mqttClient = new MqttClientFactory().CreateMqttClient();
                mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;
                mqttClient.DisconnectedAsync += MqttClient_DisconnectedAsync;
                mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
                mqttClient.ConnectingAsync += MqttClient_ConnectingAsync;

                logger.Info($"设备:{mCfg.DeviceId} 连接开始");
                mqttClient.ConnectAsync(mOptions, CancellationToken.None);

                if (para != null && !string.IsNullOrEmpty(para.MqttSysMessageTopic))
                {
                    //通过订阅事件向MQTT推送消息
                    McsEventMessage.Instance.Subscribe<List<SysMessage>>(async a =>
                    {
                        try
                        {
                            var data = JsonTool.ObjectToJson(a);
                            await PublishData(para.MqttSysMessageTopic, data);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                    });
                }
            }
        }

        public override async Task DeviceQuit()
        {
            try
            {
                mStop = true;
                await mqttClient?.DisconnectAsync();
                logger.Info($"设备:{config.DeviceId} 断开连接");
                mqttClient = null;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public override async Task PublishData(string topic, string message)
        {
            if (mqttClient != null && mqttClient.IsConnected)
            {
                //发送消息
                var info = new MqttApplicationMessage
                {
                    Topic = topic,
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,
                    PayloadSegment = Encoding.UTF8.GetBytes(message),
                    Retain = false
                };
                await mqttClient.PublishAsync(info).ConfigureAwait(false);
            }
        }

        public override async Task<DeviceRevData> DeviceWriteAsync<T>(T mCmds, CancellationToken tokenl)
        {
            DeviceRevData rev = new() { ResultCode = -1 };
            var icontrol = McsApp.McsServiceProvider.Resolve<ISysControl>();
            try
            {
                CancellationTokenSource tokenSource = new();
                Task m = null;
                if (mqttClient != null && mqttClient.IsConnected && mCmds is SendData mCmd)
                {
                    //主题订阅
                    if (mCmd.IsWaitRev)
                    {
                        //添加监控信息
                        mRevDataBuffer.TryAdd(mCmd.RevTopic, new MqttRevData
                        {
                            ClientID = mClientid,
                            Topic = mCmd.RevTopic,
                            RevMessage = null,
                            IsOK = false
                        });
                        await TopicSubscribe(mCmd.RevTopic).ConfigureAwait(false);
                        m = Task.Factory.StartNew(() =>
                        {
                            while (!tokenSource.IsCancellationRequested)
                            {
                                Thread.Sleep(mCmd.RevScanTime == 0 ? 1 : mCmd.RevScanTime);
                                var kk = mRevDataBuffer.Where(a => a.Key == mCmd.RevTopic && a.Value.IsRev).FirstOrDefault();
                                if (kk.Value != null)
                                {
                                    var mRevData = kk.Value;
                                    rev = mRevData.RevMessage;
                                    rev.ResultCode = kk.Value.IsOK ? 1 : -1;
                                    tokenSource.Cancel();
                                    break;
                                }
                            }
                        }, tokenl);
                    }


                    //发送消息
                    var message = new MqttApplicationMessage
                    {
                        Topic = mCmd.SendTopic,
                        QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce,

                        PayloadSegment = Encoding.UTF8.GetBytes(mCmd.SendMessage),
                        Retain = false
                    };
                    await mqttClient.PublishAsync(message, tokenl).ConfigureAwait(false);


                    //等待反馈
                    if (mCmd.IsWaitRev)
                    {
                        var timeout = mCmd.RevTimeOut;
                        var my = Task.Delay(timeout, tokenSource.Token);
                        await Task.WhenAny(m, my).ConfigureAwait(false);
                        if (my.IsCompleted) tokenSource.Cancel();
                        //取消订阅
                        await TopicUnsubscribe(mCmd.RevTopic).ConfigureAwait(false);
                        mRevDataBuffer.TryRemove(mCmd.RevTopic, out _);
                    }
                    else rev.ResultCode = 1;
                }
                else rev.ResultCode = -1;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                rev.ResultCode = -1;
            }
            return rev;
        }

        public override async Task TopicSubscribe(string topic)
        {
            try
            {
                var x = mTopicDataBuffer.TryAdd(topic, topic);
                if (mqttClient != null && mqttClient.IsConnected && x)
                {
                    await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                        .WithTopic(topic)
                        .WithAtLeastOnceQoS()
                        .Build()).ConfigureAwait(false);
                    logger.Info($"订阅主题:{topic} 成功");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public override async Task TopicUnsubscribe(string topic)
        {
            try
            {
                var x = mTopicDataBuffer.TryRemove(topic, out _);
                if (x && mqttClient != null && mqttClient.IsConnected)
                {
                    await mqttClient.UnsubscribeAsync(topic).ConfigureAwait(false);
                    logger.Info($"注销主题:{topic} 成功");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private async Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs args)
        {
            try
            {
                if (mqttClient != null && mqttClient.IsConnected)
                {
                    logger.Info($"{config.DeviceId} 连接成功");
                    foreach (var item in mTopicDataBuffer)
                    {
                        await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                        .WithTopic(item.Key)
                        .WithAtLeastOnceQoS()
                        .Build()).ConfigureAwait(false);
                        logger.Info($"订阅主题:{item.Key} 成功");
                    }

                    //系统初始化
                    if (!IsInit && !isDebug)
                    {
                        SendMessage<SetParaBase> message = new()
                        {
                            instruct = "SysInit",
                            set = new SetParaBase()
                        };

                        var result = await DeviceWriteAsync(new SendData(JsonTool.ObjectToJson(message)), default);
                        if (result.ResultCode == 1)
                        {
                            IsInit = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        //重试链接
        private async Task MqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs args)
        {
            try
            {
                if (mStop) return;
                logger.Info($"{config.DeviceId} 连接断开 -- 重新链接");
                await Task.Delay(1000);
                logger.Info($"设备:{config.DeviceId} 连接开始");
                await mqttClient?.ConnectAsync(mqttClient.Options);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private async Task MqttClient_ConnectingAsync(MqttClientConnectingEventArgs args)
        {
            logger.Info($"{config.DeviceId} 连接中...");
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private async Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            if (args.ClientId == mClientid)
            {
                var mTopic = args.ApplicationMessage.Topic;

                var mMessage = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                logger.Info($"客户端:{args.ClientId}--主题:{mTopic}-消息:{mMessage}");

                var result = JsonTool.JsonToObject<DeviceRevData>(mMessage);

                //判断是否是反馈消息
                var kk = mRevDataBuffer.Where(a => a.Key == mTopic).FirstOrDefault();
                if (kk.Value != null)
                {
                    var mRevData = kk.Value;
                    mRevData.IsRev = true;
                    mRevData.RevMessage = result;
                    mRevData.IsOK = false;
                    //写反馈结果
                    if (result != null) mRevData.IsOK = result.ResultCode > 0;
                    mRevDataBuffer[kk.Key] = mRevData;
                }

                await _eventBus.PublishAsync(new MqttRevData
                {
                    ClientID = args.ClientId,
                    Topic = mTopic,
                    RevMessage = result
                }).ConfigureAwait(false);
            }
        }
    }
}