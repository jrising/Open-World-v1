using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public abstract class MemoizedVariable : TemporalVariable
	{
		protected double value;
		
		public MemoizedVariable(string name, IDimensions dims)
			: base(name, dims)
		{
			value = double.NaN;
		}
		
		public override double Evaluate(double time)
		{
			if (!double.IsNaN(value) && this.time == time)
				return value;
			return base.Evaluate(time);
		}

		protected override double EvaluateInternal(double time)
		{
			if (double.IsNaN(value) || this.time != time)
				value = EvaluateUpdate(time);
			
			return value;
		}
		
		protected abstract double EvaluateUpdate(double time);
		
		public double Value {
			get {
				return value;
			}
		}
	}
}

