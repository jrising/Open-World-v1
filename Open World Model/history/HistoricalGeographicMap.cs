using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public abstract class HistoricalGeographicMap<T> : IHistoricalGeographicMap
	{
		protected GeographicMap<T>[] maps;
		protected DividedRange time;
		
		public HistoricalGeographicMap(GeographicMap<T>[] maps, DividedRange time)
		{
			this.maps = maps;
			this.time = time;
		}
		
		public abstract string Name { get; }
		public abstract Measurement get(SpaceTime st);
		public abstract IDimensions Dimensions { get; }
	}
}

