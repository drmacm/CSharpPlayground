using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpInDepth
{
    [TestFixture]
    public class GenericsAndStaticConstructors
    {
        /// <summary>Nested generic types and static constructors - executed only once per closed inner type.</summary>
        /// <typeparam name="T"></typeparam>
        class Outer<T>
        {
            public class Inner<U, V>
            {
                private static int counter = 0;
                static string typeInfo;
                static Inner()
                {
                    counter++;
                    typeInfo = string.Format("Outer<{0}>.Inner<{1}, {2}> - counter {3}", 
                                                typeof(T).Name, typeof(U).Name, typeof(V).Name, counter);
                }

                public static string GetTypeInfo()
                {
                    return typeInfo;
                }
            }
        }

        [Test]
        public void NestedGenericTypes_StaticConstructorExecutedOncePerClosedInnerType()
        {
            Assert.AreEqual("Outer<Int32>.Inner<String, DateTime> - counter 1", Outer<int>.Inner<string, DateTime>.GetTypeInfo());
            Assert.AreEqual("Outer<Int32>.Inner<DateTime, String> - counter 1", Outer<int>.Inner<DateTime, string>.GetTypeInfo());
            Assert.AreEqual("Outer<String>.Inner<Int32, Int32> - counter 1", Outer<string>.Inner<int, int>.GetTypeInfo());
            Assert.AreEqual("Outer<Object>.Inner<String, Object> - counter 1", Outer<object>.Inner<string, object>.GetTypeInfo());

            //Static constructor is called only once - counter does not increase!
            Assert.AreEqual("Outer<Int32>.Inner<String, DateTime> - counter 1", Outer<int>.Inner<string, DateTime>.GetTypeInfo());
            Assert.AreEqual("Outer<Int32>.Inner<String, DateTime> - counter 1", Outer<int>.Inner<string, DateTime>.GetTypeInfo());
        }
    }
}
