using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Classes
{
    public class CPlayer
    {
        public int Lvl { get; private set; }
        public CBigNum Gold { get; private set; }
        public CBigNum Damage { get; private set; }
        public double DamageModifier { get; private set; }
        public CBigNum UpgradeCost { get; private set; }
        public double UpgradeModifier { get; private set; }

        public CPlayer()
        {
            Lvl = 1;
            Gold = new CBigNum(0);
            Damage = new CBigNum(10);
            DamageModifier = 1.0;
            UpgradeCost = new CBigNum(100);
            UpgradeModifier = 1.15;
        }


        public void AddGold(CBigNum amount)
        {
            Gold = Gold + amount;
        }

        public bool TryUpgrade()
        {
            if (TrySpendGold(UpgradeCost))
            {
                Lvl++;
                RecalculateStats();
                return true;
            }
            return false;
        }

        public bool TrySpendGold(CBigNum amount)
        {
            if (Gold > amount || Gold == amount) 
            {
                Gold = Gold - amount;
                return true;
            }
            return false;
        }

        public void RecalculateStats()
        {
            Damage = CalculateTotalDamage();
            UpgradeCost = CalculateNextUpgradeCost();
        }

        public CBigNum CalculateTotalDamage()
        {
            return Damage * (DamageModifier * Lvl);
        }
        public CBigNum CalculateNextUpgradeCost()
        {
            return UpgradeCost * UpgradeModifier;
        }
        public CBigNum DealDamage()
        {
            return CalculateTotalDamage();
        }
    }
}