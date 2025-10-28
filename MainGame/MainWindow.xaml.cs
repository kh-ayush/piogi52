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
        Random rand = new Random();
        CEnemy CurrentEnemy;
        CPlayer Player;
        
        public MainWindow()
        {
            InitializeComponent();

            enemyTemps = new CEnemyTemplateList();
            enemyTemps.LoadJson();
            normalizeChances();
            CurrentTemplate = findByChance(rand.NextDouble());
            CurrentEnemy = new CEnemy(CurrentTemplate);

            EnemyInfo.DataContext = CurrentEnemy;

            Player = new CPlayer(
                1,                    //lvl
                new CBigNum("0"),     //gold
                new CBigNum("1"),     //damage
                1.2,                  //dmgMod
                new CBigNum("10"),    //upgradeCost
                1.2);                 //upgradeMod
            
            PlayerInfo.DataContext = Player;
        }
        private void Attack(object sender, MouseButtonEventArgs e)
        {
            CBigNum reward;
            CurrentEnemy.TakeDamage(Player.Damage, out reward);
            Player.AddGold(reward);

        }
        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            Player.TryUpgrade();
        }
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentEnemy = new CEnemy(CurrentTemplate);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEnemy != null & CurrentEnemy.IsDead)
            {
                CurrentTemplate = findByChance(rand.NextDouble());
                CurrentEnemy = new CEnemy(CurrentTemplate);
            }
        }
    }
}