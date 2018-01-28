using Repraicer.Model;
using Repraicer.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Repraicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm = new MainWindowViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            DataContext = _vm;
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
                _vm.CurrentData = (Product)e.AddedItems[0];
            }
        }

        /// <summary>
        /// Handles the OnPreviewTextInput event of the UIElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !IsTextAllowed(e.Text);

        /// <summary>
        /// Determines whether [is text allowed] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> if [is text allowed] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}