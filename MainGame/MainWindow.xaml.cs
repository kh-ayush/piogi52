using piogi52.Classes;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using MainGame.Classes;

namespace MainGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CBigNum a = new CBigNum("111111900");
            CBigNum b = new CBigNum("111111100");
            MessageBox.Show(a.Add(b).ToString());

        }
        private void Attack(object sender, MouseButtonEventArgs e)
        {

        }
    }
}