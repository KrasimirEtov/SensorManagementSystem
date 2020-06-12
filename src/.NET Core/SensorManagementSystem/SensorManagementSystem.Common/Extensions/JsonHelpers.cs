using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SensorManagementSystem.Common.Extensions
{
	public static class JsonHelpers
	{
        public static T CreateFromJsonStream<T>(this Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            T data;

            using (StreamReader streamReader = new StreamReader(stream))
            {
                data = (T)serializer.Deserialize(streamReader, typeof(T));
            }

            return data;
        }

        public static T CreateFromJsonString<T>(this string json)
        {
            T data;
            using (MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                data = CreateFromJsonStream<T>(stream);
            }
            return data;
        }

        public static T CreateFromJsonFile<T>(this string fileName)
        {
            T data;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                data = CreateFromJsonStream<T>(fileStream);
            }
            return data;
        }
    }
}
