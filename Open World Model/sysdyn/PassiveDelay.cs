using System;
using System.Collections.Generic;

namespace OpenWorldModel
{
	public class PassiveDelay : TemporalVariable
	{
		protected TemporalVariable source;
		protected double dt;
		
		protected Queue<KeyValuePair<double, double>> pastValues; // time -> value

		public PassiveDelay(TemporalVariable source, double dt)
			: base("L[" + source.Name + "]", source.Dimensions)
		{
			this.source = source;
			this.dt = dt;
			
			pastValues = new Queue<KeyValuePair<double, double>>();
			
			source.Updated += SourceUpdated;
		}
		
		public void SourceUpdated(object src, double time, double result) {
			if (source != src)
				throw new ArgumentException("Unexpected source!");
			
			if (dt == 0)
				pastValues.Clear();
			pastValues.Enqueue(new KeyValuePair<double, double>(time, result));
		}
		
		protected override double EvaluateInternal(double time)
		{
			if (dt == 0) {
				if (pastValues.Count > 0)
					return pastValues.Peek().Value;
				
				return source.Evaluate(source.CurrentTime);
			}
			
			// Drop ancient values
			while (pastValues.Count > 0 && pastValues.Peek().Key < time - dt)
				pastValues.Dequeue();
									
			// Check if we already have this time
			if (time - dt <= 0 || pastValues.Peek().Key == time - dt)
				return pastValues.Peek().Value;
			
			throw new ArgumentOutOfRangeException("Could not find a recorded past value");			
		}
	}
}

