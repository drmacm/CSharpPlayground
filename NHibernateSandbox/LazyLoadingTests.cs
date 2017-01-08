using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernateSandbox.Entities;
using NUnit.Framework;

namespace NHibernateSandbox
{
    [TestFixture]
    public class LazyLoadingTests
    {
        private ISession session;

        [SetUp]
        public void SetUp()
        {
            session = Bootstrap.SessionFactory.OpenSession();
        }

        [TearDown]
        public void TearDown()
        {
            session.Dispose();
        }

        [Test]
        public void LazyLoading_Get_AssociatedPropertiesAreNotLoaded()
        {
            var result = session.Get<Store>(1);

            Assert.False(NHibernateUtil.IsInitialized(result.Employees));
            Assert.False(NHibernateUtil.IsInitialized(result.Products));
        }

        [Test]
        public void LazyLoading_AccessingPropertyAfterSessionIsClosed_ShouldThrow()
        {
            var result = session.Get<Store>(1);
            session.Dispose();

            Assert.Throws<LazyInitializationException>(() =>
            {
                var name = result.Employees.First().FirstName;
            });
        }

        [Test]
        public void SelectNPlus1()
        {
            var fromDb = session.Get<Store>(1);
            foreach (var employee in fromDb.Employees)
            {
                Console.WriteLine(employee.FirstName);
            }
        }
    }
}
