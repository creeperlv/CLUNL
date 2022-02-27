using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CLUNL.Data.Layer0.Buffers.UITool
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BaseBufferItem : UserControl
    {
        public BaseBufferItem()
        {
            InitializeComponent();
        }
        public virtual byte[] ByteGroup { get; set; }
    }
    public class ByteBufferItem : BaseBufferItem
    {
        public ByteBufferItem()
        {
            this.ToolTip = new ToolTip() { Content = new TextBlock() { Text = "Only HEX Strings are accepted." } };
            this.Margin = new Thickness(2);
            this.MinWidth = 100;
            Description.Text = "byte[]";
            DefaultEditor.KeyDown += DefaultEditor_KeyDown;
        }
        public override byte[] ByteGroup
        {
            get
            {
                try
                {
                    return ByteUtilities.StringToByteArray(DefaultEditor.Text);
                }
                catch (global::System.Exception)
                {
                }
                return null;
            }
            set { DefaultEditor.Text = ByteUtilities.ByteArrayToString(value); }
        }
        private void DefaultEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab) return;
            e.Handled = "0123456789ABCDEF".IndexOf(char.ToUpper(e.Key.ToString()[0])) < 0;
        }
    }
    public class ByteUtilities
    {
        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            hex = hex.Replace(" ", "");
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}