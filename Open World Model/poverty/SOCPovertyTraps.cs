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

			// First get this working like SolowModel, then move into SOCSolowModel
			Stock labor = new SimplePopulationStock("Labor (L)", 10000, .01 * timescale);
			TemporalVariable firmLabor = labor / new Constant("Firms", count, Dimensionless.Instance);

			CircleGraph firms = new CircleGraph(count, 4);

			firmCapital = new NodeStock("Capital (K)", firms.GraphStack, new NodeConstant("initial", firms.GraphStack, NodeRandomInitial, GlobalDimensions.get("k")));

			TemporalVariable firmEffectiveLabor = firmLabor * (firms.NodeEdgeCount() + 1);
			TemporalVariable firmProduction = new Function("Production (Y)", x => timescale * Math.Pow(count*x[0], x[2]) * Math.Pow(count*x[1], (1 - x[2])),
			                                   GlobalDimensions.get("k"), firmCapital, firmEffectiveLabor, new Constant("Preference (alpha)", .3, Dimensionless.Instance));

			Constant savingsRate = new Constant("Savings Rate (s)", .1, GlobalDimensions.Time.RaisedTo(-1));

			firms.AddAction(new StepNodeAction(.1 * timescale, AddRandomEdge)); // technological advancement
			firms.AddAction(new StepNodeAction(GrowOrDie, new PassiveDelay(firmProduction, 1), savingsRate, firmCapital, new Constant("Depreciation Rate (delta)", .1 * timescale, GlobalDimensions.Time.RaisedTo(-1))));

			//TemporalVariable capitalPerEffectiveLaborer = firms.Sum(firmCapital) / firms.Sum(firmEffectiveLabor);
			TemporalVariable capitalPerLaborer = firms.Sum(firmCapital) / firms.Sum(firmLabor);
			TemporalVariable productionPerLaborer = firms.Sum(firmProduction) / firms.Sum(firmLabor);

			double[,] result = new double[1000, 4];
			for (int tt = 0; tt < 1000; tt++) {
				collapses = 0;
				result[tt, 0] = capitalPerLaborer.Evaluate(tt);
				result[tt, 1] = (1 - savingsRate.Evaluate(tt)) * productionPerLaborer.Evaluate(tt);
				result[tt, 2] = labor.Evaluate(tt);
				result[tt, 3] = collapses;
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
			double growth =  args[0] * args[1]; // production * savings
			double decay = args[2] * args[3]; // capital * depeciation

			double collapse = args[3];
			if (growth < decay) {
				collapse = (decay - growth) / args[2];
				growth = 0; // Stalled Growth (dK=0)
			}
			if (randgen.NextDouble() < collapse) {
				node.RemoveAllEdges();
				growth = -args[2] + NodeRandomInitial(node, 0);
				collapses++;
			}

			firmCapital.Add(node, growth);
		}

		/* Plans for making it SOC:
		 * 1. Add poverty trap so at low k, low returns to labor (e.g., A = f(k))
		 * 2. Lay out system where variables become events-- tech at each node inc. with network;
		 * depreciation happens spontaneously; and when it does it might reflect a node leaving the network,
		 * thereby destabilizing other nodes by removing their technology and capacity to maintain their own
		 * capital
		*/
	}
}

