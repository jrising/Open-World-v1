using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class RandomVariable : MemoizedVariable
	{
		protected double min;
		protected double max;

		protected Random randgen = new Random();

		public RandomVariable(string name, double min, double max, IDimensions dims)
			: base(name, dims)
		{
			this.min = min;
			this.max = max;
		}
		
		protected override double EvaluateUpdate(double time)
		{
			return (max - min) * randgen.NextDouble() + min;
		}
	}
}

