using System;
using System.IO;

namespace OpenWorldModel
{
	public class GeographicMap<T>
	{
		protected T[,] values;
		protected DividedRange longitudes;
		protected DividedRange latitudes;
		
		public delegate T ValueParser(string text);
		
		public GeographicMap(DividedRange longitudes, DividedRange latitudes)
		{
			this.longitudes = longitudes;
			this.latitudes = latitudes;
			values = new T[latitudes.Count, longitudes.Count];
		}
		
		public int LoadCSV(string filename, ValueParser parser, int r0, int c0) {
			int rr = 0;
			using (StreamReader readFile = new StreamReader(filename)) {
				string line;
				string[] row;
			
				while ((line = readFile.ReadLine()) != null) {
					row = line.Split(',');
					for (int cc = 0; cc < row.Length; cc++)
						values[rr + r0, cc + c0] = parser(row[cc]);
					rr++;
				}
			}
			
			return rr;
		}
	}
}

