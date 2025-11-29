using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainGame.Classes
{
    public class CPlayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int lvl;
        private double cooldown;
        private CBigNum gold;
        private CBigNum damage;
        private double damageModifier;
        private CBigNum damagecost;
        private CBigNum cooldowncost;
        private double upgradeModifier;

        public int Lvl {
            get => lvl;
            private set
            {
                lvl = value;
                OnPropertyChanged();
            }
        }
        public double CoolDown {
            get => cooldown;
            private set
            {
                cooldown = value;
                OnPropertyChanged();
            }
        }
        public CBigNum Gold {
            get => gold;
            private set
            {
                gold = value;
                OnPropertyChanged();
            }
        }
        public CBigNum Damage {
            get => damage;
            private set
            {
                damage = value;
                OnPropertyChanged();
            }
        }
        public double DamageModifier {
            get => damageModifier;
            private set => damageModifier = value;
        }
        public CBigNum DamageCost {
            get => damagecost;
            private set => damagecost = value;
        }
        public CBigNum CooldownCost {
            get => cooldowncost;
            private set => cooldowncost = value;
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
            DamageCost = upgradeCost;
            CoolDown = 5;
        }
        public void AddGold(CBigNum amount)
        {
            Gold = Gold + amount;
        }

        public bool TryUpgradeDM()
        {
            if (TrySpendGold(DamageCost))
            {
                Lvl++;
                Damage = Damage * (DamageModifier * Lvl);
                DamageCost *= UpgradeModifier;
                return true;
            }
            return false;
        }

        public bool TryUpgradeCD()
        {
            if (TrySpendGold(CooldownCost))
            {
                CoolDown -= 0.25;
                CooldownCost *= UpgradeModifier;
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
        public CBigNum DealDamage()
        {
            return Damage;
        }
        public double GetCoolDown()
        {
            return CoolDown;
        }
    }
}