using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace com.zumstudios.zumpass
{
    public static class DataHelper
    {
        public static void Save<T>(object obj, string fileName)
        {
            var path = GetPersistentDataPath(fileName);
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static T Load<T>(string fileName)
        {
            var path = GetPersistentDataPath(fileName);

            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                T obj = (T)formatter.Deserialize(stream);
                stream.Close();

                return obj;
            }

            return default(T);
        }

        public static void Delete(string fileName)
        {
            var path = GetPersistentDataPath(fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static DateTime GetDateTimeFromTimestamp(long timestampMilliseconds)
        {
            var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddMilliseconds(timestampMilliseconds);
        }

        public static string GetPersistentDataPath(string fileName)
        {
            if (fileName == null)
            {
                return Path.Combine(Application.persistentDataPath, "ZUMPass");
            }

            return Path.Combine(Application.persistentDataPath, "ZUMPass", fileName);
        }
    }
}

