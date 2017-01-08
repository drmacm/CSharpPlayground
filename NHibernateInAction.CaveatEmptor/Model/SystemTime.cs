using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
    public class SystemTime
    {
        private static DateTime? FakeTime;

        public static DateTime Now
        {
            get { return FakeTime.HasValue ? FakeTime.Value : DateTime.Now; }
        }

        public static DateTime NowWithoutMilliseconds
        {
            get
            {
                DateTime now = Now;
                return new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second
                    );
            }
        }

        public static void Fake(DateTime fakeDateTime)
        {
            FakeTime = fakeDateTime;
        }
    }
}