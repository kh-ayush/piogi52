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

        private DispatcherTimer timer;
        private CController controller;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += UpdateGame;

            Start_Click();
            GameCanvas.MouseLeftButtonDown += GameCanvas_MouseLeftButtonDown;

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
            Player.TryUpgrade();
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
        private void Start_Click()
        {
            GameCanvas.Children.Clear();

            controller = new CController(
                spawnRate: 1,
                startTime: 0,
                sceneSize: new System.Drawing.Size(250, 250)
            );

            timer.Start();
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(GameCanvas);
            System.Drawing.Point pt = new System.Drawing.Point((int)pos.X, (int)pos.Y);

            CObject hit = controller.mouseClick(pt);

            if (hit != null)
            {
                GameCanvas.Children.Remove(hit.Sprite);
            }
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            double delta = 0.016;
            //gameTimeLeft -= delta;

            //if (gameTimeLeft <= 0)
            //{
            //    timer.Stop();
            //    GameCanvas.Children.Clear();
            //    return;
            //}

            controller.update(delta);

            GameCanvas.Children.Clear();

            foreach (var obj in controller.Objects)
                GameCanvas.Children.Add(obj.Sprite);
        }
    }
}