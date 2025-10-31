// See https://aka.ms/new-console-template for more information

using System.Collections;
using Net9.BinaryFormatter;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

public class Program
{
    //[Net9.BinaryFormatter.Serializable]
    public enum Lol42
    {
        Test = 5
    }

    public static void Main()
    {

        Console.WriteLine("Hello, World!");

        var bf = new BinaryFormatter();
        var ms = new MemoryStream();

        var d = new Dictionary<int, string>();
        d.Add(4, "dd");
        d.Add(5, "dd");
        d.Add(6, "dd");

        var d2 = new Dictionary<int, string>();
        d2.Add(4, "dd2");
        d2.Add(5, "dd2");
        d2.Add(6, "dd2");

        var list = new List<Dictionary<int, string>>();
        list.Add(d);
        list.Add(d2);


        
        var kv = new KeyValuePair2<int, int>(1, 2);

        var to = TimeOnly.FromDateTime(DateTime.Now);

        var cs = new ConverterSelector();

//        cs.Converters.Add(new GenericStackConverterFactory());
        //cs.Converters.Add(new Net9.BinaryFormatter.Converters.DateTimeConverter());

        bf.SurrogateSelector = new ConverterSelector();

        bf.Control.IsSerializableHandlers = new IsSerializableHandlers();
        bf.Control.IsSerializableHandlers.Handlers.OfType<SerializeAllowedTypes>().Single().AllowedTypes.Add(typeof(object));
        //bf.Control.IsSerializableHandlers.Handlers.OfType<SerializeAllowedTypes>().Single().AllowedTypes.Add(typeof(IConvertible));
        //bf.Control.IsSerializableHandlers.Handlers.OfType<SerializeAllowedTypes>().Single().AllowedTypes.Add(typeof(Test));
        var b = new AllowedTypesBinder();
        b.AddAllowedType(typeof(Test));
        b.AddAllowedType(typeof(IConvertible));
        b.AddAllowedType(typeof(IComparable));
        bf.Binder = b;

        //bf.Control = 
        //   bf.IsSerializable = new IsSerializableHandlers().IsSerializable;
        //        bf.Binder

        var hs= new HashSet<int>() { 5 };
        var sta = new Stack<int>();
        sta.Push(45);
        sta.Push(145);
        
        Test nt;
        try
        {
            throw new Exception("lol");
        }
        catch (Exception e)
        {
            TraceFlags.Formatter_IConvertibleFix = true;
            TraceFlags.Formatter_IConvertibleArrayFix = true;

            nt = new Test();
            var t = nt.dict.GetType();

            bf.Serialize(ms, nt);// list);
        }

        //var bf_desser = new BinaryFormatter();
        //bf_desser.SurrogateSelector = new ConverterSelector();
        //bf_desser.Control = new IsSerializableAlwaysTrue();


        ms.Position = 0;

        var g = (Test)bf.Deserialize(ms);//< List<Dictionary<int, string>>>(ms);

        //var sta2 = (Stack<int>)g;
        //var p1 = sta2.Pop();
        //var p2 = sta2.Pop();

        if (g.i != nt.i) throw new Exception($"{g.i}, {nt.i}");
        if (g.nullable != nt.nullable) throw new Exception($"nullable: {g.nullable}, {nt.nullable}");
        if (g.nullable2 != nt.nullable2) throw new Exception($"nullable2: {g.nullable2}, {nt.nullable2}");
        if ((int)g.icon_4 != (int)nt.icon_4) throw new Exception($"icon_4: {g.icon_4}, {nt.icon_4}");
        if ((int)g.obj_4 != (int)nt.obj_4) throw new Exception($"obj_4: {g.obj_4}, {nt.obj_4}");
        if (g.to != nt.to) throw new Exception($"to: {g.to}, {nt.to}");
        if (g.don != nt.don) throw new Exception($"don: {g.don}, {nt.don}");
        if ((int)g.comp != (int)nt.comp) throw new Exception($"comp: {g.comp}, {nt.comp}");
        // Compare arrays
        if (!Enumerable.SequenceEqual(g.objarr, nt.objarr)) throw new Exception("objarr sequences are not equal");
        if (!Enumerable.SequenceEqual(g.objarr2, nt.objarr2)) throw new Exception("objarr2 sequences are not equal");
        if (!Enumerable.SequenceEqual(g.iconarr, nt.iconarr)) throw new Exception("iconarr sequences are not equal");
        if (!Enumerable.SequenceEqual(g.intarr, nt.intarr)) throw new Exception("intarr sequences are not equal");
        if (!Enumerable.SequenceEqual(g.comparr, nt.comparr)) throw new Exception("comparr sequences are not equal");
        // Compare dictionaries and lists
        if (g.dict.Count != nt.dict.Count) throw new Exception($"dict count: {g.dict.Count}, {nt.dict.Count}");
        if (g.dicts.Count != nt.dicts.Count) throw new Exception($"dicts count: {g.dicts.Count}, {nt.dicts.Count}");
        Console.WriteLine("Hello, World!");
        
        HashTableTest();
    }

    private static void HashTableTest()
    {
        var ht = new Hashtable();
        ht.Add("a", "b");
        ht.Add(1, 2);
        
        var bf = new BinaryFormatter();
        var ms = new MemoryStream();
        bool throwsError = false;
        try
        {
            bf.Serialize(ms, ht);
        }
        catch (Net9.BinaryFormatter.SerializationException ex)
        {
            Console.Out.WriteLine(ex.Message);
            throwsError = true;
        }

        if (!throwsError) throw new Exception("should: Unhandled exception. Net9.BinaryFormatter.SerializationException: Type 'System.Collections.Hashtable' in Assembly 'System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e' is not marked as serializable.\n ");
        // ms.Seek(0, SeekOrigin.Begin);
        // var graph = (Hashtable)bf.Deserialize(ms);
        // if ((string)graph["a"]! != "b") throw new Exception("a");
        // if ((int)graph[1] != 2) throw new Exception("1");
    }
}

[Net9.BinaryFormatter.Serializable]
public class Test
{
    public int? nullable = null;
    public int? nullable2 = 42;
    public int i = 55;
    public IConvertible icon_null = null!;
    public object obj_4 = 4;
    public object[] objarr = new IConvertible[] { 5,5d,7M,77};
    public object[] objarr2 = new object[] { 5, 5d, 7M, 77, 98f };
    public IConvertible icon_4 = 4;
    public IConvertible[] iconarr = new IConvertible[] { 1, 2, 3, 4 };
    public int[] intarr = new int[] { 1, 2, 3, 4 };
    public TimeOnly to = new TimeOnly(42);
    public DateOnly don = new DateOnly(1, 2, 3);
    public IComparable comp = 4;
    public IComparable[] comparr = new IComparable[] { 1, 2, 3, 4 };
    public Dictionary<string, List<TimeSpan>> dict = new();
    public List<Dictionary<string, List<TimeSpan>>> dicts = new();

}

[Net9.BinaryFormatter.Serializable]
public readonly struct KeyValuePair2<TKey, TValue>
{

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly TKey key; // Do not rename (binary serialization)
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly TValue value; // Do not rename (binary serialization)

    public KeyValuePair2(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }

    public TKey Key => key;

    public TValue Value => value;

  
}

class IsSerializableAlwaysTrue : SerializationControl
{
    public override bool IsSerializable(Type type)
    {
        return true;
    }
}