using FluentNHibernate.Mapping;

namespace NHibernateSandbox.Entities
{
  public class Employee
  {
    public virtual int Id { get; protected set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual Store Store { get; set; }
  }

  public class EmployeeMap : ClassMap<Employee>
  {
    public EmployeeMap()
    {
      Id(x => x.Id);
      Map(x => x.FirstName);
      Map(x => x.LastName);
      References(x => x.Store);
    }
  }
}