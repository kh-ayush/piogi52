using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Classes
{
    public class Enemy
    {
        public string Name { get; private set; }
        public int Lvl { get; private set; }
        public CBigNum Health { get; private set; }
        public CBigNum MaxHealth { get; private set; }
        public CBigNum Reward { get; private set; }
        public CBigNum Damage { get; private set; }
        public double HealthModifier { get; private set; }
        public double RewardModifier { get; private set; }
        public Enemy(string name, int lvl = 1)
        {
            Name = name;
            Lvl = lvl;

            MaxHealth = new CBigNum(50);
            Health = new CBigNum(50);
            Damage = new CBigNum(5);
            Reward = new CBigNum(20);

            HealthModifier = 1.25;
            RewardModifier = 1.15;
        }

        public void TakeDamage(CBigNum amount)
        {
            if (amount > Health) amount = Health;
            Health = Health - amount;
        }
        public bool IsDead()
        {
            return Health == new CBigNum(0);
        }

        public CBigNum GetReward()
        {
            return Reward;
        }

        public void Respawn()
        {
            Lvl++;
            RecalculateStats();
            Health = MaxHealth;
        }

        private void RecalculateStats()
        {
            MaxHealth = MaxHealth * HealthModifier;
            Reward = Reward * RewardModifier;
            Damage = Damage * 1.1;
        }
    }
}

