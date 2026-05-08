using CoreServiceLib.Core.IocExt;

using NLog;

namespace UIWpf.ViewModels
{
    public class AppMenuViewModel : BindableBase
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public DelegateCommand MaterialManagerUI { get; private set; }
        public DelegateCommand DeviceManagerUI { get; private set; }
        public DelegateCommand OpenStationConfigUI { get; }
        public DelegateCommand OpenToolConfigUI { get; }
        public DelegateCommand OpenPosCalUI { get; }
        public DelegateCommand AuthorityMUI { get; }
        public DelegateCommand UserMUI { get; }
        public DelegateCommand LoginUI { get; }

        public DelegateCommand FixGroupUI { get; }
        public DelegateCommand FlowConfigUI { get; }
        public DelegateCommand SubjectConfigUI { get; }
        public DelegateCommand OpenSysConfigUI { get; }
        public DelegateCommand OpenVisionDebug { get; }
        public DelegateCommand ManulControl { get; }
        public DelegateCommand QueHistory { get; }

        public AppMenuViewModel()
        {
            MaterialManagerUI = new DelegateCommand(DoMaterial);
            DeviceManagerUI = new DelegateCommand(DoDevice);
            OpenStationConfigUI = new DelegateCommand(DoOpenStationConfig);
            OpenToolConfigUI = new DelegateCommand(DoOpenToolConfigUI);
            OpenPosCalUI = new DelegateCommand(DoOpenPosCalUI);
            LoginUI = new DelegateCommand(DoLogin);
            SubjectConfigUI = new DelegateCommand(DoConfigSubject);
            FlowConfigUI = new DelegateCommand(DoFlowConfigUI);
            FixGroupUI = new DelegateCommand(DoFixGroupUI);
            UserMUI = new DelegateCommand(DoUserM);
            AuthorityMUI = new DelegateCommand(DoAuthM);
            OpenSysConfigUI = new DelegateCommand(DoSysConfig);
            OpenVisionDebug = new DelegateCommand(DoVisionDebug);
            ManulControl = new DelegateCommand(DoManulOpen);
            QueHistory = new DelegateCommand(DoQueHistory);
        }

        private void DoQueHistory()
        {
            try
            {
                McsApp.GetDiag("HistoryQueUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoManulOpen()
        {
            try
            {
                McsApp.GetDiag("ManulControlUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoVisionDebug()
        {
            try
            {
                McsApp.GetDiag("VisionDebugUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoSysConfig()
        {
            try
            {
                McsApp.GetDiag("SysConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoAuthM()
        {
            try
            {
                McsApp.GetDiag("AuthorityConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoUserM()
        {
            try
            {
                McsApp.GetDiag("UserConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoFixGroupUI()
        {
            try
            {
                McsApp.GetDiag("FixtureGroupConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoFlowConfigUI()
        {
            try
            {
                McsApp.GetDiag("RecipeConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoConfigSubject()
        {
            try
            {
                McsApp.GetDiag("SubjectConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoLogin()
        {
            try
            {
                McsApp.GetDiag("UserLoginUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoOpenPosCalUI()
        {
            try
            {
                McsApp.GetDiag("ManulCalUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoOpenToolConfigUI()
        {
            try
            {
                McsApp.GetDiag("ToolPosConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoOpenStationConfig()
        {
            try
            {
                McsApp.GetDiag("LineConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoDevice()
        {
            try
            {
                McsApp.GetDiag("DeviceConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void DoMaterial()
        {
            try
            {
                McsApp.GetDiag("MaterialConfigUI")?.ShowDialogUI();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}