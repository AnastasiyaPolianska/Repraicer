using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Repraicer.Model;
using Repraicer.ViewModels;

namespace Repraicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm = new MainWindowViewModel();
        public MainWindow()
        {
            this.DataContext = vm;
            InitializeComponent();
        }

        /// <summary>
        /// This should not be nessecary, but could not get wrapper to update Binding when
        /// SelectedItem changed...Tried lots of stuff, this is hack, but ran out of steam.
        /// </summary>
        private void CarouselControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                vm.CurrentData = (Product)e.AddedItems[0];
            }
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
