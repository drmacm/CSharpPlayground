using System.Linq;
using NUnit.Framework;

namespace CSharpInDepth
{
    /// <summary>
    /// Trying out differences between Value and reference types and parameter passing.
    /// Inspired by "C# in Depth, chapter 2.3, and http://jonskeet.uk/csharp/parameters.html
    /// </summary>
    [TestFixture]
    public class ValueAndReferenceTypes
    {
        #region Data
        /// <summary>Structs and enums are value types.</summary>
        struct IntHolderStruct
        {
            public int Number;
            public IntHolderStruct(int number)
            {
                Number = number;
            }
        }

        /// <summary>Classes, interfaces and delegates are reference types.</summary>
        class IntHolderClass
        {
            public int Number;
            public IntHolderClass(int number)
            {
                Number = number;
            }
        }
        #endregion


        #region Value and reference types - assignments
        [Test]
        public void ReferenceTypes_AssigningOneVariableToAnother_VariablesPointToSameObject()
        {
            var first = new IntHolderClass(42);
            var second = first;
            first.Number = 43;

            Assert.AreEqual(43, second.Number);
        }

        [Test]
        public void ReferenceTypes_ReassigningVariableAfterAssginment_VariablesAreIndependent()
        {
            var first = new IntHolderClass(42);
            var second = first;
            first = null;

            Assert.AreEqual(42, second.Number);
        }

        [Test]
        public void ValueTypes_AssigningOneVariableToAnother_DataIsCopied()
        {
            var first = new IntHolderStruct(1);
            var second = first;
            first.Number = 2;

            Assert.AreEqual(1, second.Number);
        }
        #endregion


        #region Passing parameters by value
        [Test]
        public void PassingByValue_ValueTypeParameter_ChangingTheValueInsideFunction_ShouldNotChangeCallerValue()
        {
            var valueType = new IntHolderStruct(5);
            F1(valueType);

            Assert.AreEqual(5, valueType.Number);
        }
        private void F1(IntHolderStruct valueType)
        {
            valueType.Number = 11;
        }

        [Test]
        public void PassingByValue_ValueTypeParameter_ReassigningTheParameter_ShouldNotChangeCallerValue()
        {
            var valueType = new IntHolderStruct(5);
            F2(valueType);

            Assert.AreEqual(5, valueType.Number);
        }
        private void F2(IntHolderStruct valueType)
        {
            valueType = new IntHolderStruct(22);
        }

        [Test]
        public void PassingByValue_ReferenceTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            var referenceType = new IntHolderClass(5);
            F3(referenceType);

            Assert.AreEqual(33, referenceType.Number);
        }
        private void F3(IntHolderClass referenceType)
        {
            referenceType.Number = 33;
        }

        [Test]
        public void PassingByValue_ReferenceTypeParameter_ReassigningTheParameter_ShouldNotChangeCallerValue()
        {
            var referenceType = new IntHolderClass(5);
            F4(referenceType);

            Assert.AreEqual(5, referenceType.Number);
        }
        private void F4(IntHolderClass referenceType)
        {
            referenceType = new IntHolderClass(44);
        }
        #endregion


        #region Passing parameters by reference
        [Test]
        public void PassingByReference_ValueTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            var valueType = new IntHolderStruct(5);
            F5(ref valueType);

