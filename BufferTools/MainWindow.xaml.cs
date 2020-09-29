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

        private void BBT_Add_Debug_Random_Click(object sender, RoutedEventArgs e)
        {

            ByteBufferItem bufferItem = new ByteBufferItem();
            bufferItem.ByteGroup = RandomArray();
            ByteBufferViewPanel.Children.Add(bufferItem);
        }
        byte[] RandomArray()
        {
            Random random = new Random();
            var length=random.Next(1, 16);
            byte[] b= new byte[length];
            for (int i = 0; i < length; i++)
            {
                b[i] = (byte)random.Next(byte.MinValue, byte.MaxValue);
            }
            return b;
        }

        private void Settings_Tab_Click(object sender, RoutedEventArgs e)
        {
            DisableAll();
            Settings.Visibility = Visibility.Visible;
        }
        void DisableAll()
        {
            foreach (var item in ByteBufferView.Children)
            {
                (item as UIElement).Visibility = Visibility.Collapsed;
            }
        }
        private void BBT_Tab_Click(object sender, RoutedEventArgs e)
        {

            DisableAll();
            ByteBufferToolView.Visibility = Visibility.Visible;
        }

        private void DBT_Tab_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TDBT_Tab_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
