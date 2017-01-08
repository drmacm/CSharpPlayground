using System;
using NHibernate.Mapping.Attributes;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    [Class(Table = "COMMENTS")]
    public class Comment : IComparable
    {
        [Id(0, Name = "Id", Column = "COMMENT_ID")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }
        [Version(Column = "VERSION")]
        public virtual int Version { get; set; }
        // Simple property mapped with an enum
        [Property(Update = false, NotNull = true, Column = "RATING")]
        public virtual Rating Rating { get; set; }
        [Property(Column = "COMMENT_TEXT", Length = 4000)]
        public virtual string Text { get; set; }
        [ManyToOne(Column = "FROM_USER_ID", OuterJoin = OuterJoinStrategy.True, Update = false, NotNull = true)]
        public virtual User FromUser { get; set; }
        // A simple uni-directional one-to-many association to item.
        [ManyToOne(Column = "ITEM_ID", OuterJoin = OuterJoinStrategy.True, Update = false, NotNull = true)]
        public virtual Item Item { get; set; }
        [Property(Column = "CREATED")]
        public virtual DateTime Created { get; set; }

        internal Comment()
        {
            Created = SystemTime.NowWithoutMilliseconds;
        }

        public Comment(Rating rating, string text, User fromUser, Item item) : this()
        {
            Rating = rating;
            Text = text;
            FromUser = fromUser;
            Item = item;
        }

        public virtual int CompareTo(object o)
        {
            if (o is Comment)
            {
                return Created.CompareTo(((Comment) o).Created);
            }
            return 0;
        }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is Comment))
                return false;

            var comment = (Comment) o;

            if (Created != comment.Created)
                return false;
            if (Rating != comment.Rating)
                return false;
            if (Text != null ? Text != comment.Text : comment.Text != null)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = Rating.GetHashCode();
            result = 29*result + (Text != null ? Text.GetHashCode() : 0);
            result = 29*result + Created.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "Comment ('" + Id + "'), " + "Rating: '" + Rating + "'";
        }
    }
}