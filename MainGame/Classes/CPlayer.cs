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
        private double timeleft;
        private bool isCD;
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
            set
            {
                cooldown = value;
                OnPropertyChanged();
            }
        }
        public bool IsCD {
            get => isCD;
            set
            {
                isCD = value;
                OnPropertyChanged();
            }
        }
        public double TimeLeft {
            get => timeleft;
            set
            {
                timeleft = value;
                OnPropertyChanged();
            }
        }
        public CBigNum Gold {
            get => gold;
            set
            {
                gold = value;
                OnPropertyChanged();
            }
        }
        public CBigNum Damage {
            get => damage;
            set
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
            CooldownCost = upgradeCost;
            CoolDown = 1;
            TimeLeft = CoolDown;
            IsCD = true;
        }
        public void AddGold(CBigNum amount)
        {
            Gold = Gold + amount;
        }
        public void SetCD()
        {
            TimeLeft = CoolDown;
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
        public void AddBonus(PlayerBonus bonus)
        {
            bonus.Apply(this); // применяем бонус
            Bonuses.Add(bonus);
        }

        private List<PlayerBonus> Bonuses = new List<PlayerBonus>();
        public void Update(double delta)
        {
            if (!IsCD)
            {
                TimeLeft -= delta;
                if (TimeLeft <= 0)
                {
                    SetCD();
                    IsCD = true;
                }
            }

            for (int i = Bonuses.Count - 1; i >= 0; i--)
            {
                Bonuses[i].Duration -= delta;

                if (Bonuses[i].Duration <= 0)
                {
                    Bonuses[i].Remove(this);
                    Bonuses.RemoveAt(i);
                }
            }
        }
    }
    public class PlayerBonus
    {
        public double Duration;
        public Action<CPlayer> Apply;
        public Action<CPlayer> Remove;

        public PlayerBonus(double duration, Action<CPlayer> apply, Action<CPlayer> remove)
        {
            Duration = duration;
            Apply = apply;
            Remove = remove;
        }
    }
}