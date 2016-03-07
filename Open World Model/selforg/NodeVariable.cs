using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class NodeVariable : TemporalVariable
	{
		public delegate double NodeEvaluate(Node node, double time);
		
		protected Stack<Node> graphstack;
		protected NodeEvaluate evaluate;
		
		public NodeVariable(string name, Stack<Node> graphstack, NodeEvaluate evaluate, IDimensions dims)
			: base(name, dims)
		{
			this.graphstack = graphstack;
			this.evaluate = evaluate;
		}
		
		protected static double NotImplementedEvaluate(Node node, double time) {
			throw new NotImplementedException();
		}

		protected override double EvaluateInternal(double time)
		{
			return evaluate(graphstack.Peek(), time);
		}
	}
}

