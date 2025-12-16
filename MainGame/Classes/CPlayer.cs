using Microsoft.Win32;
using piogi52.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;

namespace MainGame.Classes
{
    public class PlayerSaver : ISaveList<CPlayer>
    {
        public CPlayer Load(string path)
        {
            string jsonFromFile = File.ReadAllText(path);

            JsonDocument doc = JsonDocument.Parse(jsonFromFile);
            JsonElement element = doc.RootElement;
            
                int lvl = element.GetProperty("Lvl").GetInt32();
                int enemycount = element.GetProperty("EnemyCount").GetInt32();
                double maxcooldown = element.GetProperty("MaxCoolDown").GetDouble();
                double damagemodifier = element.GetProperty("DamageModifier").GetDouble();
                double upgrademodifier = element.GetProperty("UpgradeModifier").GetDouble();

            CBigNum gold = new CBigNum(element.GetProperty("Gold").GetProperty("Digits").EnumerateArray().Select(d => d.GetInt32()).ToArray());
            CBigNum damage = new CBigNum(element.GetProperty("Damage").GetProperty("Digits").EnumerateArray().Select(d => d.GetInt32()).ToArray());
            CBigNum damagecost = new CBigNum(element.GetProperty("DamageCost").GetProperty("Digits").EnumerateArray().Select(d => d.GetInt32()).ToArray());
            CBigNum cooldowncost = new CBigNum(element.GetProperty("CooldownCost").GetProperty("Digits").EnumerateArray().Select(d => d.GetInt32()).ToArray());

            return new CPlayer(lvl, enemycount, gold, damage, damagemodifier, damagecost, cooldowncost, maxcooldown, upgrademodifier);
        }
        public void Save(CPlayer player, string path)
        {
                string json = JsonSerializer.Serialize(player);
                File.WriteAllText(path, json);
        }
    }
    public class CPlayer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly ISaveList<CPlayer> _tudasuda = new PlayerSaver();
        private int lvl;
        private int enemycount;
        private double cooldown;
        private double maxcooldown;
        private double timeleft;
        private bool isCD;
        private CBigNum gold;
        private CBigNum damage;
        private double damageModifier;
        private CBigNum damagecost;
        private CBigNum cooldowncost;
        private double upgradeModifier;

        [JsonInclude]
        public int Lvl {
            get => lvl;
            private set
            {
                lvl = value;
                OnPropertyChanged();
            }
        }
        [JsonInclude]
        public int EnemyCount {
            get => enemycount;
            set
            {
                enemycount = value;
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
        [JsonInclude]
        public double MaxCoolDown {
            get => maxcooldown;
            set
            {
                maxcooldown = value;
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
        [JsonInclude]
        public CBigNum Gold {
            get => gold;
            set
            {
                gold = value;
                OnPropertyChanged();
            }
        }
        [JsonInclude]
        public CBigNum Damage {
            get => damage;
            set
            {
                damage = value;
                OnPropertyChanged();
            }
        }
        [JsonInclude]
        public double DamageModifier {
            get => damageModifier;
            private set => damageModifier = value;
        }
        [JsonInclude]
        public CBigNum DamageCost {
            get => damagecost;
            private set => damagecost = value;
        }
        [JsonInclude]
        public CBigNum CooldownCost {
            get => cooldowncost;
            private set => cooldowncost = value;
        }
        [JsonInclude]
        public double UpgradeModifier {
            get => upgradeModifier;
            private set => upgradeModifier = value;
        }

        public CPlayer(int lvl, CBigNum gold, CBigNum damage, double damageModifier, CBigNum upgradeCost, double upgradeModifier)
        {
            Lvl = lvl;
            EnemyCount = 0;
            Gold = gold;
            Damage = damage;
            DamageModifier = damageModifier; 
            UpgradeModifier = upgradeModifier;
            DamageCost = upgradeCost;
            CooldownCost = upgradeCost;
            MaxCoolDown = 1;
            CoolDown = MaxCoolDown;
            TimeLeft = CoolDown;
            IsCD = true;
        }
        //через лоад
        public CPlayer(int lvl, int enemycount, CBigNum gold, CBigNum damage, double damageModifier,
            CBigNum damagecost, CBigNum cooldowncost, double maxcooldown, double upgradeModifier)
        {
            Lvl = lvl;
            EnemyCount = enemycount;
            Gold = gold;
            Damage = damage;
            DamageModifier = damageModifier;
            UpgradeModifier = upgradeModifier;
            DamageCost = damagecost;
            CooldownCost = cooldowncost;
            MaxCoolDown = maxcooldown;
            CoolDown = MaxCoolDown;
            TimeLeft = CoolDown;
            IsCD = true;
        }
        public void SavePlayer() 
        {
            OpenFileDialog okno = new OpenFileDialog();
            if ((bool)okno.ShowDialog()) 
            {
                _tudasuda.Save(this, okno.FileName);
            }
        }
        public void LoadPlayer() 
        {
            OpenFileDialog okno = new OpenFileDialog();
            if ((bool)okno.ShowDialog()) 
            {
                CPlayer NewPlayer = _tudasuda.Load(okno.FileName);

                Lvl = NewPlayer.Lvl;
                EnemyCount = NewPlayer.EnemyCount;
                Gold = NewPlayer.Gold;
                Damage = NewPlayer.Damage;
                DamageModifier = NewPlayer.DamageModifier;
                UpgradeModifier = NewPlayer.UpgradeModifier;
                DamageCost = NewPlayer.DamageCost;
                CooldownCost = NewPlayer.CooldownCost;
                MaxCoolDown = NewPlayer.MaxCoolDown;
                CoolDown = MaxCoolDown;
                TimeLeft = CoolDown;
                IsCD = true;
            }
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
                MaxCoolDown -= 0.25;
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