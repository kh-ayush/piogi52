using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Data;
using static piogi52.Classes.EnemyTemplateConverter;

namespace piogi52.Classes
{
    //public class EnemyTypeConverter : IValueConverter
    //{
    //    public Type EnemyType { get; set; }

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return value != null && value.GetType() == EnemyType;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if ((bool)value)
    //            return Activator.CreateInstance(EnemyType);

    //        return Binding.DoNothing;
    //    }
    //}
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
                    case "CBasicEnemyTemplate":
                        return
                       JsonSerializer.Deserialize<CBasicEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                    case "CBoberEnemyTemplate":
                        return
                       JsonSerializer.Deserialize<CBoberEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
                    case "CMedEnemyTemplate":
                        return
                       JsonSerializer.Deserialize<CMedEnemyTemplate>(jsonDoc.RootElement.GetRawText(), options);
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

    public class JsonEnemySaver : ISaveList<ObservableCollection<CEnemyTemplate>>
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
        public ObservableCollection<CEnemyTemplate> Load(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                //Десериализация с определением класса противника
                return JsonSerializer.Deserialize<ObservableCollection<CEnemyTemplate>>(json, _options);// ?? new ObservableCollection<CEnemyTemplate>();
            }
            return new ObservableCollection<CEnemyTemplate>();
        }
        //Реализация функции сохранения
        public void Save(ObservableCollection<CEnemyTemplate> data, string path)
        {
            string json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(path, json);
        }
    }
}