            Assert.AreEqual(55, valueType.Number);
        }
        private void F5(ref IntHolderStruct valueType)
        {
            valueType.Number = 55;
        }

        public void PassingByReference_ValueTypeParameter_ReassigningTheParameter_ShouldChangeCallerValue()
        {
            var valueType = new IntHolderStruct(5);
            F6(ref valueType);

            Assert.AreEqual(66, valueType.Number);
        }
        private void F6(ref IntHolderStruct valueType)
        {
            valueType = new IntHolderStruct(66);
        }

        [Test]
        public void PassingByReference_ReferenceTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            var referenceType = new IntHolderClass(5);
            F7(ref referenceType);

            Assert.AreEqual(77, referenceType.Number);
        }
        private void F7(ref IntHolderClass referenceType)
        {
            referenceType.Number = 77;
        }

        [Test]
        public void PassingByReference_ReferenceTypeParameter_ReassigningTheParameter_ShouldChangeCallerValue()
        {
            var referenceType = new IntHolderClass(5);
            F8(ref referenceType);

            Assert.AreEqual(88, referenceType.Number);
        }
        private void F8(ref IntHolderClass referenceType)
        {
            referenceType = new IntHolderClass(88);
        }
        #endregion


        #region Passing parameters as output
        [Test]
        public void PassingAsOutput_ValueTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            var valueType = new IntHolderStruct(5);
            F9(out valueType);

            Assert.AreEqual(99, valueType.Number);
        }
        private void F9(out IntHolderStruct valueType)
        {
            valueType.Number = 99;
        }

        [Test]
        public void PassingAsOutput_ValueTypeParameter_ReassigningTheParameter_ShouldChangeCallerValue()
        {
            IntHolderStruct valueType;
            F10(out valueType);

            Assert.AreEqual(1010, valueType.Number);
        }
        private void F10(out IntHolderStruct valueType)
        {
            valueType = new IntHolderStruct(1010);
        }

        [Test]
        public void PassingAsOutput_ReferenceTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            IntHolderClass referenceType = new IntHolderClass(5);
            F11(out referenceType);

            Assert.Pass("referenceType variable must be assigned in the F11(), so we can't just change value of Number property.");
        }
        private void F11(out IntHolderClass referenceType)
        {
            referenceType = new IntHolderClass(0);
        }

        [Test]
        public void PassingAsOutput_ReferenceTypeParameter_ReassigningTheParameter_ShouldChangeCallerValue()
        {
            IntHolderClass referenceType = new IntHolderClass(5);
            F12(out referenceType);

            Assert.AreEqual(1212, referenceType.Number);
        }
        private void F12(out IntHolderClass referenceType)
        {
            referenceType = new IntHolderClass(1212);
        }
        #endregion


        #region Passing parameters as parameter array
        [Test]
        public void PassingAsParameterArray_ValueTypeParameter_ChangingTheValueInsideFunction_ShouldNotChangeCallerValue()
        {
            var valueTypes = new[] { new IntHolderStruct(5) };
            F13(valueTypes);
            
            Assert.AreEqual(5, valueTypes.First().Number);
        }
        private void F13(params IntHolderStruct[] valueTypes)
        {
            valueTypes.ToList().ForEach(i => i.Number = 1313);
        }

        [Test]
        public void PassingAsParameterArray_ValueTypeParameter_ReassigningTheParameter_ShouldNotChangeCallerValue()
        {
            var valueTypes = new[] { new IntHolderStruct(5) };
            F14(valueTypes);

            Assert.AreEqual(5, valueTypes.First().Number);
        }
        private void F14(params IntHolderStruct[] valueTypes)
        {
            valueTypes.ToList().ForEach(i => i = new IntHolderStruct(1414));
        }

        [Test]
        public void PassingAsParameterArray_ReferenceTypeParameter_ChangingTheValueInsideFunction_ShouldChangeCallerValue()
        {
            var referenceTypes = new[] { new IntHolderClass(5) };
            F15(referenceTypes);

            Assert.AreEqual(1515, referenceTypes.First().Number);
        }
        private void F15(params IntHolderClass[] referenceTypes)
        {
            referenceTypes.ToList().ForEach(i => i.Number = 1515);
        }

        [Test]
        public void PassingAsParameterArray_ReferenceTypeParameter_ReassigningTheParameter_ShouldNotChangeCallerValue()
        {
            var referenceTypes = new[] { new IntHolderClass(5) };
            F16(referenceTypes);

            Assert.AreEqual(5, referenceTypes.First().Number);
        }
        private void F16(params IntHolderClass[] referenceTypes)
        {
            referenceTypes.ToList().ForEach(i => i = new IntHolderClass(1616));
        }
        #endregion


        #region Other
        [Test]
        public void PassingReferenceTypeByValue_AndValueTypeByReference_IsNotTheSame()
        {
            var firstValueType = new IntHolderStruct(5);
            var secondValueType = firstValueType;
            secondValueType.Number = 6;
            F101(ref firstValueType);
            Assert.AreEqual(101101, firstValueType.Number);
            Assert.AreEqual(6, secondValueType.Number);
           

            var firstReferenceType = new IntHolderClass(5);
            var secondReferenceType = firstReferenceType;
            secondReferenceType.Number = 6;
            F102(firstReferenceType);

            Assert.AreEqual(6, firstReferenceType.Number);
            Assert.AreEqual(6, secondReferenceType.Number);
            Assert.AreEqual(firstReferenceType, secondReferenceType);
        }
        private void F101(ref IntHolderStruct valueType)
        {
            valueType = new IntHolderStruct(101101);
        }
        private void F102(IntHolderClass referenceType)
        {
            referenceType = new IntHolderClass(102102);
        }
        #endregion
    }
}
