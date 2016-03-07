using System;
namespace OpenWorldModel
{
	public class AmbiguousSystem : ISystem
	{
		protected ISystem one;
		protected ISystem two;
		protected double twoWeight;
		
		public AmbiguousSystem(ISystem one, ISystem two, double twoWeight)
		{
			this.one = one;
			this.two = two;
			this.twoWeight = twoWeight;
		}
		
		public string Name {
			get {
				return "ambiguous";
			}
		}
	}
}

