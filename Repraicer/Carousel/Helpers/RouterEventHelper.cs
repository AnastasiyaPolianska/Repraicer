﻿using System;
using System.Windows;

namespace Carousel.Helpers
{
    internal static class RoutedEventHelper
    {
        #region RoutedEvent Helper Methods

        /// <summary>
        /// A static helper method to raise a routed event on a target UIElement or ContentElement.
        /// </summary>
        /// <param name="target">UIElement or ContentElement on which to raise the event</param>
        /// <param name="args">RoutedEventArgs to use when raising the event</param>
        internal static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
        {
            switch (target)
            {
                case UIElement uiElement:
                    {
                        uiElement.RaiseEvent(args);
                    }
                    break;

                case ContentElement contentElement:
                    {
                        contentElement.RaiseEvent(args);
                    }
                    break;
            }
        }

        /// <summary>
        /// A static helper method that adds a handler for a routed event 
        /// to a target UIElement or ContentElement.
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to the event</param>
        /// <param name="routedEvent">Event that will be handled</param>
        /// <param name="handler">Event handler to be added</param>
        internal static void AddHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            switch (element)
            {
                case UIElement uiElement:
                    {
                        uiElement.AddHandler(routedEvent, handler);
                    }
                    break;

                case ContentElement contentElement:
                    {
                        contentElement.AddHandler(routedEvent, handler);
                    }
                    break;
            }
        }

        /// <summary>
        /// A static helper method that removes a handler for a routed event 
        /// from a target UIElement or ContentElement.
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to the event</param>
        /// <param name="routedEvent">Event that will no longer be handled</param>
        /// <param name="handler">Event handler to be removed</param>
        internal static void RemoveHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            switch (element)
            {
                case UIElement uiElement:
                    {
                        uiElement.RemoveHandler(routedEvent, handler);
                    }
                    break;

                case ContentElement contentElement:
                    {
                        contentElement.RemoveHandler(routedEvent, handler);
                    }
                    break;
            }
        }

        #endregion
    }
}