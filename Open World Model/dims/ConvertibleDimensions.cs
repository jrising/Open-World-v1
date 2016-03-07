using System;
using System.Collections.Generic;

namespace OpenWorldModel.Dimensions
{
	public class ConvertibleDimensions<T> : AbstractDimensions
	{
		public delegate double Converter(T value);
		
		protected Converter converter;
		
		public ConvertibleDimensions(string name, Converter converter, KeyValuePair<IDimensions, double>[] factors)
			: base(name)
		{
			this.converter = converter;
			this.factors = factors;
		}
				
		public double Convert(T value) {
			return converter(value);
		}
	}
}

