using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class SimplePopulationStock : Stock
	{
		protected double growth;
		
		public SimplePopulationStock(string name, double initial, double growth)
			: base(name, initial, GlobalDimensions.get("person"))
		{
			this.level = initial;
			this.growth = growth;
		}
		
		protected override double EvaluateInternal(double time)
		{
			level *= Math.Exp(growth * (time - this.time));
			return level;
		}
	}
}

