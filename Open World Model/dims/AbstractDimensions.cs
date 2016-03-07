using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWorldModel.Dimensions
{
	public class AbstractDimensions : IDimensions
	{
		// An instance represents a particular substance
		// will have one or the other of the following
		protected string name;
		protected KeyValuePair<IDimensions, double>[] factors;

		protected AbstractDimensions(string name) 
		{
			this.name = name;
			this.factors = null;
		}

		protected AbstractDimensions(KeyValuePair<IDimensions, double>[] factors) {
			this.name = null;
			this.factors = factors;
		}
		
		protected AbstractDimensions(string name, KeyValuePair<IDimensions, double>[] factors) {
			this.name = name;
			this.factors = factors;
		}
		
		public override string ToString()
		{
			if (name != null)
				return name;
			else {
				StringBuilder result = new StringBuilder();
				foreach (KeyValuePair<IDimensions, double> factor in factors) {
					if (factor.Key.Factors != null)
						result.AppendFormat("({0})", factor.Key.ToString());
					else
						result.AppendFormat("{0}", factor.Key.ToString());
					if (factor.Value != 1)
						result.AppendFormat("^{0}", factor.Value);
				}
				
				return result.ToString();
			}
		}
		
		public KeyValuePair<IDimensions, double>[] Factors {
			get {
				return factors;
			}
		}
		
		public bool Equals(IDimensions other)
		{
			// TODO: Improve so order doesn't matter
			if (name != null)
				return name == other.ToString();
				
			for (int ii = 0; ii < factors.Length; ii++)
				if (!factors[ii].Key.Equals(other.Factors[ii].Key) || factors[ii].Value != other.Factors[ii].Value)
					return false;
			
			return true;
		}
		
		public IDimensions Times(IDimensions other)
		{
			// TODO: Improve so combines like
			List<KeyValuePair<IDimensions, double>> newfactors = new List<KeyValuePair<IDimensions, double>>();

			if (name != null)
				newfactors.Add(new KeyValuePair<IDimensions, double>(this, 1));
			else
				newfactors.AddRange(factors);
			
			if (other.Factors != null)
				newfactors.AddRange(other.Factors);
			else
				newfactors.Add(new KeyValuePair<IDimensions, double>(other, 1));

			return new GlobalDimensions(newfactors.ToArray());
		}
		
		public IDimensions RaisedTo(double power) {
			List<KeyValuePair<IDimensions, double>> newfactors = new List<KeyValuePair<IDimensions, double>>();
			if (factors == null)
				newfactors.Add(new KeyValuePair<IDimensions, double>(this, power));
			else {
				foreach (KeyValuePair<IDimensions, double> factor in factors)
					newfactors.Add(new KeyValuePair<IDimensions, double>(factor.Key, factor.Value * power));
			}
			
			return new GlobalDimensions(newfactors.ToArray());
		}
		
		public IDimensions DividedBy(IDimensions other)
		{
			return this.Times(other.RaisedTo(-1));
		}
	}
}

