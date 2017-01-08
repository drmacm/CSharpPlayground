using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "BILLING_DETAILS")]
    public abstract class BillingDetails : IComparable
    {
        [Id(0, Name = "Id", Column = "BILLING_DETAILS_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Property(NotNull = true, Column = "OWNER_NAME")]
        public virtual string OwnerName { get; set; }
        /// <remarks>
        /// Bidirectional, required as BillingDetails is USER_ID NOT NULL. This is also a read-only property that will never be updated.
        /// </remarks>
        [ManyToOne(Update = false, OuterJoin = OuterJoinStrategy.False, Column = "USER_ID")]
        public virtual User User { get; set; }
        [Property(Update = false, NotNull = true, Column = "CREATED")]
        public virtual DateTime Created { get; set; }
        [Version(Column = "VERSION")]
        public virtual int Version { get; set; }
        /// <summary>  Checks if the billing information is correct. Check algorithm is implemented in subclasses.</summary>
        public abstract bool Valid { get; }


        protected BillingDetails()
        {
            Created = SystemTime.NowWithoutMilliseconds;
        }

        protected internal BillingDetails(string ownerName, User user) : this()
        {
            OwnerName = ownerName;
            User = user;
        }

        public virtual int CompareTo(object o)
        {
            // Billing Details are simply sorted by creation date
            if (o is BillingDetails)
                return Created.CompareTo(((BillingDetails) o).Created);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is BillingDetails))
                return false;

            var billingDetails = (BillingDetails) o;

            if (Created != billingDetails.Created)
                return false;
            if (OwnerName != billingDetails.OwnerName)
                return false;

            return true;
        }


        public override int GetHashCode()
        {
            int result;
            result = Created.GetHashCode();
            result = 29*result + OwnerName.GetHashCode();
            return result;
        }
    }
}