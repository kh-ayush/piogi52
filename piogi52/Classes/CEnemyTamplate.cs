using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace piogi52.Classes
{
    public class CEnemyTemplate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        string name;
        string iconPath;
        string iconName;
        int baseLife;
        double lifeModifier;
        int baseGold;
        double goldModifier;
        double spawnChance;

        public CEnemyTemplate(string name, string iconPath, int baseLife, double lifeModifier, int baseGold, double goldModifier, double spawnChance)
        {
            this.name = name;
            this.iconPath = iconPath;
            string[] m = iconPath.Split(new char[] { '\\' }); this.iconName = m.Last();
            this.baseLife = baseLife;
            this.lifeModifier = lifeModifier;
            this.baseGold = baseGold;
            this.goldModifier = goldModifier;
            this.spawnChance = spawnChance;
        }
        public CEnemyTemplate()
        {
            name = "Новый монстр";
            iconPath = "";
            iconName = "";
            baseLife = 0;
            lifeModifier = 1;
            baseGold = 0;
            goldModifier = 1;
            spawnChance = 0.5;
        }

        [JsonInclude]
        public string Name
        {
            get => name;
            set
            {
                name = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged("Name");
            }
        }
        [JsonInclude]
        public string IconPath
        {
            get => iconPath;
            set
            {
                iconPath = value ?? throw new ArgumentNullException(nameof(value));
                string[] m = iconPath.Split(new char[] { '\\' }); iconName = m.Last();
                OnPropertyChanged("IconPath"); OnPropertyChanged("IconName");
            }
        }
        [JsonInclude]
        public string IconName
        {
            get => iconName;
        }
        [JsonInclude]
        public int BaseLife
        {
            get => baseLife;
            set => baseLife = value;
        }

        [JsonInclude]
        public double LifeModifier
        {
            get => lifeModifier;
            set => lifeModifier = value;
        }

        [JsonInclude]
        public int BaseGold
        {
            get => baseGold;
            set => baseGold = value;
        }

        [JsonInclude]
        public double GoldModifier
        {
            get => goldModifier;
            set => goldModifier = value;
        }

        [JsonInclude]
        public double SpawnChance
        {
            get => spawnChance;
            set => spawnChance = value;
        }

    }
}
