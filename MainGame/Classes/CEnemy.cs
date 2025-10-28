using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static piogi52.MainWindow;

namespace MainGame.Classes
{
    public class Enemy
    {
        public string Name { get; private set; }
        public CBigNum MaxHitPoints { get; private set; }
        public CBigNum CurrentHitPoints { get; private set; }
        public CBigNum GoldReward { get; private set; }
        public bool IsDead { get; private set; }
        public IconItem Icon { get; private set; }
        public Enemy(string name, CBigNum maxHitPoints, CBigNum goldReward, IconItem icon = null)
        {
            Name = name;
            MaxHitPoints = maxHitPoints;
            CurrentHitPoints = new CBigNum(maxHitPoints); 
            GoldReward = goldReward;
            IsDead = false;
            Icon = icon;
        }
        public bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            goldReward = new CBigNum(0);

            if (IsDead)
                return false;
            if (dmg > CurrentHitPoints)
                dmg = CurrentHitPoints;

            CurrentHitPoints = CurrentHitPoints - dmg;

            if (CurrentHitPoints == new CBigNum(0))
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
            CurrentHitPoints = new CBigNum(0);
        }
    }
}