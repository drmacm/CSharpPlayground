using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpInDepth
{
    [TestFixture]
    public class GenericsAndTypeof
    {
        public static T ReturnArgument<T>(T t)
        {
            return t;
        }

        /// <summary>Small exercise of how generic types look like when retrieved via typeof operator</summary>
        [Test]
        public void GenericsAndTypeofOperator()
        {
            Assert.AreEqual("System.Int32", typeof(int).ToString());
            Assert.AreEqual("System.Collections.Generic.List`1[T]", typeof(List<>).ToString());
            Assert.AreEqual("System.Collections.Generic.Dictionary`2[TKey,TValue]", typeof(Dictionary<,>).ToString());
            Assert.AreEqual("System.Collections.Generic.List`1[System.Int32]", typeof(List<int>).ToString());
            Assert.AreEqual("System.Collections.Generic.Dictionary`2[System.String,System.Int32]", typeof(Dictionary<string, int>).ToString());
            Assert.AreEqual("System.Collections.Generic.List`1[System.Int64]", typeof(List<long>).ToString());
            Assert.AreEqual("System.Collections.Generic.Dictionary`2[System.Int64,System.Guid]", typeof(Dictionary<long, Guid>).ToString());
        }


        /// <summary>Various ways of retrieving generic and constructed Type objects.</summary>
        /// <remarks>There is one Type object per type.</remarks>
        [Test]
        public void GenericsAndTypeObject_VariousWaysOfRetrieving()
        {
            string listTypeName = "System.Collections.Generic.List`1";

            Type defByName = Type.GetType(listTypeName);
            Type closedByName = Type.GetType(listTypeName + "[System.String]");
            Type closedByMethod = defByName.MakeGenericType(typeof(string));
            Type closedByTypeof = typeof(List<string>);

            Type defByTypeof = typeof(List<>);
            Type defByMethod = closedByName.GetGenericTypeDefinition();

            Assert.AreEqual(closedByName, closedByMethod);
            Assert.AreEqual(closedByName, closedByTypeof);
            Assert.AreEqual(defByName, defByTypeof);
            Assert.AreEqual(defByName, defByMethod);
        }

        [Test]
        public void GenericMethodAndReflection_InvokingAMethodViaReflection()
        {
            Type type = typeof(GenericsAndTypeof);
            MethodInfo definition = type.GetMethod("ReturnArgument");
            MethodInfo constructed = definition.MakeGenericMethod(typeof(string));
            string result = (string)constructed.Invoke(null, new[] { "test value" });

            Assert.AreEqual("test value", result);
        }
    }
}
