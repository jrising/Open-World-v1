using System;
using OpenWorldModel.Dimensions;

namespace OpenWorldModel
{
	public class DataMatrix : Matrix
	{
		protected double[,] values;
		
		public DataMatrix(string name, IDimensions dims, double[,] values)
			: base(name, dims, (uint) values.GetLength(0), (uint) values.GetLength(1))
		{
			this.values = values;
		}
	}
}

