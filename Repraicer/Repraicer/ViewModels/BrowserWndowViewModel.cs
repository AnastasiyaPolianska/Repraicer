using System;

namespace Repraicer.ViewModels
{
    public class BrowserWndowViewModel : INPCBase
    {
        private Uri _productUri = null;
        private static BrowserWndowViewModel _instance;
        public static BrowserWndowViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BrowserWndowViewModel();
                }
                return _instance;
            }
        }
        
        public Uri ProductUri
        {
            get { return _productUri; }
            set
            {
                _productUri = value;
                NotifyPropertyChanged(nameof(ProductUri));
            }
        }
    }
}
