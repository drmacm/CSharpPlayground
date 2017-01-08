using System;
using System.Collections.Generic;
using NHibernate.Mapping.Attributes;
using NHibernateInAction.CaveatEmptor.Exceptions;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "USERS")]
    public class User : IComparable
    {
        [Id(0, Name = "Id", Column = "USER_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Version(Column = "VERSION")]
        public virtual int Version { get; set; }
        [Property(Column = "FIRST_NAME", NotNull = true)]
        public virtual string Firstname { get; set; }
        [Property(Column = "LAST_NAME", NotNull = true)]
        public virtual string Lastname { get; set; }
        [Property(Column = "USERNAME", Update = false, NotNull = true, Length = 16)]
        public virtual string Username { get; set; }
        [Property(Column = "`PASSWORD`", NotNull = true)]
        public virtual string Password { get; set; }
        [Property(Column = "EMAIL", NotNull = true)]
        public virtual string Email { get; set; }
        [Property(Column = "RANKING", NotNull = true)]
        public virtual int Ranking { get; set; }
        [Property(Column = "IS_ADMIN", NotNull = true)]
        public virtual bool IsAdmin { get; set; }
        [Property(Column = "CREATED", Update = false, NotNull = true)]
        public virtual DateTime Created { get; set; }
        [ComponentProperty]
        public virtual Address HomeAddress { get; set; }
        /// <summary>This is mapped by the BillingAddressComponent below</summary>
        public virtual Address BillingAddress { get; set; }
        //The default billing strategy, may be null if no BillingDetails exist.
        [ManyToOne(Column = "DEFAULT_BILLING_DETAILS_ID", NotNull = false, OuterJoin = OuterJoinStrategy.False, ForeignKey = "FK1_DEFAULT_BILLING_DETAILS_ID")]
        public virtual BillingDetails DefaultBillingDetails { get; set; }
        [Set(0, Inverse = true, Cascade = "None")]
        [Key(1)]
        [Column(2, Name = "SELLER_ID", NotNull = true)]
        [OneToMany(3, ClassType = typeof(Item))]
        public virtual ISet<Item> Items { get; set; }
        [Set(0, Inverse = true, Cascade = "all-delete-orphan")]
        [Key(1)]
        [Column(2, Name = "USER_ID", NotNull = true)]
        [OneToMany(3, ClassType = typeof(BillingDetails))]
        public virtual ISet<BillingDetails> BillingDetails { get; set; }

        protected User()
        {
            Items = new HashSet<Item>();
            BillingDetails = new HashSet<BillingDetails>();
            Created = SystemTime.NowWithoutMilliseconds;
        }

        public User(string firstname, string lastname, string username, string password, string email, Address address,
            ISet<Item> items, ISet<BillingDetails> billingDetails): this()
        {
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Password = password;
            Email = email;
            HomeAddress = address;
            Items = items;
            BillingDetails = billingDetails;
        }

        public User(string firstname, string lastname, string username, string password, string email) : this()
        {
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Password = password;
            Email = email;
        }



        public virtual void AddItem(Item item)
        {
            if (item == null)
                throw new ArgumentException("Can't add a null Item.");
            Items.Add(item);
        }

        /// <summary> 
        /// Adds a BillingDetails to the set. 
        /// This method checks if there is only one billing method in the set, then makes this the default.
        /// </summary>
        public virtual void AddBillingDetails(BillingDetails billingDetails)
        {
            if (billingDetails == null)
                throw new ArgumentException("Can't add a null BillingDetails.");

            bool added = BillingDetails.Add(billingDetails);
            if (!added)
                throw new ArgumentException("Duplicates not allowed");

            if (BillingDetails.Count == 1)
            {
                DefaultBillingDetails = billingDetails;
            }
        }

        /// <summary> 
        /// Removes a BillingDetails from the set.
        /// This method checks if the removed is the default element, and will throw a BusinessException if there is more than 
        /// one left to chose from. This might actually not be the best way to handle this situation.
        /// </summary>
        public virtual void RemoveBillingDetails(BillingDetails billingDetails)
        {
            if (billingDetails == null)
                throw new ArgumentException("Can't remove a null BillingDetails.");

            if (BillingDetails.Count >= 2)
            {
                BillingDetails.Remove(billingDetails);
                DefaultBillingDetails = BillingDetails.GetEnumerator().Current;
            }
            else
            {
                throw new BusinessException("Please set new default BillingDetails first");
            }
        }

        public virtual int CompareTo(object o)
        {
            if (o is User)
                return Created.CompareTo(((User) o).Created);
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;

            if (!(o is User))
                return false;

            var user = (User) o;

            if (Username != user.Username)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

        public override string ToString()
        {
            return "User ('" + Id + "'), " + "Username: '" + Username + "'";
        }

        public virtual void IncreaseRanking()
        {
            Ranking = Ranking + 1;
        }

        /// <summary>
        /// These class allow use of Mapping Attributes to work  with components. Not that this isn't a very elegant solution.
        /// </summary>
        [Component(Name = "BillingAddress")]
        private class BillingAddressComponent : Address
        {
            [Property(Column = "BILLING_CITY", Name = "City")] [Property(Column = "BILLING_STREET", Name = "Street")] [Property(Column = "BILLING_ZIP_CODE", Name = "Zipcode")] private string ingoreMe;
        }
    }
}