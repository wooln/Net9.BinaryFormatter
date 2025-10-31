using Net9.BinaryFormatter;
using Net9.BinaryFormatter.Converters;
using System.Collections;
using System.Collections.Specialized;
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
            // 测试TestNameValueCollection序列化, net5就已经没标记可序列化了
            TestNameValueCollectionSerialization();
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
        
        static void TestNameValueCollectionSerialization()
        {
            Console.WriteLine("Testing NameValueCollection serialization...");
            
            var bf = new Net9.BinaryFormatter.BinaryFormatter();
            bf.SurrogateSelector = new ConverterSelector();
            
            var ms = new MemoryStream();
            NameValueCollection obj = new NameValueCollection
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" },
                { "multi", "value1" },
                { "multi", "value2" } // 添加多个值到同一个键
            };

            try
            {
                Console.WriteLine("Serializing NameValueCollection...");
                bf.Serialize(ms, obj);
                Console.WriteLine("Serialization completed.");
                
                // 重置流位置
                ms.Position = 0;
                
                Console.WriteLine("Deserializing NameValueCollection...");
                var deserializedObj = (NameValueCollection)bf.Deserialize(ms);
                Console.WriteLine("Deserialization completed.");
                
                // 验证内容
                Console.WriteLine($"Original count: {obj.Count}, Deserialized count: {deserializedObj.Count}");
                
                foreach (string? key in obj.AllKeys)
                {
                    if (key != null)
                    {
                        string[]? originalValues = obj.GetValues(key);
                        string[]? deserializedValues = deserializedObj.GetValues(key);
                        
                        if (deserializedValues == null)
                        {
                            Console.WriteLine($"Key '{key}' not found in deserialized NameValueCollection");
                        }
                        else if (originalValues == null && deserializedValues == null)
                        {
                            Console.WriteLine($"Key '{key}' verified (both null)");
                        }
                        else if (originalValues != null && deserializedValues != null)
                        {
                            if (originalValues.Length != deserializedValues.Length)
                            {
                                Console.WriteLine($"Value count mismatch for key '{key}'");
                            }
                            else
                            {
                                bool valuesMatch = true;
                                for (int i = 0; i < originalValues.Length; i++)
                                {
                                    if (originalValues[i] != deserializedValues[i])
                                    {
                                        valuesMatch = false;
                                        break;
                                    }
                                }
                                
                                if (valuesMatch)
                                {
                                    Console.WriteLine($"Key '{key}' with values [{string.Join(", ", originalValues)}] verified");
                                }
                                else
                                {
                                    Console.WriteLine($"Values mismatch for key '{key}'");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Value mismatch for key '{key}'");
                        }
                    }
                }
                
                Console.WriteLine("NameValueCollection test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}