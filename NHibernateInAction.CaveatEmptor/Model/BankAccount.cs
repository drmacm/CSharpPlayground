using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    //BankAccount subclass mapping to its own table, normalized.
    [JoinedSubclass(0, Table = "BANK_ACCOUNT", ExtendsType = typeof (BillingDetails))]
    [Key(2, Column = "BANK_ACCOUNT_ID", ForeignKey = "FK1_BANK_ACCOUNT_ID")]
    public class BankAccount : BillingDetails
    {
        [Property(Column = "ACCOUNT_NUMBER", NotNull = true, Length = 16)]
        public virtual string Number { get; set; }
        [Property(Column = "BANK_NAME", NotNull = true, Length = 255)]
        public virtual string BankName { get; set; }
        [Property(Column = "BANK_SWIFT", NotNull = true, Length = 15)]
        public virtual string BankSwift { get; set; }

        internal BankAccount()
        {
        }

        public BankAccount(string ownerName, User user, string number, string bankName, string bankSwift)
            : base(ownerName, user)
        {
            Number = number;
            BankName = bankName;
            BankSwift = bankSwift;
        }

        public override string ToString()
        {
            return "BankAccount ('" + Id + "'), " + "Number: '" + Number + "'";
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is BankAccount))
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