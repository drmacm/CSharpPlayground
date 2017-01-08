namespace NHibernateInAction.CaveatEmptor.Model
{
    public class UserId
    {
        private string organizationId;


        public UserId(string username, string organizationId)
        {
            Username = username;
            OrganizationId = organizationId;
        }

        public virtual string Username { get; set; }

        public virtual string OrganizationId { get; set; }

        public override bool Equals(object o)
        {
            if (this == o)
                return true;
            if (!(o is UserId))
                return false;

            var userId = (UserId) o;

            if (organizationId != userId.organizationId)
                return false;
            if (Username != userId.Username)
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            int result;
            result = Username.GetHashCode();
            result = 29*result + organizationId.GetHashCode();
            return result;
        }
    }
}