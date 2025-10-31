using System;
using System.Collections.Specialized;

namespace Net9.BinaryFormatter.Converters
{
    public class NameValueCollectionConverter : BinaryConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(NameValueCollection);
        }

        public override void Serialize(object obj, SerializationInfo info, StreamingContext context)
        {
            var nvc = (NameValueCollection)obj;
            
            // 将NameValueCollection转换为键值对数组进行序列化
            var keys = new string[nvc.Count];
            var values = new string[nvc.Count][];
            
            int i = 0;
            foreach (string? key in nvc.AllKeys)
            {
                if (key != null)
                {
                    keys[i] = key;
                    values[i] = nvc.GetValues(key);
                    i++;
                }
            }
            
            info.AddValue("Keys", keys);
            info.AddValue("Values", values);
            info.AddValue("Count", nvc.Count);
        }

        public override object Deserialize(object obj, SerializationInfo info, StreamingContext context)
        {
            var keys = (string[]?)info.GetValue("Keys", typeof(string[]));
            var values = (string[]?[]?)info.GetValue("Values", typeof(string[]?[]));
            
            var nvc = new NameValueCollection();
            if (keys != null && values != null)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    if (values[i] != null)
                    {
                        foreach (string? value in values[i])
                        {
                            if (value != null)
                            {
                                nvc.Add(keys[i], value);
                            }
                        }
                    }
                }
            }
            
            return nvc;
        }
    }
}