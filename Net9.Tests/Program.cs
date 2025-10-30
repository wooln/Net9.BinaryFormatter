using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace Net9.Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HashTableTest();
            Console.WriteLine("Hello, World!");
        }


        [Obsolete("Obsolete")]
        static void HashTableTest()
        {
            var ht = new Hashtable();
            ht.Add("a", "b");
            ht.Add(1, 2);

            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            // project with <EnableUnsafeBinaryFormatterSerialization>true</EnableUnsafeBinaryFormatterSerialization>
            bf.Serialize(ms, ht);

            ms.Seek(0, SeekOrigin.Begin);
            var graph = (Hashtable)bf.Deserialize(ms);
            if ((string)graph["a"]! != "b") throw new Exception("a");
            if ((int)graph[1] != 2) throw new Exception("1");
        }
    }
}
