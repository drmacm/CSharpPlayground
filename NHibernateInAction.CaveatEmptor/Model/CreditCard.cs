using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    // CreditCard subclass mapping to its own table, normalized. CreditCard is immutable, we map all properties with update="false"
    [JoinedSubclass(0, Table = "CREDIT_CARD", ExtendsType = typeof (BillingDetails))]
    [Key(2, Column = "CREDIT_CARD_ID", ForeignKey = "FK1_CREDIT_CARD_ID")]
    public class CreditCard : BillingDetails
    {
        [Property(Update = false, NotNull = true, Column = "CC_TYPE")]
        public virtual CreditCardType Type { get; set; }
        [Property(NotNull = true, Column = "CC_NUMBER", Length = 16)]
        public virtual string Number { get; set; }
        [Property(Update = false, NotNull = true, Column = "EXP_MONTH", Length = 2)]
        public virtual string ExpMonth { get; set; }
        [Property(Update = false, NotNull = true, Column = "EXP_YEAR", Length = 4)]
        public virtual string ExpYear { get; set; }

        internal CreditCard()
        {
        }

        public CreditCard(string ownerName, User user, string number, CreditCardType type, string expMonth, string expYear) : base(ownerName, user)
        {
            Type = type;
            Number = number;
            ExpMonth = expMonth;
            ExpYear = expYear;
        }

        public override string ToString()
        {
            return "CreditCard ('" + Id + "'), " + "Type: '" + Type + "'";
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is CreditCard))
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

        public override bool Valid
        {
            get
            {
                return true;
            }
        }
    }
}