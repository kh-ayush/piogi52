using piogi52.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MainGame.Classes
{
    public interface IEnemy
    {
        string Name { get; }
        CBigNum MaxHitPoints { get; }
        CBigNum CurrentHitPoints { get; }
        CBigNum GoldReward { get; }
        bool IsDead { get; }
        string Icon { get; }
        bool TakeDamage(CBigNum dmg, out CBigNum goldReward);
        void RecalculateStats(CEnemyTemplate enemyTemplate, int lvl);
    }

    public class CBasicEnemy : CEnemy 
    {
        public CBasicEnemy() : base() { }
        public CBasicEnemy(CBasicEnemyTemplate template) : base(template) { }
    }
    public class CArmoredEnemy : CEnemy 
    {
        public double Armor { get; private set; }

        public CArmoredEnemy() : base() { }

        public CArmoredEnemy(CArmoredEnemyTemplate template) : base(template)
        {
            this.Armor = template.Armor;
        }
        public override bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            CBigNum reducedDamage = dmg * (1 - Armor / 100);
            return base.TakeDamage(reducedDamage, out goldReward);
        }
    }
    public class CBoberEnemy : CEnemy 
    {
        public double DodgeChance { get; private set; }
        private Random random = new Random();

        public CBoberEnemy() : base() { }

        public CBoberEnemy(CBoberEnemyTemplate template) : base(template)
        {
            this.DodgeChance = template.Bober;
        }

        public override bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            if (random.NextDouble() < DodgeChance)
            {
                goldReward = new CBigNum("0");
                MessageBox.Show("Attack is Dodged!");
                return false;
            }

            return base.TakeDamage(dmg, out goldReward);
        }
    }
    public class CMedEnemy : CEnemy 
    {
        public double Heal { get; private set; }

        public CMedEnemy() : base() { }

        public CMedEnemy(CMedEnemyTemplate template) : base(template)
        {
            this.Heal = template.Heal;
        }

        private void HealSelf()
        {
            CBigNum healAmount = MaxHitPoints * (1 - Heal/100);
            CurrentHitPoints += healAmount;
            if (CurrentHitPoints > MaxHitPoints) CurrentHitPoints = MaxHitPoints;
            MessageBox.Show("Enemy Healed!");
        }

        public override bool TakeDamage(CBigNum dmg, out CBigNum goldReward)
        {
            Random rr = new Random();

            if (rr.Next(2) == 1 && CurrentHitPoints < MaxHitPoints) HealSelf();

            return base.TakeDamage(dmg, out goldReward);
        }
    }

}
