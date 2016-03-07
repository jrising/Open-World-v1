using System;
using System.Collections.Generic;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class Stock : TemporalVariable
	{
		protected static double dt = .1;
		
		protected List<TemporalVariable> args = new List<TemporalVariable>();
		protected double level;
		
		public Stock(string name, double initial, IDimensions dims)
			: base(name, dims)
		{
			this.level = initial;
		}

		public double Level {
			get {
				return level;
			}
		}

		public void Add(double x) {
			level += x;
		}
		
		public void SetArguments(List<TemporalVariable> args) {
			this.args = args;
		}
		
		// Dynamics
		public override double Evaluate(double time)
		{
			if (this.time == time)
				return level;
			return base.Evaluate(time);
		}
		
		protected override double EvaluateInternal(double time)
		{
			for (double tt = this.time; tt <= time - dt; tt += Math.Min(dt, time - dt)) {
				double delta = 0;
				foreach (TemporalVariable arg in args)
					delta += arg.Evaluate(tt);
				if (tt == time - dt) {
					Add(dt * delta);
					break;
				} else
					Add(Math.Min(dt, time - dt - tt) * delta);
				this.time = Math.Min(tt + dt, time);
			}
			
			return level;
		}
		
		// Define connections
		public void IncreasesBy(TemporalVariable x) {
			if (!dims.DividedBy(GlobalDimensions.Time).Equals(x.Dimensions))
				throw new ArgumentException("Dimension mismatch");
			args.Add(x);
		}
		
		public void DecreasesBy(TemporalVariable x) {
			if (!dims.DividedBy(GlobalDimensions.Time).Equals(x.Dimensions))
				throw new ArgumentException("Dimension mismatch");
			args.Add(-x);
		}
	}
}

