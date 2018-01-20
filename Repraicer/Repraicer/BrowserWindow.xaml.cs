using System.Windows;
using Repraicer.ViewModels;

namespace Repraicer
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window
    {
        private readonly BrowserWndowViewModel _vm = BrowserWndowViewModel.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserWindow"/> class.
        /// </summary>
        public BrowserWindow()
        {
            DataContext = _vm;
            InitializeComponent();
        }
    }
}
