using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class Constant : TemporalVariable
	{
		protected double constant;

		public Constant(string name, double constant, IDimensions dims)
			: base(name, dims)
		{
			this.constant = constant;
		}
				
		protected override double EvaluateInternal(double time)
		{
			return constant;
		}
	}
}

