using System;

namespace Repraicer.ViewModels
{
    public class BrowserWndowViewModel : InpcBase
    {
        private Uri _productUri;

        private static BrowserWndowViewModel _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>

        //clearer way
        public static BrowserWndowViewModel Instance => _instance ?? (_instance = new BrowserWndowViewModel());

        /// <summary>
        /// Gets or sets the product URI.
        /// </summary>
        /// <value>
        /// The product URI.
        /// </value>
        public Uri ProductUri
        {
            get => _productUri;
            set
            {
                _productUri = value;
                NotifyPropertyChanged(nameof(ProductUri));
            }
        }
    }
}