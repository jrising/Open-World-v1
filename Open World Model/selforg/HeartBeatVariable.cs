using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class HeartBeatVariable : TemporalVariable
	{
		protected CircleGraph graph;
		protected double lastHeartbeat;
		
		public HeartBeatVariable(CircleGraph graph)
			: base("heartbeat", Dimensionless.Instance)
		{
			this.graph = graph;
			this.lastHeartbeat = 0;
		}
		
		public override double Evaluate(double time)
		{
			if (time - lastHeartbeat < 1)
				return lastHeartbeat;
			
			return base.Evaluate(time);
		}
		
		protected override double EvaluateInternal(double time)
		{
			while (time - lastHeartbeat >= 1) {
				lastHeartbeat++;
				graph.Heartbeat(lastHeartbeat);
			}
			
			return lastHeartbeat;
		}
	}
}

