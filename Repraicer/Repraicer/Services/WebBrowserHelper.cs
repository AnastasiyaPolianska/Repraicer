﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Repraicer.Services
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.RegisterAttached("Url", typeof(string), typeof(WebBrowserHelper),
                new PropertyMetadata(OnUrlChanged));

        public static string GetUrl(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(UrlProperty);
        }

        public static void SetUrl(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(UrlProperty, body);
        }

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is WebBrowser browser)) return;

            Uri uri = null;

            var s = e.NewValue as string;

            if (s != null)
            {
                var uriString = s;

                uri = string.IsNullOrWhiteSpace(uriString) ? null : new Uri(uriString);
            }
            else if (e.NewValue is Uri)
            {
                uri = (Uri)e.NewValue;
            }

            browser.Source = uri;
        }
    }
}