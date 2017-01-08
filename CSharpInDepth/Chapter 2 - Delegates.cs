using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpInDepth
{
    /// <summary>Contains methods that match our delegate's signature</summary>
    class DelegateActions
    {
        internal static string ToUpperCaseStatic(string input)
        {
            return input.ToUpper();
        }

        internal string ToUpperCase(string input)
        {
            return input.ToUpper();
        }

    }

    /// <summary>Trying out a few things related to delegates./summary>
    [TestFixture]
    public class Delegates
    {
        private DelegateActions delegateActions = new DelegateActions();

        delegate string StringProcessor(string input);

        [Test]
        public void VariousWaysToCreateDelegate()
        {
            var testInput = "test";
            var expectedOutput = "TEST";

            StringProcessor del1 = new StringProcessor(delegateActions.ToUpperCase);
            StringProcessor del2 = delegateActions.ToUpperCase;
            StringProcessor del3 = (StringProcessor)Delegate.CreateDelegate(typeof(StringProcessor), delegateActions, "ToUpperCase");
            StringProcessor del4 = delegate (string input)
            {
                return input.ToUpper();
            };
            StringProcessor del5 = (input) => input.ToUpper();
            StringProcessor del6 = delegate { return "TEST"; }; //anonymous method shortcut, ignoring parameters
            
            Assert.AreEqual(expectedOutput, del1(testInput));
            Assert.AreEqual(expectedOutput, del2(testInput));
            Assert.AreEqual(expectedOutput, del3(testInput));
            Assert.AreEqual(expectedOutput, del4(testInput));
            Assert.AreEqual(expectedOutput, del5(testInput));
            Assert.AreEqual(expectedOutput, del6(testInput));
        }

        [Test]
        public void AllDelegatesInheritFromMulticastDelegate()
        {
            var processor = new StringProcessor(DelegateActions.ToUpperCaseStatic);

            Assert.IsInstanceOf<MulticastDelegate>(processor);
            Assert.IsInstanceOf<Delegate>(processor);
        }

        [Test]
        public void TargetIsSetForInstanceMetodsAndNullForStaticMethods()
        {
           
            StringProcessor instanceAction = delegateActions.ToUpperCase;
            StringProcessor staticAction = DelegateActions.ToUpperCaseStatic;

            Assert.NotNull(instanceAction.Target);
            Assert.Null(staticAction.Target);
        }

      
        [Test]
        public void VariousWaysToInvokeDelegate()
        {
            StringProcessor del = DelegateActions.ToUpperCaseStatic;

            var input = "test";
            var expectedOutput = "TEST";

            var directCall = del(input);
            Assert.AreEqual(expectedOutput, directCall);

            var standardInvoke = del.Invoke(input);
            Assert.AreEqual(expectedOutput, standardInvoke);

            var dynamicInvoke = del.GetInvocationList()[0].DynamicInvoke(input);
            Assert.AreEqual(expectedOutput, dynamicInvoke);

            var asyncResult = del.BeginInvoke(input, null, null);
            var asyncInvoke = del.EndInvoke(asyncResult);
            Assert.AreEqual(expectedOutput, asyncInvoke);
        }

        [Test]
        public void DelegatesAreImmutable()
        {
            StringProcessor del1 = DelegateActions.ToUpperCaseStatic;
            StringProcessor del2 = delegateActions.ToUpperCase;

            var del3 = Delegate.Combine(del1, del2);
            StringProcessor temp = del1;

            Assert.AreSame(temp, del1);
            Assert.AreNotSame(del3, del1);
            Assert.AreNotSame(del3, del2);


        }

        
    }
}
