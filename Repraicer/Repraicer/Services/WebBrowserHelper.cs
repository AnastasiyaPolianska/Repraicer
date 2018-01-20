using System;
using System.Windows;
using System.Windows.Controls;

namespace Repraicer.Services
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.RegisterAttached(
                "Url", 
                typeof(string), 
                typeof(WebBrowserHelper),
                new PropertyMetadata(OnUrlChanged));

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns></returns>
        public static string GetUrl(DependencyObject dependencyObject) => (string)dependencyObject.GetValue(UrlProperty);

        /// <summary>
        /// Sets the URL.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="body">The body.</param>
        public static void SetUrl(DependencyObject dependencyObject, string body) => dependencyObject.SetValue(UrlProperty, body);

        /// <summary>
        /// Called when [URL changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is WebBrowser browser)) return;

            Uri uri = null;

            //var s = e.NewValue as string;

            //if (s != null)
            //{
            //    var uriString = s;

            //    uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            //}
            //else if (e.NewValue is Uri)
            //{
            //    uri = (Uri)e.NewValue;
            //}

            //used pattern matching instead of previous code. More readable, faster.
            switch (e.NewValue)
            {
                case string s:
                {
                    var uriString = s;

                    uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
                }
                    break;

                case Uri _:
                {
                    uri = (Uri)e.NewValue;
                }
                    break;
            }

            browser.Source = uri;
        }
    }
}