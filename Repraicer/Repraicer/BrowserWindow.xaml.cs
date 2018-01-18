using System.Windows;
using Repraicer.ViewModels;

namespace Repraicer
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window
    {
        private BrowserWndowViewModel vm = BrowserWndowViewModel.Instance;
        public BrowserWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
