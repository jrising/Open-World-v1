using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class Function : MemoizedVariable
	{
		public delegate double Evaluator(double[] args);
		
		protected Evaluator function;
		protected TemporalVariable[] args;
		
		public Function(string name, Evaluator function, IDimensions dims, params TemporalVariable[] args)
			: base(name, dims)
		{
			this.function = function;
			this.args = args;
		}
				
		protected override double EvaluateUpdate(double time)
		{
			double[] vals = new double[args.Length];
			for (int ii = 0; ii < args.Length; ii++) {
				vals[ii] = args[ii].Evaluate(time);
			}
			
			return function(vals);
		}
	}
}

