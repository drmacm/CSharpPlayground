using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;
using NHibernateSandbox.Entities;
using NUnit.Framework;

namespace NHibernateSandbox
{
  public static class Extensions
  {
    public static T RandomElement<T>(this IEnumerable<T> q)
    {
      var r = new Random();
      return q.Skip(r.Next(q.Count())).FirstOrDefault();
    }
  }

  [SetUpFixture]
  public class Bootstrap
  {
    private static Configuration configuration;
    public static ISessionFactory SessionFactory;

    [OneTimeSetUp]
    public void SetUp()
    {
      configuration = Configure();
      SessionFactory = configuration.BuildSessionFactory();

      //GenerateTables();
      //InsertData();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
      //DeleteData();
      //NHibernateProfiler.Stop();
    }

    private Configuration Configure()
    {
      var connectionString = @"server=(LocalDb)\MSSQLLocalDB;database=NHibernateExample;Connection Timeout=300";

      return Fluently.Configure()
      .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
      .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Product>())
      .BuildConfiguration();
    }

    private void GenerateTables()
    {
      var schemaExport = new SchemaExport(configuration);
      schemaExport.Execute(true, true, false);
    }

    private void InsertData()
    {
      using (var session = SessionFactory.OpenSession())
      {
        using (var transaction = session.BeginTransaction())
        {
          var smallStore = Builder<Store>.CreateNew().With(s => s.Id, 1).With(s => s.Name = "Small store").Build();

          var locations = Builder<Location>.CreateListOfSize(5).All().Build();
          var products = Builder<Product>.CreateListOfSize(100)
            .All()
            .With(p => p.Id, 0)
            .With(p => p.Location = locations.RandomElement())
            .Build();
          var employees = Builder<Employee>.CreateListOfSize(10)
            .All()
            .With(e => e.Id, 0)
            .Build();

          products.ForEach(p => smallStore.AddProduct(p));
          employees.ForEach(e => smallStore.AddEmployee(e));

          session.Save(smallStore);
          transaction.Commit();
        }
      }
    }

    private void DeleteData()
    {
      using (var session = SessionFactory.OpenSession())
      {
        session.Delete("from Product p");
        session.Delete("from Employee e");
        session.Delete("from Store s");
        session.Flush();
      }
    }
  }
}
