using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class NodeConstant : NodeVariable
	{
		protected Dictionary<Node, double> values;
		
		// only if constval initializer is used
		protected double constval;
		
		public NodeConstant(string name, Stack<Node> graphstack, NodeEvaluate evaluate, IDimensions dims)
			: base(name, graphstack, evaluate, dims)
		{
			values = new Dictionary<Node, double>();
		}
		
		public NodeConstant(Stack<Node> graphstack, double constval, IDimensions dims)
			: base(constval.ToString(), graphstack, NotImplementedEvaluate, dims)
		{
			values = new Dictionary<Node, double>();
			this.constval = constval;
			this.evaluate = ConstantEvaluate;
		}
		
		protected double ConstantEvaluate(Node node, double time) {
			return constval;
		}
		
		protected override double EvaluateInternal(double time)
		{
			Node node = graphstack.Peek();
			double value = 0;
			if (values.TryGetValue(node, out value))
				return value;

			return evaluate(node, time);
		}
	}
}

