using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "CUSTOMER_LOCATION")]
    public class CustomerLocation
    {
        [CompositeId]
        [KeyProperty(1, Name = "One", Column = "CUSTOMER_LOCATION_ONE", Length = 100)]
        [KeyProperty(2, Name = "Two", Column = "CUSTOMER_LOCATION_TWO", Length = 100)]
        public virtual string One { get; set; }
        public virtual string Two { get; set; }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is CustomerLocation))
                return false;

            var customerLocation = (CustomerLocation) o;

            if (One != null ? One != customerLocation.One : customerLocation.One != null)
                return false;
            if (Two != null ? Two != customerLocation.Two : customerLocation.Two != null)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = (One != null ? One.GetHashCode() : 0);
            result = 29*result + (Two != null ? Two.GetHashCode() : 0);
            return result;
        }
    }
}