// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

Console.WriteLine("Hello, World!");

#pragma warning disable SYSLIB0011 // Type or member is obsolete


var bf = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolet

var ms = new MemoryStream();
bool throwsError = false;
try
{
    bf.Serialize(ms, 42);
}
catch (System.PlatformNotSupportedException e)
{
    throwsError = true;
    Console.WriteLine(e.Message);
}

if (!throwsError)
    throw new Exception(
        "should: Unhandled exception. System.PlatformNotSupportedException: BinaryFormatter serialization and deserialization have been removed. See https://aka.ms/binaryformatter for more information.");

Console.WriteLine("Hello, World!");