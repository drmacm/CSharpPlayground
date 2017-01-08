using System;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Class(Table = "BID")]
    [Serializable]
    public class Bid : IComparable
    {
        [Id(0, Name = "Id", Column = "BID_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Version(Column = "VERSION")]
        public virtual int Version { get; set; }

        [Property(Update = false, TypeType = typeof(MonetaryAmountCompositeUserType))]
        [Column(1, Name = "INITIAL_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "INITIAL_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount Amount { get; set; }

        /// <remarks>The other side of this bidirectional one-to-many association to item.</remarks>
        [ManyToOne(Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.False, Column = "ITEM_ID")]
        public virtual Item Item { get; set; }

        /// <remarks> The other side of this bidirectional one-to-many association to user.</remarks>
        [ManyToOne(Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.True, Column = "BIDDER_ID")]
        public virtual User Bidder { get; set; }

        /// <remarks>We can't change the creation time, so map it with update="false".</remarks>
        [Property(Update = false, NotNull = true, Column = "CREATED")]
        public virtual DateTime Created { get; set; }

     
        protected Bid()
        {
            Created = SystemTime.NowWithoutMilliseconds;
        }

        public Bid(MonetaryAmount amount, Item item, User bidder) : this()
        {
            Amount = amount;
            Item = item;
            Bidder = bidder;
        }

        public virtual int CompareTo(object o)
        {
            if (o is Bid)
            {
                return Created.CompareTo(((Bid) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is Bid))
                return false;

            var bid = (Bid) o;

            //TODO: Why isn't != working in Bid Equals()
            //Below, != doesn't work (I'm missing something I know)
            if (! Amount.Equals(bid.Amount))
                return false;

            if (Created != bid.Created)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = Amount.GetHashCode();
            result = 29*result + Created.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "Bid ('" + Id + "'), " + "Created: '" + Created.ToString("r") + "' " + "Amount: '" + Amount + "'";
        }
    }
}