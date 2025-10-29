using piogi52.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;



namespace piogi52.Classes
{
    public class CEnemyTemplateList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<CEnemyTemplate> enemies { get; set; }
        public CEnemyTemplateList()
        {
            enemies = new ObservableCollection<CEnemyTemplate>();
        }
        public void addEnemy(string name, string iconPath, int baseLife, double lifeModification, int baseGold, double goldModification, double spawnChance)
        {
            enemies.Add(new CEnemyTemplate(name, iconPath, baseLife, lifeModification, baseGold, goldModification, spawnChance));
        }
        public void AddEnemy(CEnemyTemplate x)
        {
            enemies.Add(x);
        }
        public void Clear()
        {
            enemies.Clear();
        }
        public CEnemyTemplate GetByName(string name)
        {
            foreach (CEnemyTemplate x in enemies)
                if (x.Name == name) return x;
            return null;
        }
        public CEnemyTemplate GetById(int i)
        {
            return enemies[i];
        }
        public void DelByName(string name)
        {
            enemies.Remove(enemies.FirstOrDefault(x => x.Name == name));
        }
        public void DelById(int i)
        {
            enemies.RemoveAt(i);
        }
        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (CEnemyTemplate x in enemies) names.Add(x.Name);
            return names;
        }
        public void SaveJson()
        {
            string jsonString = JsonSerializer.Serialize(enemies);
            File.WriteAllText("EnemysList.json", jsonString);
        }
        public void LoadJson()
        {
            string jsonFromFile = File.ReadAllText("C:\\Users\\bob2a\\source\\repos\\piogi52\\piogi52\\bin\\Debug\\net8.0-windows\\EnemysList.json");
            JsonDocument doc = JsonDocument.Parse(jsonFromFile);
            foreach (JsonElement element in doc.RootElement.EnumerateArray())
            {
                string name = element.GetProperty("Name").GetString();
                string iconPath = element.GetProperty("IconPath").GetString();
                int baseLife = element.GetProperty("BaseLife").GetInt32();
                double lifeModification = element.GetProperty("LifeModifier").GetDouble();
                int baseGold = element.GetProperty("BaseGold").GetInt32();
                double goldModification = element.GetProperty("GoldModifier").GetDouble();
                double spawnChance = element.GetProperty("SpawnChance").GetDouble();
                enemies.Add(new CEnemyTemplate(name, iconPath, baseLife, lifeModification, baseGold, goldModification, spawnChance));
            }
        }
    }
}


