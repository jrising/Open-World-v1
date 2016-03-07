using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class NodeStock : NodeVariable
	{
		protected Dictionary<Node, Stock> stocks;
		protected List<TemporalVariable> args;
		protected NodeConstant initial;

		public NodeStock(string name, Stack<Node> graphstack, double initial, IDimensions dims)
			: base(name, graphstack, NotImplementedEvaluate, dims)
		{
			stocks = new Dictionary<Node, Stock>();
			args = new List<TemporalVariable>();
			this.initial = new NodeConstant(graphstack, initial, dims);
			this.evaluate = StockEvaluate;
		}
		
		public NodeStock(string name, Stack<Node> graphstack, NodeConstant initial)
			: base(name, graphstack, NotImplementedEvaluate, initial.Dimensions)
		{
			stocks = new Dictionary<Node, Stock>();
			args = new List<TemporalVariable>();
			this.initial = initial;
			this.evaluate = StockEvaluate;
		}
		
		protected double StockEvaluate(Node node, double time) {
			return GetStock(node).Evaluate(time);
		}
		
		public void Add(Node node, double x) {
			GetStock(node).Add(x);
		}
		
		public Stock GetStock(Node node) {
			if (!stocks.ContainsKey(node)) {
				stocks[node] = new Stock(name + "[" + node.Name + "]", time, dims);
				stocks[node].SetArguments(args);
			}
			
			return stocks[node];
		}
	}
}

