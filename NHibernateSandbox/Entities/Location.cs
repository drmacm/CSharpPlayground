using FluentNHibernate.Mapping;

namespace NHibernateSandbox.Entities
{
  public class Location
  {
    public virtual int Aisle { get; set; }
    public virtual int Shelf { get; set; }
  }

  public class LocationMap : ComponentMap<Location>
  {
    public LocationMap()
    {
      Map(x => x.Aisle);
      Map(x => x.Shelf);
    }
  }
}
