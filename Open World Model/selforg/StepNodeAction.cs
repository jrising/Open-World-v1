using System;
namespace OpenWorldModel
{
	public class StepNodeAction
	{
		public delegate void NodeAction(CircleGraph graph, Node node, double[] args);
		
		protected double prob;
		protected NodeAction action;
		protected TemporalVariable[] args;
		
		protected Random randgen;

		public StepNodeAction(NodeAction action, params TemporalVariable[] args)
		{
			this.prob = 1.0;
			this.action = action;
			this.args = args;
			
			randgen = new Random();
		}

		public StepNodeAction(double prob, NodeAction action, params TemporalVariable[] args)
		{
			this.prob = prob;
			this.action = action;
			this.args = args;

			randgen = new Random();
		}
		
		public void Perform(CircleGraph graph, Node node, double time) {
			if (prob == 1.0 || randgen.NextDouble() <= prob) {
				double[] vals = new double[args.Length];
				for (int ii = 0; ii < args.Length; ii++)
					vals[ii] = args[ii].Evaluate(time);
				
				action(graph, node, vals);
			}
		}
	}
}

