using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static piogi52.Classes.EnemyTemplateConverter;

namespace piogi52.Classes
{
    public class EnemyTemplateConverter : JsonConverter<CEnemyTemplate>
    {
        public override CEnemyTemplate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Создаем JSON-документ
            var jsonDoc = JsonDocument.ParseValue(ref reader);
            try
            {
                // Получаем тип объекта
                string type = jsonDoc.RootElement.GetProperty("$type").GetString();
                switch (type)
                {
                    //Определение типа бронированного противника
                    case "CArmoredEnemyTemplate":
                        return
                       JsonSerializer.Deserialize<CArmoredEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                    //Определение типа обычного противника
                    case "CNormalEnemyTemplate":
                        return
                       JsonSerializer.Deserialize<CBasicEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                    default:
                        throw new NotSupportedException($"Unknown type: {type}");
                }
            }
            finally
            {
                // Освобождаем ресурс
                jsonDoc.Dispose();
            }
        }
        public override void Write(Utf8JsonWriter writer, CEnemyTemplate value, JsonSerializerOptions options)
        {
            string type = value.GetType().Name; // Определяем тип: CNormalEnemyTemplate, CArmoredEnemyTemplate и т.д.
            string json = JsonSerializer.Serialize(value, value.GetType(), options);
            var jsonDoc = JsonDocument.Parse(json);
            try
            {
                writer.WriteStartObject();
                writer.WriteString("$type", type); // Добавляем информацию о типе
                                                   // Копируем все свойства
                foreach (var property in jsonDoc.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
                writer.WriteEndObject();
            }
            finally
            {
                jsonDoc.Dispose(); // Освобождаем ресурс
            }
        }
    }
    public interface ISaveList<T> 
    {   //Сигнатура загрузки
        T Load(string path);//Сигнатура сохранения
        void Save(T data, string path);
    }
    public class JsonEnemySaver : ISaveList<List<CEnemyTemplate>>
    {
        private readonly JsonSerializerOptions _options;
        public JsonEnemySaver()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                //Установка конвертера противников для реализации полиморфизма
                Converters = { new EnemyTemplateConverter() }
            };
        }
        public List<CEnemyTemplate> Load(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                //Десериализация с определение класса противника
                return JsonSerializer.Deserialize<List<CEnemyTemplate>>(json, _options) ?? new
               List<CEnemyTemplate>();
            }
            return new List<CEnemyTemplate>();
        }
        //Реализация функции сохранения
        public void Save(List<CEnemyTemplate> data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }
    }
}
