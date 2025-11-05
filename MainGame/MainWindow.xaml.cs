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
using System.Collections.ObjectModel;

namespace MainGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void normalizeChances() //нормализация шансов выбора объектов, сумма шансов
                                //должна быть равна 1
        {
            double sum = 0;
            for (int i = 0; i < enemyTemps.enemies.Count; i++)
                sum += enemyTemps.enemies[i].SpawnChance;
            for (int i = 0; i < enemyTemps.enemies.Count; i++)
                enemyTemps.enemies[i].SpawnChance /= sum;
        }
        public CEnemyTemplate findByChance(double chance) //поиск объекта по выпавшей вероятности
        {
            double sum = 0;
            for (int i = 0; i < enemyTemps.enemies.Count; i++)
            {
                sum += enemyTemps.enemies[i].SpawnChance;
                if (sum >= chance) return enemyTemps.enemies[i];
            }
            return null;
        }
        public CEnemyTemplateList enemyTemps;
        public CEnemyTemplate CurrentTemplate;
        public Random rand = new Random();
        public CEnemy CurrentEnemy;
        public CPlayer Player;
        public int EnemyCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            enemyTemps = new CEnemyTemplateList();
            enemyTemps.LoadJson();
            normalizeChances();
            CurrentTemplate = findByChance(rand.NextDouble());
            CurrentEnemy = new CEnemy(CurrentTemplate);
            EnemyCount = 0;

            NextButton.IsEnabled = false;
            RepeatButton.IsEnabled = false;

            EnemyInfo.DataContext = CurrentEnemy;

            Player = new CPlayer(
                1,                    //lvl
                new CBigNum("0"),     //gold
                new CBigNum("2"),     //damage
                1.2,                  //dmgMod
                new CBigNum("10"),    //upgradeCost
                1.2);                 //upgradeMod
            
            PlayerInfo.DataContext = Player;
        }
        private void Attack(object sender, MouseButtonEventArgs e)
        {
            CBigNum reward;
            if (CurrentEnemy.TakeDamage(Player.DealDamage(), out reward))
            {
                Player.AddGold(reward);
                EnemyCount++;
                NextButton.IsEnabled = true;
                RepeatButton.IsEnabled = true;
            }
        }
        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Player.TryUpgrade()) MessageBox.Show("^ LEVEL UP ^"); 
        }
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEnemy.IsDead)
            {
                CurrentEnemy = new CEnemy(CurrentTemplate);
                CurrentEnemy.RecalculateStats(CurrentTemplate, EnemyCount);
                EnemyInfo.DataContext = CurrentEnemy;
                NextButton.IsEnabled = false;
                RepeatButton.IsEnabled = false;
            }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEnemy.IsDead)
            {
                CurrentTemplate = findByChance(rand.NextDouble());
                CurrentEnemy = new CEnemy(CurrentTemplate);
                CurrentEnemy.RecalculateStats(CurrentTemplate, EnemyCount);
                EnemyInfo.DataContext = CurrentEnemy;
                NextButton.IsEnabled = false;
                RepeatButton.IsEnabled = false;
            }
        }
    }
}