using System;
using System.Collections.Generic;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "CATEGORY")]
    public class Category : IComparable
    {
        [Id(0, Name = "Id", Column = "CATEGORY_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Property(0)]
        [Column(1, Name = "NAME", NotNull = true, Length = 255, UniqueKey = "UNIQUE_NAME_AT_LEVEL")]
        public virtual string Name { get; set; }
        [ManyToOne(0, OuterJoin = OuterJoinStrategy.False, ForeignKey = "FK1_PARENT_CATEGORY_ID")]
        [Column(1, Name = "PARENT_CATEGORY_ID", NotNull = false)]
        public virtual Category ParentCategory { get; set; }
        /// <remarks>
        /// We use a Set for this bidirectional one-to-many. Batch fetching is particulary interesting for  this association: 
        /// We expect that the application will need much more childCategories if it accesses one. 
        /// Batch fetching can significantly improve fetching of the whole category graph.
        /// </remarks>
        [Set(0, Cascade = "all-delete-orphan", Inverse = true, BatchSize = 10)]
        [Key(1, Column = "PARENT_CATEGORY_ID")]
        [OneToMany(2, ClassType = typeof(Category))]
        public virtual ISet<Category> ChildCategories { get; set; }
        /// <remarks>
        /// We use a one-to-many association to express the relationship to a set of items. 
        /// There is an intermediate entity class, CategorizedItem, which in fact makes this a many-to-many association between Category and Item. 
        /// </remarks>
        [Set(0, Cascade = "all-delete-orphan", Inverse = true, OuterJoin = OuterJoinStrategy.False)]
        [Key(1, ForeignKey = "FK1_CATEGORIZED_ITEM_ID")]
        [Column(2, Name = "CATEGORY_ID", NotNull = true, Length = 16)]
        [OneToMany(3, ClassType = typeof(CategorizedItem))]
        public virtual ISet<CategorizedItem> CategorizedItems { get; set; }
        [Property(Update = false, NotNull = true, Column = "CREATED")]
        public virtual DateTime Created { get; set; }

        protected Category()
        {
            CategorizedItems = new HashSet<CategorizedItem>();
            ChildCategories = new HashSet<Category>();
            Created = SystemTime.NowWithoutMilliseconds;
        }

        public Category(string name, Category parentCategory, ISet<Category> childCategories, ISet<CategorizedItem> categorizedItems) : this()
        {
            Name = name;
            ParentCategory = parentCategory;
            ChildCategories = childCategories;
            CategorizedItems = categorizedItems;
        }

        public Category(string name) : this()
        {
            Name = name;
        }
    
        public virtual void AddChildCategory(Category category)
        {
            if (category == null)
                throw new ArgumentException("Can't add a null Category as child.");
            // Remove from old parent category
            if (category.ParentCategory != null)
                category.ParentCategory.ChildCategories.Remove(category);
            // Set parent in child
            category.ParentCategory = this;
            // Set child in parent
            ChildCategories.Add(category);
        }

        public virtual void AddCategorizedItem(CategorizedItem catItem)
        {
            if (catItem == null)
                throw new ArgumentException("Can't add a null CategorizedItem.");
            CategorizedItems.Add(catItem);
        }

        public virtual int CompareTo(object o)
        {
            if (o is Category)
            {
                return string.CompareOrdinal(Name, ((Category) o).Name);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Category))
                return false;

            var category = (Category) o;

            if (Created != category.Created)
                return false;
            if (Name != null ? Name != category.Name : category.Name != null)
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
            return "Category ('" + Id + "'), " + "Name: '" + Name + "'";
        }
    }
}