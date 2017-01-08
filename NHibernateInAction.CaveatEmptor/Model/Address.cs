using System;
using System.Diagnostics.CodeAnalysis;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Component(Update = false)]
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public class Address
    {
        [Property(Column = "STREET")]
        public virtual string Street { get; set; }
        [Property(Column = "ZIP_CODE")]
        public virtual string Zipcode { get; set; }
        [Property(Column = "CITY")]
        public virtual string City { get; set; }

        internal Address()
        { }

        public Address(string street, string zipcode, string city)
        {
            Street = street;
            Zipcode = zipcode;
            City = city;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Address))
                return false;

            var address = (Address) o;

            if (City != address.City)
                return false;
            if (Street != address.Street)
                return false;
            if (Zipcode != address.Zipcode)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            var result = Street.GetHashCode();
            result = 29*result + Zipcode.GetHashCode();
            result = 29*result + City.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "Street: '" + Street + "', " + "Zipcode: '" + Zipcode + "', " + "City: '" + City + "'";
        }
    }
}