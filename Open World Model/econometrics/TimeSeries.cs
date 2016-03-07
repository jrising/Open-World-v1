using System;
namespace OpenWorldModel
{
	public class TimeSeries
	{
		protected ColumnVector values;
		protected DateTime start;
		
		public TimeSeries(ColumnVector values, DateTime start)
		{
			this.values = values;
			this.start = start;
		}
	}
}

