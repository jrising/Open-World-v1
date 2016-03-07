using System;
using System.Collections.Generic;

namespace OpenWorldModel
{
	public class Node
	{
		protected string name;
		protected List<Edge> edges;
		
		public Node(string name)
		{
			this.name = name;
			edges = new List<Edge>();
		}
		
		public void Dispose() {
			RemoveAllEdges();
		}
		
		public string Name {
			get {
				return name;
			}
		}
		
		public void To(Node dest, double weight) {
			Edge between = new Edge(this, dest, weight);
			edges.Add(between);
		}

		public List<Edge> Edges {
			get {
				return edges;
			}
		}
		
		public bool ConnectedTo(Node node) {
			foreach (Edge edge in edges)
				if (edge.Origin == node || edge.Destination == node)
					return true;
			
			return false;
		}
		
		public void RemoveEdge(Edge edge) {
			edges.Remove(edge);
		}
		
		public void RemoveAllEdges() {
			while (edges.Count > 0)
				edges[0].Dispose();
			
			edges.Clear();
		}
	}
}