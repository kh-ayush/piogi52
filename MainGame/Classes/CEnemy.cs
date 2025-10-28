using piogi52.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static piogi52.MainWindow;

namespace MainGame.Classes
{
    public class CEnemy : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string name;
        private CBigNum maxHitPoints;
        private CBigNum curretnHitPoints;
        private CBigNum goldReward;
        private bool isDead;
        private string icon;
        public string Name { 
            get => name;
            private set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public CBigNum MaxHitPoints { 
            get => maxHitPoints;
            private set => maxHitPoints = value;
        }   
        public CBigNum CurrentHitPoints { 
            get => curretnHitPoints;
            private set
            {
                curretnHitPoints = value;
                OnPropertyChanged("CurretnHitPoints");
            }
        }
        public CBigNum GoldReward {
            get => goldReward;
            private set
            {
                goldReward = value;
                OnPropertyChanged("GoldReward");
            }
        }
        public bool IsDead {
            get => isDead;
            private set => isDead = value; 
        }
        public string Icon {
            get => icon;
            private set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }
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
        public bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            goldReward = new CBigNum("0");

            if (IsDead) return false;
            if (dmg > CurrentHitPoints) dmg = CurrentHitPoints;

            CurrentHitPoints = CurrentHitPoints - dmg;

            if (CurrentHitPoints == new CBigNum("0"))
            {
                Die();
                goldReward = GoldReward;
                return true;
            }
            return false;
        }
        private void Die()
        {
            IsDead = true;
            CurrentHitPoints = new CBigNum("0");
        }
    }
}