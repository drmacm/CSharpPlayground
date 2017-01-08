using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "CATEGORIZED_ITEM", Lazy = true)]
    public class CategorizedItem : IComparable
    {
        [CompositeId(1, Name = "Id", UnsavedValue = UnsavedValueType.Any)]
        [KeyProperty(2, Name = "categoryId", Access = "field")]
        [KeyProperty(3, Name = "itemId", Access = "field")]
        public virtual CategorizedItemId Id { get; set; }
        [Property(Column = "USERNAME", Update = false, NotNull = true)]
        public virtual string Username { get; set; }
        [Property(Column = "DATE_ADDED", Update = false, NotNull = true)]
        public virtual DateTime DateAdded { get; set; }
        [ManyToOne(Column = "CATEGORY_ID", Update = false, NotNull = true)]
        public virtual Category Category { get; set; }
        [ManyToOne(Column = "ITEM_ID", Update = false, NotNull = true)]
        public virtual Item Item { get; set; }

        internal CategorizedItem()
        {
            DateAdded = SystemTime.NowWithoutMilliseconds;
        }

        public CategorizedItem(string username, Category category, Item item) : this()
        {
            Username = username;
            Category = category;
            Item = item;
            Id = new CategorizedItemId(category.Id, item.Id);

            // Guarantee referential integrity
            category.CategorizedItems.Add(this);
            item.CategorizedItems.Add(this);
        }

        public virtual int CompareTo(object o)
        {
            // CategorizedItems are sorted by date
            if (o is CategorizedItem)
                return DateAdded.CompareTo(((CategorizedItem) o).DateAdded);
            return 0;
        }

        public override string ToString()
        {
            return "Added by: '" + Username + "', " + "On Date: '" + DateAdded.ToString("r");
        }

     }

    [Serializable]
    public class CategorizedItemId
    {
        private readonly long categoryId;
        private readonly long itemId;

        public CategorizedItemId()
        {
        }


        public CategorizedItemId(long categoryId, long itemId)
        {
            this.categoryId = categoryId;
            this.itemId = itemId;
        }

        public override bool Equals(object o)
        {
            if (o is CategorizedItemId)
            {
                var that = (CategorizedItemId) o;
                return categoryId == that.categoryId && itemId == that.itemId;
            }
            return false;
        }


        public override int GetHashCode()
        {
            return categoryId.GetHashCode() + itemId.GetHashCode();
        }
    }
}