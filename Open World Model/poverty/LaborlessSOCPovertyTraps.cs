using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class SOCPovertyTraps
	{
		protected static NodeStock firmCapital;
		protected static Random randgen = new Random();
		protected static int collapses;

		public static double[,] Run()
		{
			const int count = 100;
			const double timescale = 1.0 / 4;

			CircleGraph firms = new CircleGraph(count, 4);

			firmCapital = new NodeStock("Capital (K)", firms.GraphStack, new NodeConstant("initial", firms.GraphStack, NodeRandomInitial, GlobalDimensions.get("k")));

			TemporalVariable firmProduction = new Function("Production (Y)", x => timescale * x[0] * Math.Pow(x[1], x[2]),
			                                   GlobalDimensions.get("k"), (firms.NodeEdgeCount() + 1), firmCapital, new Constant("Preference (alpha)", .3, Dimensionless.Instance));

			Constant savingsRate = new Constant("Savings Rate (s)", .1, GlobalDimensions.Time.RaisedTo(-1));

			firms.AddAction(new StepNodeAction(.1 * timescale, AddRandomEdge)); // technological advancement
			firms.AddAction(new StepNodeAction(GrowOrDie, new PassiveDelay(firmProduction, 1), savingsRate, firmCapital, new Constant("Depreciation Rate (delta)", .1 * timescale, GlobalDimensions.Time.RaisedTo(-1))));

			double[,] result = new double[1000, 3];
			for (int tt = 0; tt < 1000; tt++) {
				collapses = 0;
				result[tt, 0] = firms.Sum(firmCapital).Evaluate(tt);
				result[tt, 1] = (1 - savingsRate.Evaluate(tt)) * firms.Sum(firmProduction).Evaluate(tt);
				result[tt, 2] = collapses;
			}

			return result;
		}

		public static double NodeRandomInitial(Node node, double time) {
			return randgen.NextDouble() * 100 * 10000 * 5 / 100; // avg. (K / AL) * AL / #firms
		}

		public static void AddRandomEdge(CircleGraph graph, Node node, double[] vars) {
			List<Node> others = new List<Node>();
			if (node.ConnectedTo(graph.Adjacent(node, 1)))
				others.Add(graph.Adjacent(node, 1));
			if (node.ConnectedTo(graph.Adjacent(node, -1)))
				others.Add(graph.Adjacent(node, -1));
			if (node.ConnectedTo(graph.Adjacent(node, 2)))
				others.Add(graph.Adjacent(node, 2));
			if (node.ConnectedTo(graph.Adjacent(node, -2)))
				others.Add(graph.Adjacent(node, -2));

			Node rand = null;
			do
				rand = graph.RandomNode();
			while (rand == node || others.Contains(rand));

			others.Add(rand);

			node.To(others[randgen.Next(others.Count)], 1.0);
		}

		public static void GrowOrDie(CircleGraph graph, Node node, double[] args) {
			double growth = args[0] * args[1]; // production * savings
			double decay = args[2] * args[3]; // capital * depeciation

			if (growth < decay)
				growth = 0; // Stalled Growth (dK=0)

            double collapse = decay / (args[2] + growth);
			if (randgen.NextDouble() < collapse) {
				node.RemoveAllEdges();
				growth = -args[2] + NodeRandomInitial(node, 0);
				collapses++;
			}

			firmCapital.Add(node, growth);
		}
	}
}

