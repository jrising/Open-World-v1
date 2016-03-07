using System;
namespace OpenWorldModel
{
	public class DividedRange
	{
		protected double min;
		protected double max;
		protected double widths;
		
		public DividedRange(double min, double max, double widths)
		{
			this.min = min;
			this.max = max;
			this.widths = widths;
		}
		
		public int InRange(double check) {
			if (check >= min && check <= max)
				return (int) ((check - min) / widths);
			return -1;
		}
		
		public int Count {
			get {
				return (int) ((max - min) / widths);
			}
		}
	}
}

