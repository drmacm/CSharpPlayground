using System;
using System.Globalization;

namespace NHibernateInAction.CaveatEmptor.Model
{
	[Serializable]
	public class MonetaryAmount
	{
	    public virtual string Currency { get; }
	    public virtual double Value { get; }

		public MonetaryAmount(double value, string currency)
		{
			Value = value;
			Currency = currency;
		}

	    public override bool Equals(object o)
	    {
	        if(this == o)
	            return true;

	        if(!(o is MonetaryAmount))
	            return false;

	        MonetaryAmount monetaryAmount = (MonetaryAmount) o;

	        if(Currency != monetaryAmount.Currency)
	            return false;

	        if(Value != monetaryAmount.Value)
	            return false;

	        return true;
	    }

	    public override int GetHashCode()
	    {
	        int result;
	        result = Value.GetHashCode();
	        result = 29*result + Currency.GetHashCode();
	        return result;
	    }

	    public override string ToString()
	    {
	        return "Value: '" + Value + "', " + "Currency: '" + Currency + "'";
	    }

	    public virtual int CompareTo(object o)
	    {
	        if(o is MonetaryAmount)
	        {
	            // TODO: This would actually require some currency conversion magic
	            return this.Value.CompareTo(((MonetaryAmount) o).Value);
	        }
	        return 0;
	    }

	    public static MonetaryAmount FromString(string amount, string currency)
	    {
	        return new MonetaryAmount(double.Parse(amount, NumberStyles.Any), currency);
	    }

	    public static MonetaryAmount Convert(MonetaryAmount amount, string toConcurrency)
	    {
	        // TODO: This requires some conversion magic and is therefore broken
	        return new MonetaryAmount(amount.Value, toConcurrency);
	    }
	}
}