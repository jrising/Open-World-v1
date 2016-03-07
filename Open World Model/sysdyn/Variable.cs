using System;
using System.Text;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class Variable
	{
		protected string name;
		protected IDimensions dims;
		
		public Variable(string name, IDimensions dims)
		{
			this.name = name;
			this.dims = dims;
		}
		
		public override string ToString()
		{
			return string.Format ("{0} [{1}]", Name, Dimensions);
		}
		
		public string Name {
			get {
				return name;
			}
		}
		
		public IDimensions Dimensions {
			get {
				return dims;
			}
		}
				
		// Mathematics
		
		public static Variable operator-(Variable a) {
			return new Variable("-" + a.Name, a.Dimensions);
		}
		
		public static Variable operator+(Variable a, Variable b) {
			if (!a.Dimensions.Equals(b.Dimensions))
				throw new ArgumentException("Dimensions mismatch to +");
			return new Variable("(" + a.Name + " + " + b.Name + ")", a.Dimensions);
		}
		
		public static Variable operator-(Variable a, Variable b) {
			if (!a.Dimensions.Equals(b.Dimensions))
				throw new ArgumentException("Dimensions mismatch to -");
			return new Variable("(" + a.Name + " - " + b.Name + ")", a.Dimensions);
		}
		
		public static Variable operator*(Variable a, Variable b) {
			return new Variable(a.Name + " " + b.Name, a.Dimensions.Times(b.Dimensions));
		}

		public static Variable operator/(Variable a, Variable b) {
			return new Variable("(" + a.Name + ")/(" + b.Name + ")", a.Dimensions.DividedBy(b.Dimensions));
		}
	}
}

