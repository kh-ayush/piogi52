using MainGame.Classes;
using piogi52.Classes;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Threading;

namespace MainGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void normalizeChances()
        {
            double sum = 0;
            for (int i = 0; i < enemyTemps.enemies.Count; i++)
                sum += enemyTemps.enemies[i].SpawnChance;
            for (int i = 0; i < enemyTemps.enemies.Count; i++)
                enemyTemps.enemies[i].SpawnChance /= sum;
        }
        public CEnemyTemplate findByChance(double chance)
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
        public IEnemy CurrentEnemy;
        public CPlayer Player;

        private DispatcherTimer timer;
        private CController controller;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += UpdateGame;

            controller = new CController(
                spawnRate: 1,
                startTime: 0,
                sceneSize: new System.Drawing.Size(250, 250)
            );

            enemyTemps = new CEnemyTemplateList();
            enemyTemps.LoadJson();
            normalizeChances();
            CurrentTemplate = findByChance(rand.NextDouble());
            CurrentEnemy = EnemyFactory.CreateEnemy(CurrentTemplate);
            

            NextButton.IsEnabled = false;
            RepeatButton.IsEnabled = false;

            EnemyInfo.DataContext = CurrentEnemy;

            Player = new CPlayer(
                1,                    //lvl
                new CBigNum("0"),     //gold
                new CBigNum("2"),     //damage
                1.2,                  //dmgMod
                new CBigNum("10"),    //upgradeCost
                1.2 );                 //upgradeMod
            
            PlayerInfo.DataContext = Player;

            timer.Start();
        }
        private void Attack(object sender, MouseButtonEventArgs e)
        {
            if (Player.IsCD)
            {
                var pos = e.GetPosition(GameCanvas);
                System.Drawing.Point pt = new System.Drawing.Point((int)pos.X, (int)pos.Y);

                CObject hit = controller.mouseClick(pt);
                if (hit != null) 
                {
                    hit.ApplyBonus(Player);
                    GameCanvas.Children.Remove(hit.Sprite); 
                }
                Player.IsCD = false;

                CBigNum reward;
                if (CurrentEnemy.TakeDamage(Player.DealDamage(), out reward))
                {
                    timer.Stop();

                    Player.AddGold(reward);
                    Player.EnemyCount++;
                    NextButton.IsEnabled = true;
                    RepeatButton.IsEnabled = true;

                    controller.Objects.Clear();
                    GameCanvas.Children.Clear();
                }
            }
        }
        private void UpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            Player.TryUpgradeDM();
        }
        private void UpgradeCD_Click(object sender, RoutedEventArgs e)
        {
            Player.TryUpgradeCD();
        }
        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEnemy.IsDead)
            {
                CurrentEnemy.RecalculateStats(CurrentTemplate, Player.EnemyCount);
                EnemyInfo.DataContext = CurrentEnemy;
                CurrentEnemy = EnemyFactory.CreateEnemy(CurrentTemplate);
                NextButton.IsEnabled = false;
                RepeatButton.IsEnabled = false;

                timer.Start();
            }
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEnemy.IsDead)
            {
                CurrentTemplate = findByChance(rand.NextDouble());
                CurrentEnemy = EnemyFactory.CreateEnemy(CurrentTemplate);
                CurrentEnemy.RecalculateStats(CurrentTemplate, Player.EnemyCount);
                EnemyInfo.DataContext = CurrentEnemy;
                NextButton.IsEnabled = false;
                RepeatButton.IsEnabled = false;

                timer.Start();
            }
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            double delta = 0.016;

            Player.Update(delta);
            controller.update(delta);

            GameCanvas.Children.Clear();

            foreach (var obj in controller.Objects) GameCanvas.Children.Add(obj.Sprite);
        }

        private void LoadGame(object sender, RoutedEventArgs e)
        {
            Player.LoadPlayer();
            CurrentTemplate = findByChance(rand.NextDouble());
            CurrentEnemy = EnemyFactory.CreateEnemy(CurrentTemplate);
            CurrentEnemy.RecalculateStats(CurrentTemplate, Player.EnemyCount);
            EnemyInfo.DataContext = CurrentEnemy;
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            Player.SavePlayer();
        }
    }
}