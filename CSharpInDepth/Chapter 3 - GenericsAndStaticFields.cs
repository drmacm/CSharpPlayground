using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpInDepth
{
    [TestFixture]
    public class GenericsAndStaticFields
    {
        /// <summary>
        /// Static fields belong to the type where they are declared.
        /// With generics, there is one static field for each closed type.
        /// </summary>
        class TypeWithField<T>
        {
            public static string field;
        }

        [Test]
        public void GenericTypes_HaveStaticFieldsPerClosedType()
        {
            TypeWithField<int>.field = "I am int";
            TypeWithField<string>.field = "I am string";
            TypeWithField<DateTime>.field = "I am DateTime";

            Assert.AreEqual("I am int", TypeWithField<int>.field);
            Assert.AreEqual("I am string", TypeWithField<string>.field);
            Assert.AreEqual("I am DateTime", TypeWithField<DateTime>.field);

            TypeWithField<int>.field = "I am changed int";
            Assert.AreEqual("I am changed int", TypeWithField<int>.field);
        }
    }
}
