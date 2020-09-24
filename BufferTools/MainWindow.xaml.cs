using CLUNL.Data.Layer0.Buffers.UITool;
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

namespace BufferTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BBT_Add_Click(object sender, RoutedEventArgs e)
        {
            ByteBufferItem baseBufferItem = new ByteBufferItem();
            ByteBufferViewPanel.Children.Add(baseBufferItem);
        }
    }
}
