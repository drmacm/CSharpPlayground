using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace NHibernateSandbox.Entities
{
  public class Product
  {
    public virtual int Id { get; protected set; }
    public virtual string Name { get; set; }
    public virtual double Price { get; set; }
    public virtual Location Location { get; set; }
    public virtual IList<Store> StoresStockedIn { get; set; }

    public Product()
    {
      StoresStockedIn = new List<Store>();
    }
  }

  public class ProductMap : ClassMap<Product>
  {
    public ProductMap()
    {
      Id(x => x.Id);
      Map(x => x.Name);
      Map(x => x.Price);
      HasManyToMany(x => x.StoresStockedIn)
        .Cascade.All()
        .Inverse()
        .Table("StoreProduct");

      Component(x => x.Location);
    }
  }
}