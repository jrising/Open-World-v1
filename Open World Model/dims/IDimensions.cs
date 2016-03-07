using System;
using System.Collections.Generic;

namespace OpenWorldModel.Dimensions
{
	public interface IDimensions
	{
		string ToString();
		bool Equals(IDimensions other);
		KeyValuePair<IDimensions, double>[] Factors { get; }

		IDimensions Times(IDimensions other);
		IDimensions DividedBy(IDimensions other);
		IDimensions RaisedTo(double power);
	}
}

