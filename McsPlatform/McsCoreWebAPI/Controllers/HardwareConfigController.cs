using HitbotMqtt.Paras;

using Mapster;

using McsCoreInterface.CoreDtos.Hardwares;

using McsCoreLib.Bases;
using McsCoreLib.Interfaces.Hardware;
using McsCoreLib.Models.Hardware.Device;
using McsCoreLib.Paras;
using McsCoreLib.Tools;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using NLog;

namespace McsCoreInterface.Controllers
{
    /// <summary>
    ///  系统硬件参数配置接口
    /// </summary>

    public class HardwareConfigController(IConfiguration config, IHardwareManager device) : McsControlerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///  获取设备类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public McsWebApiResult<List<DeviceTypeDto>> GetDeviceType()
        {
            try
            {
                var db = McsDbTool.GetDBRepositoryRef<DeviceTypeConfig>();
                var datas = db.AsQueryable().ToList().Adapt<List<DeviceTypeDto>>();

                return SuccessResult(datas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<List<DeviceTypeDto>>();
            }
        }

        /// <summary>
        /// 获取MQTT参数配置
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public McsWebApiResult<List<MqttParaDto>> GetAllMqttPara()
        {
            try
            {
                List<MqttParaDto> mqttParas = [];
                var db = McsDbTool.GetDBRepositoryRef<DeviceConfig>();
                var datas = db.AsQueryable().Where(a => a.DeviceTypeID == 1).ToList();
                foreach (var item in datas)
                {
                    var para = JsonTool.JsonObjectToObject<HitbotMqttPara>(item.DevicePara);
                    mqttParas.Add(new MqttParaDto()
                    {
                        DeviceId = item.DeviceId,
                        ClientID = para.ClientID,
                        MqttSysMessageTopic = para.MqttSysMessageTopic,
                        ServerIP = para.ServerIP,
                        ServerPort = para.ServerPort,
                        TimeOut = para.TimeOut,
                        DeviceName = item.DeviceName,
                        IsDisable = item.IsDisable
                    });
                }
                return SuccessResult(mqttParas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<List<MqttParaDto>>();
            }
        }

        /// <summary>
        ///  MQTT配置参数保存
        /// </summary>
        /// <param name="datas">保存数据</param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult SaveMqttDeviceConfig(List<MqttParaDto> datas)
        {
            try
            {
                List<DeviceConfig> configs = [];
                var stationid = config.GetValue<string>("LineID");
                var db = McsDbTool.GetDBRepositoryRef<DeviceConfig>();

                //删除旧的配置
                db.AsDeleteable().Where(a => a.DeviceTypeID == 1).ExecuteCommand();

                foreach (var item in datas)
                {
                    var para = new HitbotMqttPara()
                    {
                        ClientID = item.DeviceId,
                        MqttSysMessageTopic = item.MqttSysMessageTopic,
                        ServerIP = item.ServerIP,
                        ServerPort = item.ServerPort,
                        TimeOut = item.TimeOut,

                    };
                    configs.Add(new DeviceConfig()
                    {
                        DeviceId = item.DeviceId,
                        DeviceName = item.DeviceName,
                        IsDisable = item.IsDisable,
                        DeviceTypeID = 1,
                        LineID = stationid,
                        DevicePara = JsonTool.ObjectToJsonObject(para)
                    });
                }
                db.InsertOrUpdate(configs);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }



        /// <summary>
        ///  删除硬件配置
        /// </summary>
        /// <param name="mDeviceID"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult DeleteDeviceConfig(string mDeviceID)
        {
            try
            {
                var db = McsDbTool.GetDBRepositoryRef<DeviceConfig>();
                db.DeleteById(mDeviceID);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 设备重启动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<McsWebApiResult> DeviceReboot()
        {
            try
            {
                var mresult = await device.DeviceCreate();
                if (mresult)
                {
                    return SuccessResult();
                }
                else
                {
                    return ErrorResult("系统硬件重启失败");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }
    }
}