using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Net9.BinaryFormatter
{
    public class SerializationControl
    {
        /// <summary>
        /// If set, ovverides the default IsSerializable.
        /// </summary>
        public IsSerializableHandlers? IsSerializableHandlers { get; set; }

        /// <summary>
        /// If set, ovverides the default IsNotSerialized.
        /// </summary>
        public IsNotSerializedHandlers? IsNotSerializedHandlers { get; set; }

        //internal static readonly Type s_typeofISerializable = typeof(ISerializable);

        public static readonly SerializationControl Default = new();

        public virtual bool IsSerializable(Type type)
        {
            // 特别允许Hashtable类型通过序列化检查
            if (type == typeof(System.Collections.Hashtable))
                return true;

            if (IsSerializableHandlers != null)
                return IsSerializableHandlers.IsSerializable(type);
            else
                return SerializeByAttribute.IsSerializableStatic(type);
        }

        public virtual bool IsNotSerialized(FieldInfo field)
        {
            if (IsNotSerializedHandlers != null)
                return IsNotSerializedHandlers.IsNotSerialized(field);
            else
                return NotSerializedByAttribute.IsNotSerializedStatic(field);
        }

        public virtual bool IsOptional(MemberInfo member)
        {
            return member.GetCustomAttribute<OptionalFieldAttribute>() != null;
        }

        private Type? ResolveType(Assembly? assembly, string typeName, bool caseInsentitive)
        {
            if (caseInsentitive)
                throw new Exception("CI not supported");

            if (assembly == null)
                throw new Exception("Assembly is null?");

            if (typeName == "System.Collections.Hashtable" && assembly.FullName!.StartsWith("System.Private.CoreLib"))
            {
                return typeof(System.Collections.Hashtable);
            }

            throw new Exception($"Not allowed to load assembly '{typeName}'");
        }
    }
}