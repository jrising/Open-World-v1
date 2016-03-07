using System;
using System.Collections.Generic;

namespace OpenWorldModel
{
	public class Delay : TemporalVariable
	{
		protected TemporalVariable source;
		protected double dt;
		
		protected Queue<KeyValuePair<double, double>> pastValues; // time -> value

		public Delay(TemporalVariable source, double dt)
			: base("L[" + source.Name + "]", source.Dimensions)
		{
			this.source = source;
			this.dt = dt;
			
			pastValues = new Queue<KeyValuePair<double, double>>();
		}
		
		public void Record(double time) {
			// Record value before Evaluate?
			KeyValuePair<double, double>[] pastArray = pastValues.ToArray();
			if (pastValues.Count == 0 || pastArray[pastValues.Count - 1].Key != source.CurrentTime)
				pastValues.Enqueue(new KeyValuePair<double, double>(source.CurrentTime, source.Evaluate(source.CurrentTime)));
			
			if (source.CurrentTime != time)
				pastValues.Enqueue(new KeyValuePair<double, double>(time, source.Evaluate(time)));
		}
		
		protected override double EvaluateInternal(double time)
		{
			// Add evaluation of the actual time to queue
			Record(time);

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

