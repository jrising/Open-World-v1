using System;
namespace OpenWorldModel
{
	public class Dims
	{
		/* TODO: Work on concept of convertible dimensions:

		// time variables
		public static IDimensions s = GlobalDimensions.get("s");
		public static IDimensions min = GlobalDimensions.Add("min", s*60);
		public static IDimensions hr = GlobalDimensions.Add("hr", min*60);
		public static IDimensions day = GlobalDimensions.Add("day", hr*24);
		public static IDimensions month = GlobalDimensions.Add("month", Depends(theDate, day*365.25/12));
		public static IDimensions year = GlobalDimensions.Add("year", Depends(theDate, day*365.25));
		
		// space variables
		public static IDimensions m = GlobalDimensions.get("m");
		public static IDimensions cm = GlobalDimensions.Add("cm", m/100);
		public static IDimensions mm = GlobalDimensions.Add("mm", m/1000);
		public static IDimensions km = GlobalDimensions.Add("km", m*1000);
		public static IDimensions inch = GlobalDimensions.Add("in", cm*2.54);
		public static IDisposable ft = GlobalDimensions.Add("ft", inch*12);
		public static IDisposable mi = GlobalDimensions.Add("mi", ft*5280);
		
		// mass variables
		public static IDimensions kg = GlobalDimensions.get("kg");
		*/
		
		public Dims()
		{
		}
	}
}

