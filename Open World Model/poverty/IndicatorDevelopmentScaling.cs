using System;
using System.IO;
using System.Collections.Generic;

namespace OpenWorldModel
{
	public class IndicatorDevelopmentScaling
	{
		// country -> indicator -> [years, values]
		protected Dictionary<string, Dictionary<string, KeyValuePair<double[], double[]>>> bycountry;
		protected int BIN_COUNT = 10;
		
		public IndicatorDevelopmentScaling()
		{
			bycountry = new Dictionary<string, Dictionary<string, KeyValuePair<double[], double[]>>>();
		}
		
		public void AddDirectory(string path) {
			foreach (string file in Directory.GetFiles(path))
				AddFile(Path.Combine(path, file));
		}
		
		public void AddFile(string path) {
			DataReader reader = new DataReader(path);
			KeyValuePair<string, double[]>? header = reader.ReadLabeledRow();
			if (!header.HasValue)
				return;
			
			string indicator = header.Value.Key;
			if (indicator == "" || indicator == "Country" || indicator == "Row Labels")
				return;
			
			KeyValuePair<string, double[]>? row;
			while ((row = reader.ReadLabeledRow()) != null) {
				if (!row.HasValue)
					break;
				
				Dictionary<string, KeyValuePair<double[], double[]>> byindicator;
				if (!bycountry.ContainsKey(row.Value.Key))
					bycountry[row.Value.Key] = new Dictionary<string, KeyValuePair<double[], double[]>>();
				byindicator = bycountry[row.Value.Key];

				byindicator[header.Value.Key] = new KeyValuePair<double[], double[]>(header.Value.Value, row.Value.Value);
			}
		}
		
		public void NormalizeYearChanges() {
			foreach (KeyValuePair<string, Dictionary<string, KeyValuePair<double[], double[]>>> kvp in bycountry) {
				CollectCountry.NormalizeYearChangesByCountry(kvp.Value);
			}
		}
		
		// indicator -> bin -> points
		public Dictionary<string, List<double>[]> GetHDIBins() {
			Dictionary<string, List<double>[]> result = new Dictionary<string, List<double>[]>();
			
			foreach (KeyValuePair<string, Dictionary<string, KeyValuePair<double[], double[]>>> countryvp in bycountry) {
				foreach (KeyValuePair<string, KeyValuePair<double[], double[]>> indicatorvp in countryvp.Value) {
					if (!result.ContainsKey(indicatorvp.Key)) {
						result[indicatorvp.Key] = new List<double>[BIN_COUNT];
						for (int bin = 0; bin < BIN_COUNT; bin++)
							result[indicatorvp.Key][bin] = new List<double>();
					}
					List<double>[] bins = result[indicatorvp.Key];
				
					for (int ii = 0; ii < Math.Min(indicatorvp.Value.Key.Length, indicatorvp.Value.Value.Length); ii++) {
						if (double.IsNaN(indicatorvp.Value.Key[ii]) || double.IsNaN(indicatorvp.Value.Value[ii]) || indicatorvp.Value.Key[ii] < 1900)
							continue;
						
						// Determine the bin
						double hdi = EstimateHDI(countryvp.Key, indicatorvp.Value.Key[ii]);
						if (double.IsNaN(hdi) || hdi == 1)
							continue;
						
						int bin = (int) (hdi * BIN_COUNT);
						bins[bin].Add(indicatorvp.Value.Value[ii]);
					}
				}
			}
			
			return result;
		}
		
		public double[,] ValuesToPDF(List<double> values) {
			values.Sort();
			// Count unique values
			// are all values the same?
			bool allsame = true;
			for (int ii = 1; ii < values.Count; ii++)
				if (values[ii] != values[0]) {
					allsame = false;
					break;
				}
			if (allsame)
				return new double[1,2] { { values[0], 1 }};

			// skip any zeros
			int find = 0; // find will be after first nonzero
			while (find < values.Count)
				if (values[find++] != 0)
					break;
			int uniques = 1;
			for (int ii = find; ii < values.Count; ii++) {
				if (values[ii] == values[ii - 1])
					continue;
				uniques++;
			}
			double[,] points = new double[uniques, 2];
			points[0, 0] = values[0];
			points[0, 1] = (1.0 / (values.Count)) / values[find-1];
			double contrib = (1.0 / (values.Count)) / values[find-1];
			int jj = 0;
			for (int ii = find; ii < values.Count; ii++) {
				if (values[ii] == values[ii - 1])
					points[jj, 1] += contrib;
				else {
					jj++;
					points[jj, 0] = values[ii];
					points[jj, 1] = contrib = (1.0 / (values.Count)) / (values[ii] - values[ii - 1]);
				}
			}
			
			return points;
		}
		
		public double[] GetPowerLawRSqr(List<double>[] indicatorBins) {
			double[] bins = new double[BIN_COUNT];

			for (int bin = 0; bin < BIN_COUNT; bin++) {
				if (indicatorBins[bin].Count == 0) {
					bins[bin] = double.NaN;
					continue;
				}
				
				double[,] points = ValuesToPDF(indicatorBins[bin]);
				double rsqr = OLS.PointsRSqr(points);
				bins[bin] = rsqr;
			}
			
			return bins;
		}
		
		public double EstimateHDI(string country, double year) {
			// find the year above and below
			if (!bycountry.ContainsKey(country) || !bycountry[country].ContainsKey("HDI") ||
				bycountry[country]["HDI"].Value.Length == 0)
				return double.NaN;

			KeyValuePair<double, double>[] points = CollectCountry.GetBorderValues(year, bycountry[country]["HDI"]);
			if (points.Length == 1 && points[0].Key == year)
				return points[0].Value;

			if (!bycountry[country].ContainsKey("Total GDP- PPP") || !bycountry[country].ContainsKey("Total population"))
				return double.NaN;
			
			double inYear = CollectCountry.GetYearValue(year, bycountry[country]["Total GDP- PPP"]) / 
				CollectCountry.GetYearValue(year, bycountry[country]["Total population"]);

			if (points.Length == 1) {				
				double inHdiYear = CollectCountry.GetYearValue(points[0].Key, bycountry[country]["Total GDP- PPP"]) / 
					CollectCountry.GetYearValue(points[0].Key, bycountry[country]["Total population"]);
				
				return Math.Min(1, points[0].Value * inYear / inHdiYear);
			} else {
				double inHdiLowerYear = CollectCountry.GetYearValue(points[0].Key, bycountry[country]["Total GDP- PPP"]) / 
					CollectCountry.GetYearValue(points[0].Key, bycountry[country]["Total population"]);
				double lowerEstimate = Math.Min(1, points[0].Value * inYear / inHdiLowerYear);
				
				double inHdiUpperYear = CollectCountry.GetYearValue(points[1].Key, bycountry[country]["Total GDP- PPP"]) / 
					CollectCountry.GetYearValue(points[1].Key, bycountry[country]["Total population"]);
				double upperEstimate = Math.Min(1, points[0].Value * inYear / inHdiUpperYear);
				
				if (double.IsNaN(lowerEstimate))
					return upperEstimate;
				if (double.IsNaN(upperEstimate))
					return lowerEstimate;
				
				return ((year - points[0].Key) * upperEstimate + (points[1].Key - year) * lowerEstimate) /
						(points[1].Key - points[0].Key);
			}
		}
	}
}

