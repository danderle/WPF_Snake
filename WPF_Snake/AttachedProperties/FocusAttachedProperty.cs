using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Snake.AttachedProperties
{
    public class FocusAttachedProperty : UIElement
    {
        public static readonly DependencyProperty FocusProperty = DependencyProperty.RegisterAttached(
            "Focus",
            typeof(bool),
            typeof(FocusAttachedProperty),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnValuePropertyChanged)));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as TextBox;
            if (target != null && (bool)e.NewValue)
            {
                target.Focus();
                Keyboard.Focus(target);
            }
        }

        public static bool GetFocus(UIElement target) => (bool)target.GetValue(FocusProperty);

        public static void SetFocus(UIElement target, bool value) => target.SetValue(FocusProperty, value);
    }
}
