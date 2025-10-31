using Net9.BinaryFormatter;
using Net9.BinaryFormatter.Converters;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Net9.BFTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 测试Hashtable序列化
            TestHashtableSerialization();
            Console.WriteLine("Success!");
        }

        static void TestHashtableSerialization()
        {
            Console.WriteLine("Testing Hashtable serialization...");
            
            var bf = new Net9.BinaryFormatter.BinaryFormatter();
            bf.SurrogateSelector = new ConverterSelector();
            
            var ms = new MemoryStream();
            var ht = new Hashtable();
            ht.Add("key1", "value1");
            ht.Add("key2", 123);
            ht.Add(42, "numeric key");
            ht.Add(true, "boolean key");

            try
            {
                Console.WriteLine("Serializing Hashtable...");
                bf.Serialize(ms, ht);
                Console.WriteLine("Serialization completed.");
                
                // 重置流位置
                ms.Position = 0;
                
                Console.WriteLine("Deserializing Hashtable...");
                var deserializedHt = (Hashtable)bf.Deserialize(ms);
                Console.WriteLine("Deserialization completed.");
                
                // 验证内容
                Console.WriteLine($"Original count: {ht.Count}, Deserialized count: {deserializedHt.Count}");
                
                foreach (DictionaryEntry entry in ht)
                {
                    if (!deserializedHt.Contains(entry.Key))
                    {
                        Console.WriteLine($"Key '{entry.Key}' not found in deserialized Hashtable");
                    }
                    else if (!Equals(deserializedHt[entry.Key], entry.Value))
                    {
                        Console.WriteLine($"Value for key '{entry.Key}' does not match");
                    }
                    else
                    {
                        Console.WriteLine($"Key '{entry.Key}' with value '{entry.Value}' verified");
                    }
                }
                
                Console.WriteLine("Hashtable test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}