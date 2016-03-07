using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace OpenWorldModel
{
	public class CollectCountry
	{
		protected string country;
		protected Dictionary<string, KeyValuePair<double[], double[]>> indicators;
		
		public CollectCountry(string country)
		{
			this.country = country;
			indicators = new Dictionary<string, KeyValuePair<double[], double[]>>();
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
			
			double[] row = reader.FindLabeledRow(country);
			if (row != null)
				indicators[header.Value.Key] = new KeyValuePair<double[], double[]>(header.Value.Value, row);
		}
		
		public string HtmlDisplay() {
			// Find earliest year
			double earliest = int.MaxValue;
			foreach (KeyValuePair<string, KeyValuePair<double[], double[]>> kvp in indicators)
				for (int ii = 0; ii < Math.Min(kvp.Value.Key.Length, kvp.Value.Value.Length); ii++)
					if (!double.IsNaN(kvp.Value.Value[ii])) {
						earliest = Math.Min(earliest, kvp.Value.Key[ii]);
						break;
					}
			
			StringBuilder result = new StringBuilder();
			result.AppendLine("<table><tr>");
			result.AppendLine("<th>Year</th>");
			
			// Display header row
			foreach (KeyValuePair<string, KeyValuePair<double[], double[]>> kvp in indicators)
				result.AppendFormat("<th>{0}</th>", kvp.Key);
			result.AppendLine("</tr>");
			
			// Display each year
			for (int year = (int) earliest; year <= 2011; year++) {
				result.AppendFormat("<tr><td>{0}</td>", year);
				foreach (KeyValuePair<string, KeyValuePair<double[], double[]>> kvp in indicators) {
					bool found = false;
					for (int ii = 0; ii < kvp.Value.Key.Length; ii++)
						if (kvp.Value.Key[ii] == year) {
							if (kvp.Value.Value.Length > ii)
								result.AppendFormat("<td>{0}</td>", double.IsNaN(kvp.Value.Value[ii]) ? "" : kvp.Value.Value[ii].ToString());
							else
								result.Append("<td></td>");
							found = true;
						}

					if (!found)
						result.AppendLine("<td></td>");
				}
				result.AppendLine("</tr>");
			}
			result.AppendLine("</table>");
			
			return result.ToString();
		}
		
		public void DropMislabled() {
			indicators.Remove("");
			indicators.Remove("Row Labels");
			indicators.Remove("Country");
		}
		
		public void NormalizeYearChanges() {
			NormalizeYearChangesByCountry(indicators);
		}
		
		public static void NormalizeYearChangesByCountry(Dictionary<string, KeyValuePair<double[], double[]>> indicators) {
			Dictionary<string, KeyValuePair<double[], double[]>> replace = new Dictionary<string, KeyValuePair<double[], double[]>>();
			
			foreach (KeyValuePair<string, KeyValuePair<double[], double[]>> kvp in indicators) {
				double[] results = new double[Math.Min(kvp.Value.Key.Length, kvp.Value.Value.Length)];
				if (results.Length == 0)
					continue;
				
				double sum = 0;
				int count = 0;
				results[0] = double.NaN;
				for (int ii = 1; ii < results.Length; ii++) {
					if (!double.IsNaN(kvp.Value.Value[ii]) && !double.IsNaN(kvp.Value.Value[ii - 1]) &&
					    kvp.Value.Key[ii] - kvp.Value.Key[ii - 1] == 1) {
						results[ii] = Math.Abs(kvp.Value.Value[ii] - kvp.Value.Value[ii - 1]);
						sum += results[ii];
						count++;
					} else
						results[ii] = double.NaN;
				}
				
				if (sum > 0 && count > 1) {
					for (int ii = 1; ii < results.Length; ii++)
						results[ii] /= (sum / count);

					replace[kvp.Key] = new KeyValuePair<double[], double[]>(kvp.Value.Key, results);
				}
			}
			
			indicators = replace;
		}

		public static double GetYearValue(double year, KeyValuePair<double[], double[]> kvp) {
			return GetYearValue(year, kvp.Key, kvp.Value);
		}

		public static double GetYearValue(double year, double[] years, double[] values) {
			for (int ii = 0; ii < Math.Min(years.Length, values.Length); ii++)
				if (years[ii] == year)
					return values[ii];
			
			return double.NaN;
		}
		
		// returns lower, upper
		public static KeyValuePair<double, double>[] GetBorderValues(double year, KeyValuePair<double[], double[]> kvp) {
			return GetBorderValues(year, kvp.Key, kvp.Value);
		}
		
		public static KeyValuePair<double, double>[] GetBorderValues(double year, double[] years, double[] values) {
			for (int ii = 0; ii < Math.Min(years.Length, values.Length); ii++) {
				if (years[ii] == year)
					return new KeyValuePair<double, double>[] { new KeyValuePair<double, double>(year, values[ii]) };
			
				if (years[ii] > year) {
					if (ii > 0)
						return new KeyValuePair<double, double>[] { new KeyValuePair<double, double>(years[ii-1], values[ii-1]),
																	 new KeyValuePair<double, double>(years[ii], values[ii]) };
					else
						return new KeyValuePair<double, double>[] { new KeyValuePair<double, double>(years[ii], values[ii]) };
				}
			}
			
			int jj = Math.Min(years.Length, values.Length) - 1;
			return new KeyValuePair<double, double>[] { new KeyValuePair<double, double>(years[jj], values[jj]) };
		}
	}
}

