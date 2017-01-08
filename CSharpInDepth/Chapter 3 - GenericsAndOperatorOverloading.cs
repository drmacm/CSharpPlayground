using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpInDepth
{
    [TestFixture]
    public class OperatorOverloadingWithGenerics
    {
        /// <summary>Sample class which overload == operator, used to check behavior of generic methods.</summary>
        class StringHolder
        {
            public string Value { get; set; }

            public static bool operator ==(StringHolder first, StringHolder second)
            {
                return first.Value == second.Value;
            }

            public static bool operator !=(StringHolder first, StringHolder second)
            {
                return !(first == second);
            }
        }

        /// <summary>
        /// Reference comparison of 2 variables of type T, where T is a reference type (no other constraints).
        /// If the type specified by the caller overloads the == operator, that overload IS NOT used in generic method.
        /// Equality test for 2 strings in this method will return "false", although the string class overloads the == operator.
        /// </summary>
        static bool AreReferencesEqual<T>(T first, T second)
            where T : class
        {
            return first == second;
        }

        /// <summary>
        /// Reference comparison of 2 variables of type T, where T is constrained to be implicitly convertible to StringHolder.
        /// If the type used in a constraint overloads the == operator, that overload IS used in the generic method
        /// Equality test for 2 StringHolders in this method will return "true", because is overloads the == operator.
        /// </summary>
        static bool AreConstrainedReferencesEqual<T>(T first, T second)
            where T : StringHolder
        {
            return first == second;
        }

        [Test]
        public void GenericMethod_DoesNotMakeUseOfOperatorOverloading_IfSpecifiedByTheCaller()
        {
            string piece = "world";
            string first = "Hello " + piece;
            string second = "Hello " + piece;

            Assert.True(first == second);
            Assert.False(AreReferencesEqual(first, second));
        }

        [Test]
        public void GenericMethod_MakesUseOfOperatorOverloading_IfSpecifiedByTheConstraint()
        {
            string piece = "world";
            string first = "Hello " + piece;
            string second = "Hello " + piece;
            StringHolder firstHolder = new StringHolder { Value = first };
            StringHolder secondHolder = new StringHolder { Value = second };

            Assert.True(first == second);
            Assert.True(AreConstrainedReferencesEqual(firstHolder, secondHolder));
        }
    }
}
