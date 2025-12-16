using piogi52.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static piogi52.MainWindow;

namespace MainGame.Classes
{
    public abstract class CEnemy : INotifyPropertyChanged, IEnemy
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string name;
        private CBigNum maxHitPoints;
        private CBigNum currentHitPoints;
        private CBigNum goldReward;
        private bool isDead;
        private string icon;
        public string Name
        {
            get => name;
            private set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public CBigNum MaxHitPoints
        {
            get => maxHitPoints;
            private set => maxHitPoints = value;
        }
        public CBigNum CurrentHitPoints
        {
            get => currentHitPoints;
            set
            {
                currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public CBigNum GoldReward
        {
            get => goldReward;
            private set
            {
                goldReward = value;
                OnPropertyChanged();
            }
        }
        public bool IsDead
        {
            get => isDead;
            private set => isDead = value;
        }
        public string Icon
        {
            get => icon;
            private set
            {
                icon = value;
                OnPropertyChanged();
            }
        }
        protected CEnemy() { }
        public CEnemy(string name, CBigNum maxHitPoints, CBigNum goldReward, string icon)
        {
            Name = name;
            MaxHitPoints = maxHitPoints;
            CurrentHitPoints = maxHitPoints;
            GoldReward = goldReward;
            IsDead = false;
            Icon = icon;
        }
        public CEnemy(CEnemyTemplate enemyTemplate)
        {
            Name = enemyTemplate.Name;
            MaxHitPoints = new CBigNum(Convert.ToString(enemyTemplate.BaseLife));
            CurrentHitPoints = MaxHitPoints;
            GoldReward = new CBigNum(Convert.ToString(enemyTemplate.BaseGold));
            IsDead = false;
            Icon = enemyTemplate.IconPath;
        }
        public virtual bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            goldReward = new CBigNum("0");

            if (IsDead) return false;
            if (dmg > CurrentHitPoints) dmg = CurrentHitPoints;

            CurrentHitPoints -= dmg;

            if (CurrentHitPoints == new CBigNum("0"))
            {
                IsDead = true;
                goldReward = GoldReward;
            }
            return IsDead;
        }
        public void RecalculateStats(CEnemyTemplate enemyTemplate, int lvl)
        {
            MaxHitPoints = MaxHitPoints * (enemyTemplate.LifeModifier * lvl);
            CurrentHitPoints = MaxHitPoints;
            GoldReward = GoldReward * (enemyTemplate.GoldModifier * lvl);
        }
    }

    public class EnemyFactory
    {
        public static IEnemy CreateEnemy(CEnemyTemplate template)
        {
            if (template == null)
                return null;

            return template switch
            {
                CBasicEnemyTemplate basic => new CBasicEnemy(basic),
                CArmoredEnemyTemplate armored => new CArmoredEnemy(armored),
                CMedEnemyTemplate medic => new CMedEnemy(medic),
                CBoberEnemyTemplate bober => new CBoberEnemy(bober),
            };
        }

        //public static IEnemy CreateEnemy(string typeName, params object[] args)
        //{
        //    // Находим тип по названию
        //    var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == typeName);

        //    // Создаем объект с передачей параметров в конструктор
        //    return (IEnemy)Activator.CreateInstance(type, args);
        //}
    }
}