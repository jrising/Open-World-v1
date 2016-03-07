using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class ColumnVector : Matrix
	{
		public ColumnVector(string name, IDimensions dims, uint num)
			: base(name, dims, num, 1)
		{
		}
	}
}

