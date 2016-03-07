using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class GraphVariable : TemporalVariable
	{
		public delegate double GraphEvaluate(TemporalVariable[] args, double time);

		private GraphEvaluate evaluate;
		private TemporalVariable[] args;

		public GraphVariable(string name, GraphEvaluate evaluate, IDimensions dims, params TemporalVariable[] args)
			: base(name, dims)
		{
			this.evaluate = evaluate;
			this.args = args;
		}		
		
		protected override double EvaluateInternal(double time)
		{
			return evaluate(args, time);
		}
	}
}

