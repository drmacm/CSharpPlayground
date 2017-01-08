using System;

namespace NHibernateInAction.CaveatEmptor.Model
{
    [Serializable]
    public enum Rating
    {
        Excellent,
        Ok,
        Low,
    }

    [Serializable]
    public enum ItemState
    {
        Draft,
        Pending,
        Active,
    }

    [Serializable]
    public enum CreditCardType
    {
        MasterCard,
        Visa,
        Amex,
    }
}
