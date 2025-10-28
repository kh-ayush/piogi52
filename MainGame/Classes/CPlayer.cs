using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainGame.Classes
{
    public class CPlayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int lvl;
        private CBigNum gold;
        private CBigNum damage;
        private double damageModifier;
        private CBigNum upgradeCost;
        private double upgradeModifier;

        public int Lvl {
            get => lvl;
            private set
            {
                lvl = value;
                OnPropertyChanged("Lvl");
            }
        }
        public CBigNum Gold {
            get => gold;
            private set
            {
                gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public CBigNum Damage {
            get => damage;
            private set
            {
                damage = value;
                OnPropertyChanged("Damage");
            }
        }
        public double DamageModifier {
            get => damageModifier;
            private set => damageModifier = value;
        }
        public CBigNum UpgradeCost {
            get => upgradeCost;
            private set => upgradeCost = value;
        }
        public double UpgradeModifier {
            get => upgradeModifier;
            private set => upgradeModifier = value;
        }

        public CPlayer(int lvl, CBigNum gold, CBigNum damage, double damageModifier, CBigNum upgradeCost, double upgradeModifier)
        {
            Lvl = lvl;
            Gold = gold;
            Damage = damage;
            DamageModifier = damageModifier; 
            UpgradeModifier = upgradeModifier;
            UpgradeCost = upgradeCost;
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