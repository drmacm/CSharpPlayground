using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace General
{
    /// <summary>
    /// Exercise related to covariance and contravariance for array types, delegate types
    /// and generic type arguments.
    /// Inspired by 11-part series on this topic, written by Eric Lippert, which can be found here:
    /// https://blogs.msdn.microsoft.com/ericlippert/tag/covariance-and-contravariance/
    /// First part of the series is here:
    /// https://blogs.msdn.microsoft.com/ericlippert/2007/10/16/covariance-and-contravariance-in-c-part-one/
    /// </summary>
    public class CovarianceAndContravariance
    {
        #region Classes
        public class Animal { }
        public class Mammal : Animal { }
        public class Reptile : Animal { }
        public class Giraffe : Mammal { }
        public class Tiger : Mammal { }
        public class Snake : Reptile { }
        public class Turtle : Reptile { }
        #endregion

        #region Delegates
        delegate Mammal MammalCreator();
        delegate void MammalHolder(Mammal mammal);

        delegate R MammalProcessor<in T, out R>(T t);

        delegate void GenericHolder<in T>(T value);         //T is contravariant here (this is the same as Action<T>
        delegate void Meta<out T>(GenericHolder<T> holder); //T is covariant here! (this is a "doubly contravariant" position, which makes it covariant)
        #endregion

        #region Interfaces
        public interface IBase<U>
        {
            IDerived<U> MakeDerived();
        }
        public interface IDerived<T> : IBase<T>
        {

        }
        #endregion

        #region Actions for delegates
        static Giraffe MakeGiraffe() { return new Giraffe(); }
        static Mammal MakeMammal() { return new Mammal(); }
        static Animal MakeAnimal() { return new Animal(); }
        static void HoldGiraffe(Giraffe giraffe) { }
        static void HoldMammal(Mammal mammal) { }
        static void HoldAnimal(Animal animal) { }
        #endregion

        /// <summary>https://blogs.msdn.microsoft.com/ericlippert/2007/10/17/covariance-and-contravariance-in-c-part-two-array-covariance/</summary>
        [Test]
        public void Covariance_ArraysOfReferenceTypes_FailsOnlyAtRuntimeBecauseOfBadDesignChoiceFromCSharp1()
        {
            Animal[] animals = new Mammal[10];

            Assert.Throws<ArrayTypeMismatchException>(() => animals[0] = new Snake());
        }

        /// <summary>
        /// <see cref="MammalCreator"/> delegate represents a function that returns <see cref="Mammal"/>, but the actual function 
        /// returns <see cref="Giraffe"/>. Users of this function expect Mammal and it's ok if they get a "smaller" type than Mammal.
        /// 
        /// Therefore, converstions from method groups to delegates are covariant in their return types.
        /// </summary>
        [Test]
        public void Covariance_MethodGroupToDelegate_CanReturnSameOrSmallerType()
        {
            MammalCreator mammalCreator1 = MakeMammal;
            MammalCreator mammalCreator2 = MakeGiraffe;

            Mammal mammal1 = mammalCreator1();
            Mammal mammal2 = mammalCreator2();

            Assert.NotNull(mammal1);
            Assert.NotNull(mammal2);
        }

        /// <summary>
        /// Delegate represents a function that takes <see cref="Mammal"/>, but the actual function takes <see cref="Animal"/>. 
        /// Users of this function will pass in Mammal or a "smaller" type, so the method must know how to work with it. 
        /// It means that the method assigned to a delegate must work with Mammal or "bigger" type, otherwise we may pass a Tiger 
        /// to method that expects Giraffe. In this case the direction of assignability is reversed:
        /// Mammal is smaller than Animal, so method that takes Animal is smaller than method that takes Mammal.
        /// 
        /// Because of this reversal, conversions from method groups to delegates are contravariant in their argument types. 
        /// </summary>
        [Test]
        public void Contravariance_MethodGroupToDelegate_CanTakeSameOrBiggerType()
        {
            Tiger mammalToHold = new Tiger();

            MammalHolder mammalHolder1 = HoldMammal;
            MammalHolder mammalHolder2 = HoldAnimal;

            mammalHolder1(mammalToHold);
            mammalHolder2(mammalToHold);

            Assert.Pass("If this line executes, means that contravariance works as expected :)");
        }

        /// <summary>
        /// Since C# 4, covariance and contravariance work with the type expressions as well, 
        /// not just with conversions of method groups to delegates.
        /// </summary>
        [Test]
        public void Variance_DelegatesWithTypeExpressions()
        {
            MammalProcessor<Animal, Giraffe> first = (a) => { return new Giraffe(); };
            MammalProcessor<Mammal, Mammal> second = first;

            var mammal = second(new Tiger());
            Assert.NotNull(mammal);
        }

        /// <summary>
        /// In case of higher-order functions (function that takes a delegate), 
        /// if the delegate passed as argument has contravariant input parameters, the delegate itself becames covariant,
        /// although it's an input parameter.
        /// </summary>
        [Test]
        public void Variance_HigherOrderFunctions()
        {
            //GenericHolder itself is **contravariant** on the T parameter (like an Action<T>)
            GenericHolder<Animal> animalHolder = (a) => { Console.WriteLine(a.ToString()); };
            GenericHolder<Mammal> mammalHolder = (m) => { Console.WriteLine(m.ToString()); };
            GenericHolder<Giraffe> giraffeHolder = (g) => { Console.WriteLine(g.ToString()); };
            animalHolder(new Animal());
            animalHolder(new Mammal());
            animalHolder(new Giraffe());
            mammalHolder(new Mammal());
            mammalHolder(new Giraffe());
            giraffeHolder(new Giraffe());
            giraffeHolder = mammalHolder = animalHolder;
            
            //Meta is **covariant** on the input parameter of type GenericHolder
            Meta<Giraffe> giraffeMeta = (GenericHolder<Giraffe> holder) => holder(new Giraffe());
            Meta<Mammal> mammalMeta = (GenericHolder<Mammal> holder) => holder(new Mammal());
            Meta<Animal> animalMeta = (GenericHolder<Animal> holder) => holder(new Animal());
            giraffeMeta(giraffeHolder);
            giraffeMeta(mammalHolder);
            giraffeMeta(animalHolder);
            mammalMeta(mammalHolder);
            mammalMeta(animalHolder);
            animalMeta(animalHolder);
            animalMeta = mammalMeta = giraffeMeta;

            Assert.Pass("If this line executes, means that contravariance works as expected :)");
        }


    }
}
