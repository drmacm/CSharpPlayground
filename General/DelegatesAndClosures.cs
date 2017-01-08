using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace General
{
    [TestFixture]
    class DelegatesAndClosures
    {
        delegate string PrettyPrinter();

        [Test]
        public void ClosuresAreCapturingVariable_NotJustValue_AllDelegatesWillPrint3()
        {
            List<PrettyPrinter> printers = new List<PrettyPrinter>();
            for (int i = 0; i < 3; i++)
            {
                PrettyPrinter printer = delegate { return i.ToString(); };
                printers.Add(printer);
            }

            Assert.AreEqual("3", printers[0]());
            Assert.AreEqual("3", printers[1]());
            Assert.AreEqual("3", printers[2]());
        }
    }
}
