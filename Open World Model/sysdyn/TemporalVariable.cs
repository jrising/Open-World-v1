using System;
using System.Collections.Generic;
using System.Text;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public abstract class TemporalVariable : Variable
	{
		protected bool evaluating = false;
		protected double time = 0;
		
		public delegate void UpdateHandler(object sender, double time, double value);
		public event UpdateHandler Updated;
		
		protected static Stack<Variable> evalstack = new Stack<Variable>();

		public TemporalVariable(string name, IDimensions dims)
			: base(name, dims)
		{
		}
		
		public double CurrentTime {
			get {
				return time;
			}
		}
		
		public virtual double Evaluate(double time) {
			if (time < this.time)
				throw new ArgumentOutOfRangeException("Time before now");
			if (evaluating) {
				// Construct trace
				StringBuilder trace = new StringBuilder();
				trace.AppendLine(Name);
				foreach (Variable v in evalstack.ToArray())
					trace.AppendLine(v.Name);
				throw new Exception("Reciprical definitions: " + trace.ToString());
			}
				                                   
			this.evaluating = true;
			evalstack.Push(this);
			double result = EvaluateInternal(time);
			this.time = time;
			if (evalstack.Pop() != this)
				throw new Exception("Out of order iterations");
			this.evaluating = false;
			
			if (Updated != null)
				Updated(this, time, result);
			
			return result;
		}
		
		protected abstract double EvaluateInternal(double time);
		
		public static TemporalVariable operator-(TemporalVariable a) {
			return new Function("-" + a.Name, x => -x[0], a.Dimensions, a);
		}
		
		public static TemporalVariable operator+(TemporalVariable a, TemporalVariable b) {
			if (!a.Dimensions.Equals(b.Dimensions))
				throw new ArgumentException("Dimensions mismatch to +");
			return new Function("(" + a.Name + " + " + b.Name + ")", x => x[0] + x[1], a.Dimensions, a, b);
		}
		
		public static TemporalVariable operator+(TemporalVariable a, double b) {
			if (!a.Dimensions.Equals(Dimensionless.Instance))
				throw new ArgumentException("Dimensions mismatch to + #");
			return new Function("(" + a.Name + " + " + b + ")", x => x[0] + b, a.Dimensions, a);
		}
		
		public static TemporalVariable operator-(TemporalVariable a, TemporalVariable b) {
			if (!a.Dimensions.Equals(b.Dimensions))
				throw new ArgumentException("Dimensions mismatch to -");
			return new Function("(" + a.Name + " - " + b.Name + ")", x => x[0] - x[1], a.Dimensions, a, b);
		}
		
		public static TemporalVariable operator*(TemporalVariable a, TemporalVariable b) {
			return new Function(a.Name + " " + b.Name, x => x[0] * x[1], a.Dimensions.Times(b.Dimensions), a, b);
		}

		public static TemporalVariable operator/(TemporalVariable a, TemporalVariable b) {
			return new Function("(" + a.Name + ")/(" + b.Name + ")", x => x[0] / x[1], a.Dimensions.DividedBy(b.Dimensions), a, b);
		}

	}
}

