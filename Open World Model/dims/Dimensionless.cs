using System;
using System.Collections.Generic;

namespace OpenWorldModel.Dimensions
{
	public class Dimensionless : IDimensions
	{
		private static Dimensionless instance;
		
		private Dimensionless()
		{
		}
		
		public static Dimensionless Instance {
			get {
				if (instance == null)
					instance = new Dimensionless();
				return instance;
			}
		}
		
		public IDimensions RaisedTo(double power)
		{
			return this;
		}
		
		public IDimensions Times (IDimensions other)
		{
			return other;
		}
		
		public IDimensions DividedBy (IDimensions other)
		{
			return other.RaisedTo(-1);
		}
		
		public bool Equals(IDimensions other)
		{
			return this == other;
		}
		
		public KeyValuePair<IDimensions, double>[] Factors {
			get {
				return null;
			}
		}
		
		public override string ToString ()
		{
			return "";
		}
	}
}

