using HitbotMqtt.Paras;

using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Tools;

using CoreWpfLib.WpfEx;

using Newtonsoft.Json.Linq;

using System.Windows;

namespace UIWpf.ViewModels
{
    public class MqttConfigViewModel : BindableBase
    {
        private HitbotMqttPara mqttPara;

        public HitbotMqttPara Para
        {
            get { return mqttPara; }
            set { SetProperty(ref mqttPara, value); }
        }

        public DelegateCommand<Window> DataSave { get; private set; }
        public DelegateCommand<Window> DataCancel { get; private set; }

        public MqttConfigViewModel()
        {
            var x = McsApp.GetParaData<JObject>();
            if (x != null)
            {
                Para = JsonTool.JsonObjectToObject<HitbotMqttPara>(x);
            }
            else
            {
                Para = new HitbotMqttPara();
            }

            DataSave = new DelegateCommand<Window>(DoSave);
            DataCancel = new DelegateCommand<Window>(DoCancel);
        }

        private void DoSave(Window window)
        {
            McsApp.SetParaData(Para);
            McsDiag.OkMessage("数据保存完成");
            window.Close();
        }

        private void DoCancel(Window window)
        {
            window.Close();
        }
    }
}