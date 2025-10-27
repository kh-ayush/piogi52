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

            //MessageBox.Show((new CBigNum("3712117") + new CBigNum("15512900")).ToString()); //19225017
            //MessageBox.Show((new CBigNum("3712117") - new CBigNum("1911900")).ToString());  //1800217
            //MessageBox.Show((new CBigNum("10014217") / 3).ToString());                      //3338072
            //MessageBox.Show((new CBigNum("7135654") * 23).ToString());                      //164120042

        }
        private void Attack(object sender, MouseButtonEventArgs e)
        {

        }
    }
}