using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public interface IHistoricalGeographicMap
	{
		string Name { get; }
		Measurement get(SpaceTime st);
		IDimensions Dimensions { get; }
	}
}

