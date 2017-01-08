using System;
using System.Collections.Generic;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Exceptions;
using NHibernateInAction.CaveatEmptor.Persistence;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "ITEM", Lazy = false)]
    public class Item : IComparable, IAuditable
    {
        [Id(0, Name = "Id", Column = "ITEM_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Version(Column = "VERSION")]
        public virtual int Version { get; set; }
        [Property(Column = "NAME", Length = 255, NotNull = true, Update = false)]
        public virtual string Name { get; set; }
        [Property(Column = "DESCRIPTION", Length = 4000, NotNull = true)]
        public virtual string Description { get; set; }
        [Property(TypeType = typeof(MonetaryAmountCompositeUserType))]
        [Column(1, Name = "INITIAL_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "INITIAL_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount InitialPrice { get; set; }
        [Property(TypeType = typeof(MonetaryAmountCompositeUserType))]
        [Column(1, Name = "RESERVE_PRICE", NotNull = true, Length = 2)]
        [Column(2, Name = "RESERVE_PRICE_CURRENCY", NotNull = true)]
        public virtual MonetaryAmount ReservePrice { get; set; }
        [Property(Column = "START_DATE", NotNull = true, Update = false)]
        public virtual DateTime StartDate { get; set; }
        [Property(Column = "END_DATE", NotNull = true, Update = false)]
        public virtual DateTime EndDate { get; set; }
        [Property(Column = "STATE", NotNull = true)]
        public virtual ItemState State { get; set; }
        [Property(Column = "APPROVAL_DATETIME", NotNull = false)]
        public virtual DateTime? ApprovalDatetime { get; set; }
        [Property(Column = "CREATED", Update = false, NotNull = true)]
        public virtual DateTime Created { get; set; }
        [ManyToOne(Column = "APPROVED_BY_USER_ID", ForeignKey = "FK1_APPROVED_BY_USER_ID")]
        public virtual User ApprovedBy { get; set; }
        [ManyToOne(Column = "SELLER_ID", Update = false, NotNull = true, OuterJoin = OuterJoinStrategy.False)]
        public virtual User Seller { get; set; }
        [ManyToOne(Column = "SUCCESSFUL_BID_ID", OuterJoin = OuterJoinStrategy.False, NotNull = false)]
        public virtual Bid SuccessfulBid { get; set; }

        /*We use a one-to-many association to express the relationship
	     to a set of categories. There is an intermediate entity class,
	     CategorizedItem, which in fact makes this a many-to-many
	     association between Item and Category.*/
        [Set(0, Inverse = true, Lazy = CollectionLazy.True, Cascade = "all-delete-orphan")]
        [Key(1, ForeignKey = "FK2_CATEGORIZED_ITEM_ID")]
        [Column(2, Name = "ITEM_ID", NotNull = true, Length = 16)]
        [OneToMany(3, ClassType = typeof(CategorizedItem))]
        public virtual ISet<CategorizedItem> CategorizedItems { get; set; }

        [Bag(0, OrderBy = "CREATED desc", Inverse = true, Cascade = "All", Name = "Bids")]
        [Key(1, Column = "ITEM_ID")]
        [OneToMany(2, ClassType = typeof(Bid))]
        public virtual IList<Bid> Bids { get; set; }

        [Query(Name = "maxBid",
            Content = "select b from Bid b where b.Amount.Value = (select max(b.Amount.Value) from Bid b where b.Item.Id = :itemid)"
            )] private long id;

        protected Item()
        {
            Bids = new List<Bid>();
            CategorizedItems = new HashSet<CategorizedItem>();
            Created = SystemTime.NowWithoutMilliseconds;
        }

        public Item(string name, string description, User seller, MonetaryAmount initialPrice,
                    MonetaryAmount reservePrice, DateTime startDate, DateTime endDate, IList<Bid> bids,
                    Bid successfulBid) : this(name, description, seller, initialPrice, reservePrice, startDate, endDate)
        {
            Bids = bids;
            SuccessfulBid = successfulBid;
        }

        public Item(string name, string description, User seller, MonetaryAmount initialPrice,
                    MonetaryAmount reservePrice, DateTime startDate, DateTime endDate) : this()
        {
            Name = name;
            Seller = seller;
            Description = description;
            InitialPrice = initialPrice;
            ReservePrice = reservePrice;
            StartDate = startDate;
            EndDate = endDate;
            State = ItemState.Draft;
        }

        public virtual void AddCategorizedItem(CategorizedItem catItem)
        {
            if (catItem == null)
                throw new ArgumentException("Can't add a null CategorizedItem.");
            CategorizedItems.Add(catItem);
        }

        public virtual void AddBid(Bid bid)
        {
            if (bid == null)
                throw new ArgumentException("Can't add a null Bid.");

            Bids.Add(bid);
        }

        public virtual int CompareTo(object o)
        {
            if (o is Item)
            {
                return Created.CompareTo(((Item) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is Item))
                return false;

            var item = (Item) o;

            if (Created != item.Created)
                return false;

            if (Name != null ? Name != item.Name : item.Name != null)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = (Name != null ? Name.GetHashCode() : 0);
            result = 29*result + Created.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "Item ('" + Id + "'), " + "Name: '" + Name + "' " + "Initial Price: '" + InitialPrice + "'";
        }

        /// <summary> Places a bid while checking business constraints.
        /// This method may throw a BusinessException if one of the requirements
        /// for the bid placement wasn't met, e.g. if the auction already ended.
        /// </summary>
        public virtual Bid PlaceBid(User bidder, MonetaryAmount bidAmount, Bid currentMaxBid, Bid currentMinBid)
        {
            // Check highest bid (can also be a different Strategy (pattern))
            if (currentMaxBid != null && currentMaxBid.Amount.CompareTo(bidAmount) > 0)
            {
                throw new BusinessException("Bid too low.");
            }

            // Auction is active
            if (State != ItemState.Active)
                throw new BusinessException("Auction is not active yet.");

            // Auction still valid
            if (EndDate < DateTime.Now)
                throw new BusinessException("Can't place new bid, auction already ended.");

            // Create new Bid
            var newBid = new Bid(bidAmount, this, bidder);

            // Place bid for this Item
            AddBid(newBid);

            return newBid;
        }

        /// <summary> Anyone can set this item into PENDING state for approval.</summary>
        public virtual void SetPendingForApproval()
        {
            State = ItemState.Pending;
        }

        /// <summary> Approve this item for auction and set its state to active. </summary>
        public virtual void Approve(User byUser)
        {
            if (!byUser.IsAdmin)
                throw new PermissionException("Not an administrator.");

            if (State != ItemState.Pending)
                throw new BusinessException("Item still in draft.");

            State = ItemState.Active;
            ApprovedBy = byUser;
            ApprovalDatetime = SystemTime.NowWithoutMilliseconds;
        }
    }
}