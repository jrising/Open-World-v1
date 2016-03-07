using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWorldModel.Dimensions
{
	public class GlobalDimensions : AbstractDimensions
	{
		protected static Dictionary<string, IDimensions> dict = new Dictionary<string, IDimensions>();
		protected static IDimensions time;
		
		public GlobalDimensions(string name)
			: base(name) {
		}
				
		public GlobalDimensions(KeyValuePair<IDimensions, double>[] factors)
			: base(factors) {
		}
		
		public GlobalDimensions(string name, KeyValuePair<IDimensions, double>[] factors)
			: base(name, factors) {
		}
		
		public static IDimensions get(string name) {
			IDimensions dims = null;
			if (dict.TryGetValue(name, out dims))
				return dims;

			dims = new GlobalDimensions(name);
			dict.Add(name, dims);
			return dims;
		}
		
		public static IDimensions Add(string name, IDimensions dims) {
			if (dict.ContainsKey(name))
				throw new ArgumentException("GlobalDimension " + name + " is already defined");
			IDimensions newdims = new GlobalDimensions(name, dims.Factors);
			dict.Add(name, newdims);
			return newdims;
		}
		
		public static IDimensions Time {
			get {
				if (time == null)
					time = get("time");
				return time;
			}
		}
	}
}

