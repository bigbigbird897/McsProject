

using Microsoft.Extensions.Configuration;

namespace UIWpf.ViewModels
{
    public class AppMainUIViewModel : BindableBase
    {
        private string mbaseUrl;

        public string BaseUrl
        {
            get { return mbaseUrl; }
            set { SetProperty(ref mbaseUrl, value); }
        }

        public AppMainUIViewModel(IConfiguration configuration)
        {
            BaseUrl = configuration.GetValue<string>("BaseUrl");
        }
    }
}