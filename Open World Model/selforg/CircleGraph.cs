using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class CircleGraph
	{
		protected Node[] nodes;
		protected Stack<Node> evalstack;

		protected List<StepNodeAction> stepNodeActions;
		
		// keeps track of time
		protected HeartBeatVariable heartbeat;
		
		public CircleGraph(int n, int e)
		{
			nodes = new Node[n];
			for (int i = 0; i < n; i++)
				nodes[i] = new Node(i.ToString());
			
			for (int i = 0; i < n; i++) {
				for (int j = 1; j < e; j++) {
					nodes[i].To(nodes[(i + j) % n], 1);
					nodes[i].To(nodes[(n + i - j) % n], 1);
				}
			}
			
			this.evalstack = new Stack<Node>();
			this.heartbeat = new HeartBeatVariable(this);
			this.stepNodeActions = new List<StepNodeAction>();
		}
		
		public Stack<Node> GraphStack {
			get {
				return evalstack;
			}
		}
		
		public void AddAction(StepNodeAction action) {
			stepNodeActions.Add(action);
		}
		
		public Node Adjacent(Node node, int num) {
			int ii = Array.IndexOf(nodes, node);
			return nodes[(nodes.Length + ii + num) % nodes.Length];
		}
		
		public Node RandomNode() {
			Random randgen = new Random();
			return nodes[randgen.Next(nodes.Length)];
		}
		
		// Nodal Variables
		
		public NodeVariable NodeEdgeCount() {
			return new NodeVariable("#edges", evalstack, NodeEdgeCountEvaluate, Dimensionless.Instance);
		}
		
		public double NodeEdgeCountEvaluate(Node node, double time) {
			heartbeat.Evaluate(time);
			return node.Edges.Count;
		}
		
		public TemporalVariable Sum(TemporalVariable bynode) {
			return new GraphVariable("Sum " + bynode.Name, SumEvaluate, bynode.Dimensions, bynode);
		}

		public double SumEvaluate(TemporalVariable[] args, double time) {
			double total = 0;
			foreach (Node node in nodes) {
				evalstack.Push(node);
				total += args[0].Evaluate(time);
				if (evalstack.Pop() != node)
					throw new Exception("Node stack out of order");
			}

			return total;
		}
		
		// Required Functions
		
		public void Heartbeat(double time) {
			foreach (StepNodeAction action in stepNodeActions) {
				foreach (Node node in nodes) {
					evalstack.Push(node);
					action.Perform(this, node, time);
					if (evalstack.Pop() != node)
						throw new Exception("Node stack out of order");
				}
			}
		}
	}
}

