using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
    public class ByteBufferItem : BaseBufferItem
    {
        public ByteBufferItem()
        {
            DefaultEditor.BorderThickness = new Thickness(2);
            DefaultEditor.Padding = new Thickness(8,4,8,4);
            this.MinWidth = 100;
            Description.Text = "Byte[]";
        }
    }
}