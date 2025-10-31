using System;
using System.Collections;

namespace Net9.BinaryFormatter.Converters
{
    public class HashtableConverter : BinaryConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(Hashtable);
        }

        public override void Serialize(object obj, SerializationInfo info, StreamingContext context)
        {
            var hashtable = (Hashtable)obj;
            
            // 将Hashtable转换为键值对数组进行序列化
            var entries = new DictionaryEntry[hashtable.Count];
            int i = 0;
            foreach (DictionaryEntry entry in hashtable)
            {
                entries[i] = entry;
                i++;
            }
            
            info.AddValue("KeyValues", entries);
            info.AddValue("HashSize", hashtable.Count);
        }

        public override object Deserialize(object obj, SerializationInfo info, StreamingContext context)
        {
            var entries = (DictionaryEntry[]?)info.GetValue("KeyValues", typeof(DictionaryEntry[]));
            
            var hashtable = new Hashtable();
            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    hashtable.Add(entry.Key, entry.Value);
                }
            }
            
            return hashtable;
        }
    }
}