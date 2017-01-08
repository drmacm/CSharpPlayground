using NHibernate;
using NHibernateSandbox.Entities;
using NUnit.Framework;

namespace NHibernateSandbox
{
  [TestFixture]
  public class QueryOverTests
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
    public void SimpleSelect()
    {
      var result = session.QueryOver<Store>()
        .Where(s => s.Name == "Small store")
        .Select(s => s.Id)
        .SingleOrDefault<int>();

      Assert.AreEqual(1, result);
    }

    [Test]
    public void Transformation_DistinctRootEntity()
    {
      var result = session.QueryOver<Store>()
        .Where(s => s.Name == "Small store")
        .JoinQueryOver<Product>(s => s.Products).Where(p => p.Id < 20)
        .SingleOrDefault();

      Assert.NotNull(result);
    }
  }
}
