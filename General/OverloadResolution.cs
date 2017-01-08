using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace General
{
    /// <summary>
    /// Exercise related to method overload resolution. 
    /// Inspired by: http://csharpindepth.com/Articles/General/Overloading.aspx
    /// </summary>
    [TestFixture]
    class OverloadResolution
    {
        internal class Base
        {
            public virtual string Foo(int i)
            {
                return "Base.Foo(int): " + i;
            }
        }

        internal class Derived : Base
        {
            public override string Foo(int i)
            {
                return "Derived.Foo(int): " + i;
            }
            public string Foo(object o)
            {
                return "Derived.Foo(object): " + o;
            }
        }


        [Test]
        public void MethodOverloadResolutionTest()
        {
            Base normalBase = new Base();
            Base reassignedBase = new Derived();
            Derived derived = new Derived();

            int i = 10;
            Assert.AreEqual("Base.Foo(int): 10", normalBase.Foo(i));
            Assert.AreEqual("Derived.Foo(int): 10", reassignedBase.Foo(i));
            Assert.AreEqual("Derived.Foo(object): 10", derived.Foo(i));

        }
    }
}
