using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CourseDesign.Extensions
{
    /// <summary>
    /// 用于在登录窗口的密码，进行数据绑定的类
    /// </summary>
    public class PasswordExtension
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordExtension),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged) // 回调函数
                );

        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// 属性变更时触发的事件
        /// </summary>
        /// <param name="s">回调对象</param>
        /// <param name="e">事件接收对象</param>
        static void OnPasswordPropertyChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = s as PasswordBox;
            string password = (string)e.NewValue;

            if (passwordBox != null && passwordBox.Password != password)
                passwordBox.Password = password;
        }
    }

    /// <summary>
    /// 行为
    /// </summary>
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged; // 附加事件
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged; // 注销事件
        }

        /// <summary>
        /// 当值改变时，处理的逻辑
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void AssociatedObject_PasswordChanged(object s, RoutedEventArgs e)
        {
            PasswordBox passwordBox = s as PasswordBox;
            string password = PasswordExtension.GetPassword(passwordBox);

            if (passwordBox != null && passwordBox.Password != password)
                PasswordExtension.SetPassword(passwordBox, passwordBox.Password);
        }
    }

}
