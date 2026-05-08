using CoreServiceLib.Core.Extensions.McsLogger;
using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Core.McsEvent;

using Microsoft.Extensions.Configuration;

using Prism.Events;
using Prism.Navigation.Regions;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Host.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ObservableCollection<SysMessage> SysMessages { get; } = [];

        public bool IsChanged { get; set; }

        private string mSysTitle;

        public string SysTitle
        {
            get { return mSysTitle; }
            set { SetProperty(ref mSysTitle, value); }
        }

        public MainWindowViewModel(IRegionManager region, IConfiguration config)
        {
            try
            {
                SysTitle = config.GetValue<string>("LineName");
                McsEventMessage.Instance.Subscribe<List<SysMessage>>(GetMessage, ThreadOption.UIThread);
                McsToRegion.Regist(region);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void GetMessage(List<SysMessage> list)
        {
            SysMessages.Clear();
            SysMessages.AddRange(list);
        }
    }
}