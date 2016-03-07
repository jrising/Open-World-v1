using System;
namespace OpenWorldModel
{
	public class Edge
	{
		protected Node orig;
		protected Node dest;
		protected double weight;
		
		public Edge(Node orig, Node dest, double weight)
		{
			this.orig = orig;
			this.dest = dest;
			this.weight = weight;
		}
		
		public void Dispose() {
			orig.RemoveEdge(this);
			dest.RemoveEdge(this);
		}
		
		public Node Origin {
			get {
				return orig;
			}
		}
		
		public Node Destination {
			get {
				return dest;
			}
		}
	}
}

